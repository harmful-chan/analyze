# Analyze ���ݷ���
analyze.core: ʹ�� .net core 3.1 ��ܡ���װ.net8������ʹ��<br>
analyze.core.win: ʹ��.net8��ܡ���װ.net8������ʹ��<br>

## Features
manage: ��̨�������ܿ����ڻ�ȡ�û���Ϣ�Լ�������¼��<br>
order: ��������ԭʼ���ݣ�����ſ�˿�����¼��<br>
daily: ����ÿ�յ������ݣ��ϱ����ݵ���ҵ΢��Ⱥ��<br>

## Architecture
```shell
bin    ��ʷ�����汾��ִ�г���ͨ����ʱ���β

��������     ��ŵ��̵�����ԭʼ����
|- 1��      1�·ݵ����ĵ�������
   |- ��˾05cn1076763119lzfae����aa    (��˾���+����cn��+���+����)�ǳ������ļ���
      |- ����.xlsx    �������ж�����¼������֧�ֳ�ʱ��ε������Բ���Ҫ�ֱ���
      |- �ſ� 01.xlsx    �������зſ��¼����Ҫ����ÿ�µ�����
      |- ...
      |- �˿� 01.xlsx    ���������˿��¼����Ҫ����ÿ�µ�����

�ۿ��¼     ���ÿһ�������ۿ��¼
|- 2024��02��16��    ʹ������ۿ������
   |- 8183800207889872.txt    ������Ӧ�ۿ��¼
   ...
|- temp.txt    ��Ҫ�ۿ�Ķ�����¼���ͻ�ID{�ո�}������

����ͳ��     �����������¼
|- 1��    1�·ݵ�����¼
   |- 202401cn1079617241kcsae��ɽ�о�������Ƽ����޹�˾aa.xlsx    ����+cn+��˾��+�ǳ�.xlsx

ÿ������    ʹ��Ӱ����ȡ�ĵ������ݡ�ʹ��excel�ļ�����
|- 2024��02��25��    ִ�г�����ʱ��
   |- ��˾05cn1076763119lzfae������������ˮ�������޹�˾aa.xlsx    (��˾���+����cn��+��˾��+����+�ǳ�)�����ļ���

�����¼    ÿ�����ⶩ��
|- 2024��1��    �˿���·�
   |- xxx.xlsx    ���˿���Ļ����ϡ����ӻ��﷢����Ϣ���ж��Ƿ�����˿

ԭʼ����    ԭʼ������¼���ɹ���¼
|- 2024��02��21��
   |- ���̼�¼.xlsx
   |- �����ܱ�.xlsx
   |- �����ɹ���.xlsx
   |- �����ɹ���.xlsx
   |- Զ�ڻ�����ʷ����.xlsx


```
## Example
### manage
```shell
# �۳�������Ӧ����
.\analyze.core.exe manage -u 5377028 --deduction  8185077759712497 8185077759712492 ...
.\analyze.core.exe manage -f "�ۿ��¼\temp.txt"
```

### order
```
# �г�ָ�������˿��¼������xlsx
#analyze.core order -a refund -r \{rawdir\} -d \{datadir\} -o \{output_dir\} -p \{folder_prefix\} -y \{year\} -m \{month\} [-l]
.\analyze.exe order -a refund -r "ԭʼ����\2024��02��21��\" -d "��������\1��" -o "�����¼\2024��1��" -p "��˾23cn1077984038qwgae����Ⱥ��aa" -y 2024 -m 1 -l

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

# �г����󲢱��� xlsx
.\analyze.core.exe order -a lend -r "ԭʼ����\2024��02��21��\" -d "��������\1��" -o "����ͳ��\1��" -p "��˾23cn1077984038qwgae����Ⱥ��aa" -y 2024 -m 1 -l
��˾23cn1077984038qwgae���ڰ���Ⱥ���Ƽ����޹�˾aa  Lend:46104.35 Cost:37231.73 Profit:8872.62  Rate:0.24
���� ����ͳ��\1��\202401cn1077984038qwgae���ڰ���Ⱥ���Ƽ����޹�˾aa.xlsx
```

