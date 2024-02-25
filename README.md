# Analyze 数据分析
analyze.core 使用 .net core 3.1 框架。安装.net8可正常使用
analyze.core.win 使用.net8框架。安装.net8可正常使用

## Features
manage: 后台管理功能可用于获取用户信息以及订单记录。
order: 处理店铺原始数据，计算放款，退款，利润记录。
daily: 处理每日店铺数据，上报数据到企业微信群。

## Architecture
```shell
bin    历史发布版本可执行程序。通常以时间结尾

订单数据     存放店铺导出的原始数据
|- 1月      1月份导出的店铺数据
   |- 公司05cn1076763119lzfae誉川aa    (公司序号+店铺cn号+简称+店铺)昵称命名文件夹
      |- 订单.xlsx    店铺所有订单记录，导出支持长时间段导出所以不需要分表格。
      |- 放款 01.xlsx    店铺所有放款记录。需要按照每月导出。
      |- ...
      |- 退款 01.xlsx    店铺所有退款记录。需要按照每月导出。

扣款记录     存放每一个订单扣款记录
|- 2024年02月16日    使用命令扣款当天日期
   |- 8183800207889872.txt    订单对应扣款记录
   ...
|- temp.txt    需要扣款的订单记录。客户ID{空格}订单号

利润统计     导出的利润记录
|- 1月    1月份导出记录
   |- 202401cn1079617241kcsae佛山市炬马网络科技有限公司aa.xlsx    日期+cn+公司名+昵称.xlsx

每日数据    使用影刀获取的店铺数据。使用excel文件保存
|- 2024年02月25日    执行程序当天时间
   |- 公司05cn1076763119lzfae广州市誉川防水建材有限公司aa.xlsx    (公司序号+店铺cn号+公司名+店铺+昵称)命名文件夹

索赔记录    每月索赔订单
|- 2024年1月    退款订单月份
   |- xxx.xlsx    在退款订单的基础上。增加货物发货信息。判断是否可以退款。

原始数据    原始订单记录。采购记录
|- 2024年02月21日
   |- 店铺记录.xlsx
   |- 订单总表.xlsx
   |- 巴西采购单.xlsx
   |- 美国采购单.xlsx
   |- 远期汇率历史数据.xlsx


```
## Example
### manage
- analyze.core manage -u \{client_id\} --deduction \{order_id\} \{order_id\} ...
```shell
\analyze.core.exe manage -u 5377028 --deduction  8185077759712497 8185077759712492 ...
```

