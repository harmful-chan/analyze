using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Clients.Webhook
{
    public delegate void UpLoadProgress(int percentage);
    public delegate void UpLoadCompleteDel();
    internal static class FileType
    {
        static string url = @"https://qyapi.weixin.qq.com/cgi-bin/webhook/upload_media?key={0}&type=file";
        internal static string UpLoadFileKey { get; set; }


        public static event UpLoadProgress upLoadProgressEvent;
        public static event UpLoadCompleteDel upLoadCompleteDelEvent;
        internal static async Task<OperationResult> SendFile(string FilePath)
        {
            string Posturl = string.Format(url, UpLoadFileKey);
            OperationResult result = new OperationResult();
            FormItemModel formItem = new FormItemModel()
            {
                Key = Path.GetFileName(FilePath),
                FileContent = File.OpenRead(FilePath),
                FileName = Path.GetFileName(FilePath),
                Value = "",

                IsFile = true,
            };
            if (formItem.FileContent.Length >= 1024 * 1024 * 20)
            {
                result.IsSuccess = false;
                result.ErrorCode = -2;
                result.Content = "文件最大不能超过20M";
            }
            string re = await PostForm(Posturl, formItem);
            dynamic jObect = JsonConvert.DeserializeObject<dynamic>(re);
            string media_id = jObect.media_id;
            if (media_id == null)
            {
                result.IsSuccess = false;
                result.ErrorCode = jObect.errcode;
                result.Content = jObect.errmsg;
                return result;
            }
            return await TextType.SendTextFile(media_id);
        }


        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formItems">Post表单内容</param>
        /// <param name="cookieContainer"></param>
        /// <param name="timeOut">默认20秒</param>
        /// <param name="encoding">响应内容的编码类型（默认utf-8）</param>
        /// <returns></returns>
        static async Task<string> PostForm(string url, FormItemModel formItems, CookieContainer cookieContainer = null, string refererUrl = null, Encoding encoding = null, int timeOut = 60000)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            #region 初始化请求对象
            request.Method = "POST";
            request.Timeout = timeOut;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
            if (!string.IsNullOrEmpty(refererUrl))
                request.Referer = refererUrl;
            if (cookieContainer != null)
                request.CookieContainer = cookieContainer;
            #endregion

            string boundary = "----" + DateTime.Now.Ticks.ToString("x");//分隔符
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            //请求流
            var postStream = new MemoryStream();
            #region 处理Form表单请求内容
            //是否用Form上传文件
            var formUploadFile = formItems != null;
            if (formUploadFile)
            {
                //文件数据模板
                string fileFormdataTemplate =
                    "\r\n--" + boundary +
                    "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
                    "\r\nContent-Type: application/octet-stream" +
                    "\r\n\r\n";
                //文本数据模板
                string dataFormdataTemplate =
                    "\r\n--" + boundary +
                    "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                    "\r\n\r\n{1}";
                string formdata = null;
                if (formItems.IsFile)
                {
                    //上传文件
                    formdata = string.Format(
                        fileFormdataTemplate,
                        formItems.Key, //表单键
                        formItems.FileName);
                }
                else
                {
                    //上传文本
                    formdata = string.Format(
                        dataFormdataTemplate,
                        formItems.Key,
                        formItems.Value);
                }

                //统一处理
                byte[] formdataBytes = null;
                //第一行不需要换行
                if (postStream.Length == 0)
                    formdataBytes = Encoding.UTF8.GetBytes(formdata.Substring(2, formdata.Length - 2));
                else
                    formdataBytes = Encoding.UTF8.GetBytes(formdata);
                postStream.Write(formdataBytes, 0, formdataBytes.Length);

                //写入文件内容
                if (formItems.FileContent != null && formItems.FileContent.Length > 0)
                {
                    using (var stream = formItems.FileContent)
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead = 0;
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            postStream.Write(buffer, 0, bytesRead);
                        }
                    }
                }
                //结尾
                var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                postStream.Write(footer, 0, footer.Length);

            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            #endregion

            request.ContentLength = postStream.Length;

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;
                //直接写入流
                Stream requestStream = request.GetRequestStream();

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                long totalBytesRead = 0;
                await Task.Run(() =>
                {
                    while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                        totalBytesRead += bytesRead;

                        // 计算上传进度
                        int progressPercentage = (int)(totalBytesRead * 100 / postStream.Length);
                        upLoadProgressEvent?.Invoke(progressPercentage);
                    }
                });
                ////debug
                //postStream.Seek(0, SeekOrigin.Begin);
                //StreamReader sr = new StreamReader(postStream);
                //var postStr = sr.ReadToEnd();
                postStream.Close();//关闭文件访问
                upLoadCompleteDelEvent?.Invoke();
            }
            #endregion
            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                if (cookieContainer != null)
                {
                    response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
                }

                return await Task.Run(() =>
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.UTF8))
                        {
                            string retString = myStreamReader.ReadToEnd();
                            return retString;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                var json = new
                {
                    errcode = -4,
                    errmsg = ex.Message,
                };
                return json.ToString();
            }
        }
    }


}