### daily
����ÿ��ĵ������ݱ��浽һ��xlsx�ļ��С�����ǰ�����й̶���ʽ
```
A1:��˾�� B1:�����ǳ�
A2:��Ӫ���� B2:�������� C2:������� D2:�¼�����
A3:24Сʱ�ظ��� B3:�ɽ����� C3:�����԰� D3:������ E3:������ F3:72Сʱ������
A4:�ſ� B4:���� C4:��; D4:�ſ�

���¼�¼���Բ����Ⱥ�
�ʽ����ּ�¼
��;������¼
���׶�����¼
������¼
```
```
.\analyze.core.exe daily -d ".\ÿ������\2024��02��25��\" --shop-info ".\������Ϣ.xlsx" --list-company
| Company  | CN | Opera  | UP   | Check | Down | IM24    | Good    | Dispute | Wrong | Dispute Line | F30 | D30 | Exp30 | Fin | Dis | Close | Talk | Palt | All | Ready Line   | New | Ready | Wait | Lead     | Freeze  | OnWay    | Arre    | Lose | Get   | Reality | Balance  |
|----------|----|--------|------|-------|------|---------|---------|---------|-------|--------------|-----|-----|-------|-----|-----|-------|------|------|-----|--------------|-----|-------|------|----------|---------|----------|---------|------|-------|---------|----------|
| ������ó  | aa | �º���  | 68   | 0     | 2491 | 0.00%   | 0.00%   | 0.00%   | 0.00% |              | 0   | 0   | 0     | 105 | 21  | 21    | 0    | 0    | 0   |              | 0   | 0     | 0    | 2849.2   | 0       | 0        | 0       | 0    | 8600  | 12999   | 4723.87  |
| ������ˮ  | aa | ֣����  | 1990 | 0     | 4340 | 73.68%  | 82.14%  | 29.82%  | 0.00% | 02��03:58:34 | 11  | 17  | 29    | 215 | 81  | 77    | 4    | 0    | 3   | 09��06:13:55 | 1   | 0     | 0    | 3591.84  | 0       | 11698.47 | 0       | 4380 | 23382 | 39000   | 1310.92  |
| С������  | aa | ������  | 1470 | 0     | 755  | 75.00%  | 100.00% | 18.18%  | 0.00% | 02��23:53:49 | 6   | 2   | -1    | 265 | 76  | 75    | 1    | 0    | 2   | 09��11:33:03 | 0   | 0     | 0    | 1545.97  | 1070.25 | 9124.56  | 0       | 497  | 20665 | 32000   | 10152.67 |
| ��˧����  | aa | ������  | 1666 | 0     | 222  | 90.98%  | 95.56%  | 28.00%  | 0.00% | 02��17:46:54 | 38  | 20  | 9     | 228 | 55  | 48    | 6    | 1    | 3   | 09��10:54:56 | 2   | 0     | 0    | 1462.66  | 0       | 8977.22  | 0       | 1745 | 17440 | 76163   | 3004.11  |
...

.\analyze.core.exe daily -d ".\ÿ������\2024��02��25��\" --shop-info ".\������Ϣ.xlsx" --list-company
| Company  | Lead     | Freeze   | OnWay    | Arre    | Lose     | Get   | Reality | Balance | Profit    |
|----------|----------|----------|----------|---------|----------|-------|---------|---------|-----------|
| ������ó  | 2849.20  | 0.00     | 0.00     | 0.00    | 0.00     | 8600  | 12999   | 4724    | 3174.07   |
| ������ˮ  | 3591.84  | 0.00     | 11698.47 | 0.00    | 4380.00  | 23382 | 39000   | 1311    | -3396.77  |
| С������  | 1545.97  | 1070.25  | 9124.56  | 0.00    | 497.00   | 20665 | 32000   | 10153   | 8991.20   |
| ��˧����  | 3371.16  | 0.00     | 14810.44 | 1018.91 | 2626.00  | 31018 | 76163   | 3004    | -27604.20 |
| ������  | 4135.34  | 1959.51  | 25239.41 | 0.00    | 3992.00  | 21761 | 45500   | 3193    | 4836.46   |
| ��Գ����  | 6617.92  | 1696.47  | 31754.91 | 0.00    | 4805.00  | 33276 | 65000   | 264     | 2107.85   |
| ������ó  | 5310.51  | 981.86   | 47318.97 | 0.00    | 8660.00  | 22500 | 62000   | 787     | 5256.55   |
...

.\analyze.core.exe daily -d ".\ÿ������\2024��02��25��\" --shop-info ".\������Ϣ.xlsx" --upload-order    # �ϴ�����

.\analyze.core.exe daily -d ".\ÿ������\2024��02��25��\" --shop-info ".\������Ϣ.xlsx" --upload-info    # �ϴ�����

```
### purchase
��ɹ�����
```
.\analyze.core.exe purchase [--br-purchase-filenmae "Z:\���ݲɼ�\ԭʼ����\2024��02��25��\�����ɹ���-������1.xlsx"] -l -b Leo
| I  | date       | Order | Pending | Processing | Solved | Cancel | Cut |
|----|------------|-------|---------|------------|--------|--------|-----|
| 1  | 0001-01-01 | 179   | 165     | 0          | 1      | 0      | 13  |
| 2  | 2023-11-20 | 2     | 0       | 0          | 2      | 0      | 0   |
| 3  | 2023-11-24 | 1     | 0       | 0          | 1      | 0      | 0   |
| 4  | 2023-11-25 | 1     | 0       | 0          | 1      | 0      | 0   |
| 5  | 2023-11-27 | 2     | 0       | 0          | 2      | 0      | 0   |
| 6  | 2023-11-30 | 3     | 0       | 0          | 3      | 0      | 0   |
| 7  | 2023-12-01 | 6     | 0       | 0          | 5      | 1      | 0   |
| 8  | 2023-12-04 | 4     | 0       | 0          | 4      | 0      | 0   |
| 9  | 2023-12-05 | 2     | 0       | 0          | 1      | 0      | 1   |
| 10 | 2023-12-06 | 1     | 0       | 0          | 1      | 0      | 0   |
| 11 | 2023-12-07 | 3     | 0       | 0          | 3      | 0      | 0   |
| 12 | 2023-12-08 | 3     | 0       | 0          | 3      | 0      | 0   |
| 13 | 2023-12-11 | 5     | 0       | 0          | 5      | 0      | 0   |
| 14 | 2023-12-12 | 5     | 0       | 0          | 4      | 0      | 1   |
| 15 | 2023-12-13 | 3     | 0       | 0          | 3      | 0      | 0   |
| 16 | 2023-12-14 | 5     | 0       | 0          | 4      | 0      | 1   |
| 17 | 2023-12-15 | 14    | 0       | 0          | 13     | 0      | 1   |
| 18 | 2023-12-16 | 11    | 0       | 0          | 10     | 0      | 1   |
| 19 | 2023-12-18 | 34    | 0       | 0          | 25     | 0      | 9   |
| 20 | 2023-12-19 | 27    | 0       | 2          | 19     | 0      | 6   |
| 21 | 2023-12-20 | 23    | 0       | 2          | 13     | 1      | 7   |
| 22 | 2023-12-21 | 35    | 0       | 6          | 20     | 2      | 7   |
| 23 | 2023-12-22 | 38    | 1       | 2          | 23     | 0      | 12  |
| 24 | 2023-12-23 | 27    | 0       | 1          | 19     | 0      | 7   |
| 25 | 2023-12-25 | 41    | 4       | 5          | 22     | 5      | 5   |
| 26 | 2023-12-26 | 32    | 0       | 2          | 21     | 1      | 8   |
| 27 | 2023-12-27 | 72    | 1       | 7          | 41     | 12     | 11  |
| 28 | 2023-12-28 | 51    | 0       | 3          | 28     | 1      | 19  |
...
```


