### 订单总表
	订单号 订单状态 扣款交易号 扣款金额

### 订单
	订单号 付款时间  订单金额 收货国家 发货时间 
	
### 放款
	订单号 放款金额 交易佣金 联盟佣金 cashback 结算时间

### 退款
	订单号 成交金额 退款金额 退款来源 退款成功时间

### 采购表格 巴西
	订单号 状态

### 采购表格 总表
	订单号 物流单号

### 充值记录
	交易号 单笔金额

### 扣款记录
	交易号 单笔金额

### 入款表格
	支借总金额 索赔总金额 

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

### 订单管理
analyze manage --list-client-orders all    //列出所有用户订单



.\analyze.exe refund -r "C:\BaiduSyncdisk\公司\寰球易贸\利润分成\1月\原始数据\2024-01-25" \
-d "C:\BaiduSyncdisk\公司\寰球易贸\利润分成\1月" \
-p "公司23cn1077984038qwgae百舸群创aa"  -y 2024 -m 1 -l \
-o "C:\BaiduSyncdisk\公司\寰球易贸\利润分成\1月\索赔"

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


## tag
### analyze.core.202402151726
analyze.core.202402151726.exe daily -d "D:\BaiduSyncdisk\Desktop\数据 采集\每日数据\2024年02月15日" -l -u
