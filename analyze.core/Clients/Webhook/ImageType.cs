using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Clients.Webhook
{
    internal static class ImageType
    {
        internal static string RobotUrl { get; set; }
        internal static async Task<OperationResult> SendPhoto(string FilePath)
        {
            OperationResult result = new OperationResult();
            bool isphoto = IsPhoto(FilePath);
            if (!isphoto)
            {
                result.IsSuccess = false;
                result.ErrorCode = -1;
                result.Content = "不是图片文件";
                return result;
            }
            long filebytessize = new FileInfo(FilePath).Length;
            if (filebytessize >= 1024 * 1024 * 20)
            {
                result.IsSuccess = false;
                result.ErrorCode = -2;
                result.Content = "文件最大不能超过2M";
                return result;
            }
            Image image = Image.FromFile(FilePath);
            return await SendPhoto(RobotUrl, image);
        }

        static async Task<OperationResult> SendPhoto(string RobotUrl, Image image)
        {
            OperationResult result = new OperationResult();
            string recData = await PostAsync(RobotUrl, getImageTypeJson(image));
            dynamic json = JsonConvert.DeserializeObject<dynamic>(recData);
            int errorcode = json.errcode;
            string errmsg = json.errmsg;
            result.IsSuccess = errorcode == 0;
            result.ErrorCode = errorcode;
            result.Content = errmsg;
            return result;
        }

        static bool IsPhoto(string FilePath)
        {
            string extension = Path.GetExtension(FilePath);

            // 支持的图片文件扩展名列表
            string[] imageExtensions = { ".jpg", ".png" };
            if (imageExtensions.Contains(extension.ToLower()))
            {
                // 使用Image类尝试加载图片文件
                try
                {
                    using (var image = Image.FromFile(FilePath))
                    {
                        return true;
                    }
                }
                catch
                {
                    // 如果加载图片文件失败，则说明该文件不是有效的图片文件
                    return false;
                }
            }
            return false;
        }

        static async Task<string> PostAsync(string url, string data)
        {
            byte[] postdata = Encoding.UTF8.GetBytes(data);
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = postdata.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(postdata, 0, postdata.Length);
            }
            string resData = "";
            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        resData = sr.ReadToEnd();
                    }
                }
            }
            return resData;
        }

        static string ImageToBase64(Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, image.RawFormat);
                byte[] imageBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        static string GetImageMD5(Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, image.RawFormat);
                byte[] imageBytes = memoryStream.ToArray();

                using (MD5 md5 = MD5.Create())
                {
                    byte[] hashBytes = md5.ComputeHash(imageBytes);
                    string md5Hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    return md5Hash;
                }
            }
        }

        static string getImageTypeJson(Image image)
        {
            var json = new
            {
                msgtype = "image",
                image = new
                {
                    base64 = ImageToBase64(image),
                    md5 = GetImageMD5(image)
                }
            };
            return JsonConvert.SerializeObject(json);
        }
    }
}