### order
- analyze.core order -a refund -r \{rawdir\} -d \{datadir\} -o \{output_dir\} -p \{folder_prefix\} -y \{year\} -m \{month\} [-l]
```
.\analyze.exe order -a refund -r "原始数据\2024年02月21日\" -d "订单数据\1月" -o "索赔记录\2024年1月" -p "公司23cn1077984038qwgae百舸群创aa" -y 2024 -m 1 -l

| I  | Order            | Turnover | Refund  | Cost     | TradeId                   | Deduction | Status | Country       | RefundTime          | OrderTime           | PaymentTime         | ShippingTime        | ReceiptTime         |
|----|------------------|----------|---------|----------|---------------------------|-----------|--------|---------------|---------------------|---------------------|---------------------|---------------------|---------------------|
| 1  | 8183162859744225 | 376.92   | 376.92  |          |                           |           |        | Brazil        | 2024-01-01 06:03:23 | 2024-01-01 05:45:00 | 2024-01-01 05:56:00 |                     |                     |
| 2  | 8180601751803179 | 263.45   | 250.28  | 160.318  | 5377003-115-20231120-2580 | 160.318   | 1      | Brazil        | 2024-01-01 21:19:27 | 2023-11-18 19:41:00 | 2023-11-18 19:41:00 | 2023-11-20 05:28:00 | 2023-12-17 05:28:00 |
| 3  | 8182074538559639 | 344.75   | 344.75  | 201.498  | 5377003-115-20231212-5590 | 201.498   |        | Brazil        | 2024-01-03 01:21:32 | 2023-12-11 03:30:00 | 2023-12-11 03:30:00 | 2023-12-12 18:31:00 |                     |
| 4  | 8182940873655844 | 392.42   | 392.42  | 295.36   | 5377003-115-20231225-5642 | 295.36    |        | Brazil        | 2024-01-07 04:36:29 | 2023-12-24 09:20:00 | 2023-12-24 09:20:00 | 2023-12-25 00:35:00 |                     |
| 5  | 8181098723039505 | 145.62   | 145.62  | 157.336  | 5377003-115-20231205-6012 | 157.336   |        | Brazil        | 2024-01-10 04:04:44 | 2023-12-04 05:57:00 | 2023-12-04 06:01:00 | 2023-12-05 01:27:00 | 2024-01-01 01:27:00 |
| 6  | 8181973781689635 | 237.86   | 237.86  | 178.636  | 5377003-115-20231221-6763 | 178.636   |        | Brazil        | 2024-01-10 19:49:02 | 2023-12-20 04:52:00 | 2023-12-20 04:55:00 |                     | 2024-01-04 17:59:00 |
| 7  | 8184000399607281 | 78.73    | 78.73   |          |                           |           |        | Brazil        | 2024-01-11 10:55:19 | 2024-01-11 10:50:00 | 2024-01-11 10:51:00 |                     |                     |
| 8  | 8181127093312526 | 1202.16  | 801.51  | 766.587  | 5377003-115-20231128-5653 | 766.587   | 1      | United States | 2024-01-12 03:08:26 | 2023-11-27 06:56:00 | 2023-11-27 06:56:00 | 2023-11-28 02:55:00 | 2023-12-20 02:55:00 |

...
```
- analyze.core order -a lead ...
```
.\analyze.core.exe order -a lend -r "原始数据\2024年02月21日\" -d "订单数据\1月" -o "利润统计\1月" -p "公司23cn1077984038qwgae百舸群创aa" -y 2024 -m 1 -l
公司23cn1077984038qwgae深圳百舸群创科技有限公司aa  Lend:46104.35 Cost:37231.73 Profit:8872.62  Rate:0.24
保存 利润统计\1月\202401cn1077984038qwgae深圳百舸群创科技有限公司aa.xlsx
```
### daily
```
.\analyze.core.exe daily -d ".\每日数据\2024年02月25日\" --shop-info ".\店铺信息.xlsx" --list-company
| Company  | CN | Opera  | UP   | Check | Down | IM24    | Good    | Dispute | Wrong | Dispute Line | F30 | D30 | Exp30 | Fin | Dis | Close | Talk | Palt | All | Ready Line   | New | Ready | Wait | Lead     | Freeze  | OnWay    | Arre    | Lose | Get   | Reality | Balance  |
|----------|----|--------|------|-------|------|---------|---------|---------|-------|--------------|-----|-----|-------|-----|-----|-------|------|------|-----|--------------|-----|-------|------|----------|---------|----------|---------|------|-------|---------|----------|
| 逐浪商贸  | aa | 陈汉锋  | 68   | 0     | 2491 | 0.00%   | 0.00%   | 0.00%   | 0.00% |              | 0   | 0   | 0     | 105 | 21  | 21    | 0    | 0    | 0   |              | 0   | 0     | 0    | 2849.2   | 0       | 0        | 0       | 0    | 8600  | 12999   | 4723.87  |
| 誉川防水  | aa | 郑淑琪  | 1990 | 0     | 4340 | 73.68%  | 82.14%  | 29.82%  | 0.00% | 02天03:58:34 | 11  | 17  | 29    | 215 | 81  | 77    | 4    | 0    | 3   | 09天06:13:55 | 1   | 0     | 0    | 3591.84  | 0       | 11698.47 | 0       | 4380 | 23382 | 39000   | 1310.92  |
| 小马奔奔  | aa | 范秋怡  | 1470 | 0     | 755  | 75.00%  | 100.00% | 18.18%  | 0.00% | 02天23:53:49 | 6   | 2   | -1    | 265 | 76  | 75    | 1    | 0    | 2   | 09天11:33:03 | 0   | 0     | 0    | 1545.97  | 1070.25 | 9124.56  | 0       | 497  | 20665 | 32000   | 10152.67 |
| 世帅电子  | aa | 陈美晴  | 1666 | 0     | 222  | 90.98%  | 95.56%  | 28.00%  | 0.00% | 02天17:46:54 | 38  | 20  | 9     | 228 | 55  | 48    | 6    | 1    | 3   | 09天10:54:56 | 2   | 0     | 0    | 1462.66  | 0       | 8977.22  | 0       | 1745 | 17440 | 76163   | 3004.11  |
...

.\analyze.core.exe daily -d ".\每日数据\2024年02月25日\" --shop-info ".\店铺信息.xlsx" --list-company
| Company  | Lead     | Freeze   | OnWay    | Arre    | Lose     | Get   | Reality | Balance | Profit    |
|----------|----------|----------|----------|---------|----------|-------|---------|---------|-----------|
| 逐浪商贸  | 2849.20  | 0.00     | 0.00     | 0.00    | 0.00     | 8600  | 12999   | 4724    | 3174.07   |
| 誉川防水  | 3591.84  | 0.00     | 11698.47 | 0.00    | 4380.00  | 23382 | 39000   | 1311    | -3396.77  |
| 小马奔奔  | 1545.97  | 1070.25  | 9124.56  | 0.00    | 497.00   | 20665 | 32000   | 10153   | 8991.20   |
| 世帅电子  | 3371.16  | 0.00     | 14810.44 | 1018.91 | 2626.00  | 31018 | 76163   | 3004    | -27604.20 |
| 翌洋互联  | 4135.34  | 1959.51  | 25239.41 | 0.00    | 3992.00  | 21761 | 45500   | 3193    | 4836.46   |
| 金猿宝传  | 6617.92  | 1696.47  | 31754.91 | 0.00    | 4805.00  | 33276 | 65000   | 264     | 2107.85   |
| 键程外贸  | 5310.51  | 981.86   | 47318.97 | 0.00    | 8660.00  | 22500 | 62000   | 787     | 5256.55   |
...

.\analyze.core.exe daily -d ".\每日数据\2024年02月25日\" --shop-info ".\店铺信息.xlsx" --upload-order    # 上传订单

.\analyze.core.exe daily -d ".\每日数据\2024年02月25日\" --shop-info ".\店铺信息.xlsx" --upload-info    # 上传数据

```




