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

analyze 2023-01-25 00:00:00 2023-08-08 00:00:00 5376914 cn1077459042pzzae 
pro_ra:0.3 pro_c:6r  pc_br:256r tc_br:5w1
pay(180:182|150:149):48424.64r|2822.7r    
mark(180|169):59448r|523r
unknow(82|50):55156.64r|523r
allin(89|30):485r|523r in(89|30):485r|523r
out1(86|12):5965r|523r out2(12|5):5965r|523r
shipped(12):299r skip(72):8488r cut(15):4815r do(485):5962r todo(85):8595r
bal(3):51000r refund(3):89682.2r rebate(3):9000r 
costus(85):54998r costbr(87):999r

loss:mark(out1) + mark(out2)
mloss:mark(out1) + mark((pay-out2)/pay)
profit:unknow(82) + allin(89) - mark(unknow) - mark(allin) - loss
spend:(unknow(pro)+allin(pro))*(rate+0.3)/rate - unknow(pro) - allin(pro)  + pro*6 + costus + costbr