## tag
### daily

```shell
#202402151726
analyze.core.202402151726.exe daily -d "D:\BaiduSyncdisk\Desktop\���ݲɼ�\ÿ������\2024��02��15��" -l -u

#202402170154
analyze.core.202402170154.exe daily  -d "F:\BaiduSyncdisk\Desktop\���ݲɼ�\ÿ������\2024��02��16��" --list-opear
analyze.core.202402170154.exe daily  -d "F:\BaiduSyncdisk\Desktop\���ݲɼ�\ÿ������\2024��02��16��" --list-company
analyze.core.202402170154.exe daily  -d "F:\BaiduSyncdisk\Desktop\���ݲɼ�\ÿ������\2024��02��16��" --list-profit
analyze.core.202402170154.exe daily  -d "F:\BaiduSyncdisk\Desktop\���ݲɼ�\ÿ������\2024��02��16��" --upload-order
analyze.core.202402170154.exe daily  -d "F:\BaiduSyncdisk\Desktop\���ݲɼ�\ÿ������\2024��02��16��" --upload-info

#202402210011
#202402221333
analyze.core.202402221333.exe daily -d "Z:\���ݲɼ�\ÿ������\2024��02��20��\" --shop-info "Z:\���ݲɼ�\������Ϣ.xlsx" {--list-profit | --list-opear | --list-company | --upload-order --upload-info}
```


### manage
```
#202402160626
analyze.core.202402160626.exe manage -u 5377028 --deduction 8185077759712497 8185077759712492
analyze.core.202402160626.exe manage -f ".\�ۿ��¼\temp.txt"
```
### purchase
```
#202402260120
analyze.core.202402260120.exe manage -u 5377028 --deduction 8185077759712497 8185077759712492
analyze.core.202402260120.exe manage -f ".\�ۿ��¼\temp.txt"
```




### ��������Ҫ�ֶ�
```
### �����ܱ� : ������ ����״̬ �ۿ�׺� �ۿ���
### ���̶��� : ������ ����ʱ��  ������� �ջ����� ����ʱ�� 
### ���̷ſ� : ������ �ſ��� ����Ӷ�� ����Ӷ�� cashback ����ʱ��
### �����˿� : ������ �ɽ���� �˿��� �˿���Դ �˿�ɹ�ʱ��
### �����ɹ��� : ������ ״̬
### �����ɹ��� : ������ ��������
### ��ֵ��¼ : ���׺� ���ʽ��
### �ۿ��¼ : ���׺� ���ʽ��
### ������ : ֧���ܽ�� �����ܽ�� 
```


## other
```
analyze 
> 2023-01-25 00:00:00 2023-08-08 00:00:00 5376914 cn1077459042pzzae 
pro_ra:0.3 pro_c:6r  pc_br:256r tc_br:5w1
����������֧����������������������֧��������
�����ܶ�����ܶ
�귢�����������귢�������귢�ۿ��ܶ�����ۿ��ܶ
��;������������;��������;�ܶ������;�ܶ
�ſ������������ſ��������ſ��ܶ�����ſ��ܶ
�����������������������������ܶ���������ܶ
�˿������������˿��������˿��ܶ�����˿��ܶ
�����˿����������������˿������������˿��ܶ�����˿��ܶ


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