## tag
### daily
```shell
#202402151726
analyze.core.202402151726.exe daily -d "D:\BaiduSyncdisk\Desktop\数据采集\每日数据\2024年02月15日" -l -u

#202402170154
analyze.core.202402170154.exe daily  -d "F:\BaiduSyncdisk\Desktop\数据采集\每日数据\2024年02月16日" --list-opear
analyze.core.202402170154.exe daily  -d "F:\BaiduSyncdisk\Desktop\数据采集\每日数据\2024年02月16日" --list-company
analyze.core.202402170154.exe daily  -d "F:\BaiduSyncdisk\Desktop\数据采集\每日数据\2024年02月16日" --list-profit
analyze.core.202402170154.exe daily  -d "F:\BaiduSyncdisk\Desktop\数据采集\每日数据\2024年02月16日" --upload-order
analyze.core.202402170154.exe daily  -d "F:\BaiduSyncdisk\Desktop\数据采集\每日数据\2024年02月16日" --upload-info

#202402210011
#202402221333
analyze.core.202402221333.exe daily -d "Z:\数据采集\每日数据\2024年02月20日\" --shop-info "Z:\数据采集\店铺信息.xlsx" {--list-profit | --list-opear | --list-company | --upload-order --upload-info}
```

### manage
```
#202402160626
analyze.core.202402160626.exe manage -u 5377028 --deduction 8185077759712497 8185077759712492
analyze.core.202402160626.exe manage -f ".\扣款记录\temp.txt"
```



## 表格所需要字段
```
### 订单总表 : 订单号 订单状态 扣款交易号 扣款金额
### 店铺订单 : 订单号 付款时间  订单金额 收货国家 发货时间 
### 店铺放款 : 订单号 放款金额 交易佣金 联盟佣金 cashback 结算时间
### 店铺退款 : 订单号 成交金额 退款金额 退款来源 退款成功时间
### 巴西采购单 : 订单号 状态
### 美国采购单 : 订单号 物流单号
### 充值记录 : 交易号 单笔金额
### 扣款记录 : 交易号 单笔金额
### 入款表格 : 支借总金额 索赔总金额 
```


## other
```
analyze 
> 2023-01-25 00:00:00 2023-08-08 00:00:00 5376914 cn1077459042pzzae 
pro_ra:0.3 pro_c:6r  pc_br:256r tc_br:5w1
订单总数：支付总数：促销总数：促销支付总数：
订单总额：促销总额：
标发总数：促销标发总数：标发扣款总额：促销扣款总额：
在途总数：促销在途总数：在途总额：促销在途总额：
放款总数：促销放款总数：放款总额：促销放款总额：
入账总数：促销入账总数：入账总额：促销入账总额：
退款总数：促销退款总数：退款总额：促销退款总额：
部分退款总数：促销部分退款总数：部分退款总额：部分退款总额：


pay(180:182|150:149):48424.64r|2822.7r    
mark(180|169):59448r|523r
unknow(82|50):55156.64r|523r
allin(89|30):485r|523r 
in(89|30):485r|523r
out1(86|12):5965r|523r
out2(12|5):5965r|523r

shipped(12):299r skip(72):8488r cut(15):4815r do(485):5962r todo(85):8595r
bal(3):51000r refund(3):89682.2r rebate(3):9000r 
costus(85):54998r costbr(87):999r

loss:mark(out1) + mark(out2)
mloss:mark(out1) + mark((pay-out2)/pay)
profit:unknow(82) + allin(89) - mark(unknow) - mark(allin) - loss
spend:(unknow(pro)+allin(pro))*(rate+0.3)/rate - unknow(pro) - allin(pro)  + pro*6 + costus + costbr
```




