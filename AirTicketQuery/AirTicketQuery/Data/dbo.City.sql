USE [C:\MYGITREPOSITORY\STUDYSAMPLES\AIRTICKETQUERY\AIRTICKETQUERY\DATA\AIRTICKET.MDF]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[City];

CREATE TABLE [dbo].[City] (
    [C_ID]   INT IDENTITY (1, 1) NOT NULL,
    [C_NAME] NVARCHAR (20) NULL,
    [C_CODE] NVARCHAR (3)  NULL, 
    [C_CE_CODE] NVARCHAR(10) NULL,
	[C_WS_CODE] NVARCHAR(10) NULL
);
GO
INSERT INTO [dbo].[City] ([C_NAME], [C_CODE],[C_CE_CODE]) VALUES (N'北京', N'PEK',N'nay')
INSERT INTO [dbo].[City] ([C_NAME], [C_CODE],[C_CE_CODE]) VALUES (N'上海', N'SHA',N'pvg')
INSERT INTO [dbo].[City] ([C_NAME], [C_CODE],[C_CE_CODE]) VALUES (N'长沙', N'CSX',N'csx')
INSERT INTO [dbo].[City] ([C_NAME], [C_CODE],[C_CE_CODE]) VALUES (N'广州', N'CAN',N'can')
INSERT INTO [dbo].[City] ([C_NAME], [C_CODE],[C_CE_CODE]) VALUES (N'成都', N'CTU',N'ctu')
INSERT INTO [dbo].[City] ([C_NAME], [C_CODE],[C_CE_CODE]) VALUES (N'海口', N'HAK',N'hak')
INSERT INTO [dbo].[City] ([C_NAME], [C_CODE],[C_CE_CODE]) VALUES (N'重庆', N'CKG',N'ckg')
INSERT INTO [dbo].[City] ([C_NAME], [C_CODE],[C_CE_CODE]) VALUES (N'深圳', N'SZX',N'szx')
INSERT INTO [dbo].[City] ([C_NAME], [C_CODE],[C_CE_CODE]) VALUES (N'杭州', N'HGH',N'hgh')
INSERT INTO [dbo].[City] ([C_NAME], [C_CODE],[C_CE_CODE]) VALUES ( N'大连', N'DLC',N'dlc')
INSERT INTO [dbo].[City] ([C_NAME], [C_CODE],[C_CE_CODE]) VALUES ( N'武汉', N'WUH',N'wuh')
GO
update dbo.City set C_WS_CODE='YIE' where C_NAME=N'阿尔山';
update dbo.City set C_WS_CODE='AKU' where C_NAME=N'阿克苏';
update dbo.City set C_WS_CODE='AAT' where C_NAME=N'阿勒泰';
update dbo.City set C_WS_CODE='NGQ' where C_NAME=N'阿里';
update dbo.City set C_WS_CODE='AKA' where C_NAME=N'安康';
update dbo.City set C_WS_CODE='AQG' where C_NAME=N'安庆';
update dbo.City set C_WS_CODE='AOG' where C_NAME=N'鞍山';
update dbo.City set C_WS_CODE='AVA' where C_NAME=N'安顺';
update dbo.City set C_WS_CODE='AYN' where C_NAME=N'安阳';
update dbo.City set C_WS_CODE='AEB' where C_NAME=N'百色';
update dbo.City set C_WS_CODE='BSD' where C_NAME=N'保山';
update dbo.City set C_WS_CODE='BAV' where C_NAME=N'包头';
update dbo.City set C_WS_CODE='RLK' where C_NAME=N'巴彦淖尔';
update dbo.City set C_WS_CODE='BHY' where C_NAME=N'北海';
update dbo.City set C_WS_CODE='BJS' where C_NAME=N'北京';
update dbo.City set C_WS_CODE='BFU' where C_NAME=N'蚌埠';
update dbo.City set C_WS_CODE='BFJ' where C_NAME=N'毕节';
update dbo.City set C_WS_CODE='BPL' where C_NAME=N'博乐';
update dbo.City set C_WS_CODE='NBS' where C_NAME=N'长白山';
update dbo.City set C_WS_CODE='CGQ' where C_NAME=N'长春';
update dbo.City set C_WS_CODE='CGD' where C_NAME=N'常德';
update dbo.City set C_WS_CODE='BPX' where C_NAME=N'昌都';
update dbo.City set C_WS_CODE='CSX' where C_NAME=N'长沙';
update dbo.City set C_WS_CODE='CIH' where C_NAME=N'长治';
update dbo.City set C_WS_CODE='CZX' where C_NAME=N'常州';
update dbo.City set C_WS_CODE='CHG' where C_NAME=N'朝阳';
update dbo.City set C_WS_CODE='CTU' where C_NAME=N'成都';
update dbo.City set C_WS_CODE='CYI' where C_NAME=N'嘉义';
update dbo.City set C_WS_CODE='CIF' where C_NAME=N'赤峰';
update dbo.City set C_WS_CODE='CKG' where C_NAME=N'重庆';
update dbo.City set C_WS_CODE='DLU' where C_NAME=N'大理';
update dbo.City set C_WS_CODE='DLU' where C_NAME=N'大理市';
update dbo.City set C_WS_CODE='DLC' where C_NAME=N'大连';
update dbo.City set C_WS_CODE='DDG' where C_NAME=N'丹东';
update dbo.City set C_WS_CODE='DQA' where C_NAME=N'大庆';
update dbo.City set C_WS_CODE='DAT' where C_NAME=N'大同';
update dbo.City set C_WS_CODE='DAX' where C_NAME=N'达县';
update dbo.City set C_WS_CODE='DAX' where C_NAME=N'达州';
update dbo.City set C_WS_CODE='LUM' where C_NAME=N'德宏';
update dbo.City set C_WS_CODE='DEY' where C_NAME=N'德阳';
update dbo.City set C_WS_CODE='DIG' where C_NAME=N'迪庆';
update dbo.City set C_WS_CODE='DGM' where C_NAME=N'东莞';
update dbo.City set C_WS_CODE='DOY' where C_NAME=N'东营';
update dbo.City set C_WS_CODE='DNH' where C_NAME=N'敦煌';
update dbo.City set C_WS_CODE='DSN' where C_NAME=N'鄂尔多斯';
update dbo.City set C_WS_CODE='ERL' where C_NAME=N'二连浩特';
update dbo.City set C_WS_CODE='ENH' where C_NAME=N'恩施';
update dbo.City set C_WS_CODE='FUO' where C_NAME=N'佛山';
update dbo.City set C_WS_CODE='FUG' where C_NAME=N'阜阳';
update dbo.City set C_WS_CODE='FYN' where C_NAME=N'富蕴';
update dbo.City set C_WS_CODE='FOC' where C_NAME=N'福州';
update dbo.City set C_WS_CODE='KOW' where C_NAME=N'赣州';
update dbo.City set C_WS_CODE='GOQ' where C_NAME=N'格尔木';
update dbo.City set C_WS_CODE='GHN' where C_NAME=N'广汉';
update dbo.City set C_WS_CODE='LHK' where C_NAME=N'光化';
update dbo.City set C_WS_CODE='GYS' where C_NAME=N'广元';
update dbo.City set C_WS_CODE='CAN' where C_NAME=N'广州';
update dbo.City set C_WS_CODE='KWL' where C_NAME=N'桂林';
update dbo.City set C_WS_CODE='KWE' where C_NAME=N'贵阳';
update dbo.City set C_WS_CODE='GYU' where C_NAME=N'固原';
update dbo.City set C_WS_CODE='HRB' where C_NAME=N'哈尔滨';
update dbo.City set C_WS_CODE='HAK' where C_NAME=N'海口';
update dbo.City set C_WS_CODE='HLD' where C_NAME=N'海拉尔';
update dbo.City set C_WS_CODE='HMI' where C_NAME=N'哈密市';
update dbo.City set C_WS_CODE='HMI' where C_NAME=N'哈密';
update dbo.City set C_WS_CODE='HDG' where C_NAME=N'邯郸';
update dbo.City set C_WS_CODE='HGH' where C_NAME=N'杭州';
update dbo.City set C_WS_CODE='HZG' where C_NAME=N'汉中';
update dbo.City set C_WS_CODE='HFE' where C_NAME=N'合肥';
update dbo.City set C_WS_CODE='HEK' where C_NAME=N'黑河';
update dbo.City set C_WS_CODE='HNY' where C_NAME=N'衡阳';
update dbo.City set C_WS_CODE='HTN' where C_NAME=N'和田';
update dbo.City set C_WS_CODE='HTN' where C_NAME=N'和田市';
update dbo.City set C_WS_CODE='HKG' where C_NAME=N'香港';
update dbo.City set C_WS_CODE='HIA' where C_NAME=N'淮安';
update dbo.City set C_WS_CODE='HJJ' where C_NAME=N'怀化';
update dbo.City set C_WS_CODE='HUN' where C_NAME=N'花莲';
update dbo.City set C_WS_CODE='TXN' where C_NAME=N'黄山';
update dbo.City set C_WS_CODE='XAA' where C_NAME=N'黄石';
update dbo.City set C_WS_CODE='HYN' where C_NAME=N'黄岩';
update dbo.City set C_WS_CODE='HET' where C_NAME=N'呼和浩特';
update dbo.City set C_WS_CODE='HUZ' where C_NAME=N'惠州';
update dbo.City set C_WS_CODE='JGD' where C_NAME=N'加格达奇';
update dbo.City set C_WS_CODE='JMU' where C_NAME=N'佳木斯';
update dbo.City set C_WS_CODE='KNC' where C_NAME=N'吉安';
update dbo.City set C_WS_CODE='JGN' where C_NAME=N'嘉峪关';
update dbo.City set C_WS_CODE='SWA' where C_NAME=N'揭阳';
update dbo.City set C_WS_CODE='JIL' where C_NAME=N'吉林';
update dbo.City set C_WS_CODE='TNA' where C_NAME=N'济南';
update dbo.City set C_WS_CODE='JIC' where C_NAME=N'金昌';
update dbo.City set C_WS_CODE='JDZ' where C_NAME=N'景德镇';
update dbo.City set C_WS_CODE='JGS' where C_NAME=N'井冈山';
update dbo.City set C_WS_CODE='JHG' where C_NAME=N'景洪';
update dbo.City set C_WS_CODE='JNG' where C_NAME=N'济宁';
update dbo.City set C_WS_CODE='JJN' where C_NAME=N'晋江';
update dbo.City set C_WS_CODE='JNZ' where C_NAME=N'锦州';
update dbo.City set C_WS_CODE='JUH' where C_NAME=N'九华山';
update dbo.City set C_WS_CODE='JIU' where C_NAME=N'九江';
update dbo.City set C_WS_CODE='JZH' where C_NAME=N'九寨沟';
update dbo.City set C_WS_CODE='JXA' where C_NAME=N'鸡西';
update dbo.City set C_WS_CODE='KJI' where C_NAME=N'喀纳斯';
update dbo.City set C_WS_CODE='KGT' where C_NAME=N'康定';
update dbo.City set C_WS_CODE='KHH' where C_NAME=N'高雄';
update dbo.City set C_WS_CODE='KHG' where C_NAME=N'喀什市';
update dbo.City set C_WS_CODE='KHG' where C_NAME=N'喀什';
update dbo.City set C_WS_CODE='KRY' where C_NAME=N'克拉玛依';
update dbo.City set C_WS_CODE='KNH' where C_NAME=N'金门';
update dbo.City set C_WS_CODE='KCA' where C_NAME=N'库车';
update dbo.City set C_WS_CODE='KRL' where C_NAME=N'库尔勒';
update dbo.City set C_WS_CODE='KMG' where C_NAME=N'昆明';
update dbo.City set C_WS_CODE='LHW' where C_NAME=N'兰州';
update dbo.City set C_WS_CODE='LXA' where C_NAME=N'拉萨';
update dbo.City set C_WS_CODE='LCX' where C_NAME=N'连城';
update dbo.City set C_WS_CODE='LIA' where C_NAME=N'梁平';
update dbo.City set C_WS_CODE='LYG' where C_NAME=N'连云港';
update dbo.City set C_WS_CODE='LLB' where C_NAME=N'荔波';
update dbo.City set C_WS_CODE='LJG' where C_NAME=N'丽江';
update dbo.City set C_WS_CODE='LNJ' where C_NAME=N'临沧';
update dbo.City set C_WS_CODE='LXI' where C_NAME=N'林西';
update dbo.City set C_WS_CODE='LYI' where C_NAME=N'临沂';
update dbo.City set C_WS_CODE='LZY' where C_NAME=N'林芝';
update dbo.City set C_WS_CODE='HZH' where C_NAME=N'黎平';
update dbo.City set C_WS_CODE='LZH' where C_NAME=N'柳州';
update dbo.City set C_WS_CODE='LCX' where C_NAME=N'龙岩';
update dbo.City set C_WS_CODE='LYA' where C_NAME=N'洛阳';
update dbo.City set C_WS_CODE='LZO' where C_NAME=N'泸州';
update dbo.City set C_WS_CODE='MFM' where C_NAME=N'澳门';
update dbo.City set C_WS_CODE='MZG' where C_NAME=N'马公';
update dbo.City set C_WS_CODE='LUM' where C_NAME=N'芒市';
update dbo.City set C_WS_CODE='NZH' where C_NAME=N'满洲里';
update dbo.City set C_WS_CODE='MXZ' where C_NAME=N'梅县';
update dbo.City set C_WS_CODE='MXZ' where C_NAME=N'梅州';
update dbo.City set C_WS_CODE='MIG' where C_NAME=N'绵阳';
update dbo.City set C_WS_CODE='OHE' where C_NAME=N'漠河';
update dbo.City set C_WS_CODE='MDG' where C_NAME=N'牡丹江';
update dbo.City set C_WS_CODE='NLT' where C_NAME=N'那拉提';
update dbo.City set C_WS_CODE='KHN' where C_NAME=N'南昌';
update dbo.City set C_WS_CODE='NAO' where C_NAME=N'南充';
update dbo.City set C_WS_CODE='NKG' where C_NAME=N'南京';
update dbo.City set C_WS_CODE='NNG' where C_NAME=N'南宁';
update dbo.City set C_WS_CODE='NTG' where C_NAME=N'南通';
update dbo.City set C_WS_CODE='NNY' where C_NAME=N'南阳';
update dbo.City set C_WS_CODE='NGB' where C_NAME=N'宁波';
update dbo.City set C_WS_CODE='PZI' where C_NAME=N'攀枝花';
update dbo.City set C_WS_CODE='SYM' where C_NAME=N'普洱';
update dbo.City set C_WS_CODE='HSN' where C_NAME=N'普陀山';
update dbo.City set C_WS_CODE='JIQ' where C_NAME=N'黔江';
update dbo.City set C_WS_CODE='IQM' where C_NAME=N'且末';
update dbo.City set C_WS_CODE='TAO' where C_NAME=N'青岛';
update dbo.City set C_WS_CODE='IQN' where C_NAME=N'庆阳';
update dbo.City set C_WS_CODE='SHP' where C_NAME=N'秦皇岛';
update dbo.City set C_WS_CODE='NDG' where C_NAME=N'齐齐哈尔';
update dbo.City set C_WS_CODE='JJN' where C_NAME=N'泉州';
update dbo.City set C_WS_CODE='JUZ' where C_NAME=N'衢州';
update dbo.City set C_WS_CODE='RKZ' where C_NAME=N'日喀则';
update dbo.City set C_WS_CODE='SYX' where C_NAME=N'三亚';
update dbo.City set C_WS_CODE='SHA' where C_NAME=N'上海';
update dbo.City set C_WS_CODE='SHP' where C_NAME=N'山海关';
update dbo.City set C_WS_CODE='SWA' where C_NAME=N'汕头';
update dbo.City set C_WS_CODE='SHS' where C_NAME=N'沙市';
update dbo.City set C_WS_CODE='SHE' where C_NAME=N'沈阳';
update dbo.City set C_WS_CODE='SZX' where C_NAME=N'深圳';
update dbo.City set C_WS_CODE='SJW' where C_NAME=N'石家庄';
update dbo.City set C_WS_CODE='JJN' where C_NAME=N'石狮';
update dbo.City set C_WS_CODE='SYM' where C_NAME=N'思茅';
update dbo.City set C_WS_CODE='SZV' where C_NAME=N'苏州';
update dbo.City set C_WS_CODE='TCG' where C_NAME=N'塔城';
update dbo.City set C_WS_CODE='TXG' where C_NAME=N'台中';
update dbo.City set C_WS_CODE='TPE' where C_NAME=N'台北';
update dbo.City set C_WS_CODE='TTT' where C_NAME=N'台东';
update dbo.City set C_WS_CODE='TYN' where C_NAME=N'太原';
update dbo.City set C_WS_CODE='HYN' where C_NAME=N'台州';
update dbo.City set C_WS_CODE='TVS' where C_NAME=N'唐山';
update dbo.City set C_WS_CODE='TCZ' where C_NAME=N'腾冲';
update dbo.City set C_WS_CODE='TSN' where C_NAME=N'天津';
update dbo.City set C_WS_CODE='THQ' where C_NAME=N'天水';
update dbo.City set C_WS_CODE='TNH' where C_NAME=N'通化';
update dbo.City set C_WS_CODE='TGO' where C_NAME=N'通辽';
update dbo.City set C_WS_CODE='TEN' where C_NAME=N'铜仁市';
update dbo.City set C_WS_CODE='TEN' where C_NAME=N'铜仁';
update dbo.City set C_WS_CODE='TLQ' where C_NAME=N'吐鲁番';
update dbo.City set C_WS_CODE='WXN' where C_NAME=N'万州';
update dbo.City set C_WS_CODE='WEF' where C_NAME=N'潍坊';
update dbo.City set C_WS_CODE='WEH' where C_NAME=N'威海';
update dbo.City set C_WS_CODE='WNH' where C_NAME=N'文山县';
update dbo.City set C_WS_CODE='WNH' where C_NAME=N'文山';
update dbo.City set C_WS_CODE='WNZ' where C_NAME=N'温州';
update dbo.City set C_WS_CODE='WUA' where C_NAME=N'乌海';
update dbo.City set C_WS_CODE='WUH' where C_NAME=N'武汉';
update dbo.City set C_WS_CODE='WHU' where C_NAME=N'芜湖';
update dbo.City set C_WS_CODE='HLH' where C_NAME=N'乌兰浩特';
update dbo.City set C_WS_CODE='URC' where C_NAME=N'乌鲁木齐';
update dbo.City set C_WS_CODE='WUX' where C_NAME=N'无锡';
update dbo.City set C_WS_CODE='WUS' where C_NAME=N'武夷山';
update dbo.City set C_WS_CODE='WUZ' where C_NAME=N'梧州';
update dbo.City set C_WS_CODE='XMN' where C_NAME=N'厦门';
update dbo.City set C_WS_CODE='SIA' where C_NAME=N'西安';
update dbo.City set C_WS_CODE='XFN' where C_NAME=N'襄樊';
update dbo.City set C_WS_CODE='DIG' where C_NAME=N'香格里拉';
update dbo.City set C_WS_CODE='XFN' where C_NAME=N'襄阳';
update dbo.City set C_WS_CODE='XIC' where C_NAME=N'西昌';
update dbo.City set C_WS_CODE='XIL' where C_NAME=N'锡林浩特';
update dbo.City set C_WS_CODE='XNT' where C_NAME=N'邢台';
update dbo.City set C_WS_CODE='ACX' where C_NAME=N'兴义';
update dbo.City set C_WS_CODE='XNN' where C_NAME=N'西宁';
update dbo.City set C_WS_CODE='JHG' where C_NAME=N'西双版纳';
update dbo.City set C_WS_CODE='XUZ' where C_NAME=N'徐州';
update dbo.City set C_WS_CODE='ENY' where C_NAME=N'延安';
update dbo.City set C_WS_CODE='YNZ' where C_NAME=N'盐城';
update dbo.City set C_WS_CODE='YTY' where C_NAME=N'扬州';
update dbo.City set C_WS_CODE='YNJ' where C_NAME=N'延吉';
update dbo.City set C_WS_CODE='YNT' where C_NAME=N'烟台';
update dbo.City set C_WS_CODE='YBP' where C_NAME=N'宜宾';
update dbo.City set C_WS_CODE='YIH' where C_NAME=N'宜昌';
update dbo.City set C_WS_CODE='YIC' where C_NAME=N'宜春';
update dbo.City set C_WS_CODE='LDS' where C_NAME=N'伊春';
update dbo.City set C_WS_CODE='YIN' where C_NAME=N'伊犁';
update dbo.City set C_WS_CODE='INC' where C_NAME=N'银川';
update dbo.City set C_WS_CODE='YIN' where C_NAME=N'伊宁市';
update dbo.City set C_WS_CODE='YIW' where C_NAME=N'义乌';
update dbo.City set C_WS_CODE='LLF' where C_NAME=N'永州';
update dbo.City set C_WS_CODE='UYN' where C_NAME=N'榆林';
update dbo.City set C_WS_CODE='YCU' where C_NAME=N'运城';
update dbo.City set C_WS_CODE='YUS' where C_NAME=N'玉树';
update dbo.City set C_WS_CODE='YUS' where C_NAME=N'玉树县';
update dbo.City set C_WS_CODE='DYG' where C_NAME=N'张家界';
update dbo.City set C_WS_CODE='ZQZ' where C_NAME=N'张家口';
update dbo.City set C_WS_CODE='YZY' where C_NAME=N'张掖';
update dbo.City set C_WS_CODE='ZHA' where C_NAME=N'湛江';
update dbo.City set C_WS_CODE='ZAT' where C_NAME=N'昭通';
update dbo.City set C_WS_CODE='CGO' where C_NAME=N'郑州';
update dbo.City set C_WS_CODE='HJJ' where C_NAME=N'芷江';
update dbo.City set C_WS_CODE='ZGN' where C_NAME=N'中山';
update dbo.City set C_WS_CODE='ZHY' where C_NAME=N'中卫';
update dbo.City set C_WS_CODE='HSN' where C_NAME=N'舟山';
update dbo.City set C_WS_CODE='ZUH' where C_NAME=N'珠海';
update dbo.City set C_WS_CODE='ZUZ' where C_NAME=N'株洲';
update dbo.City set C_WS_CODE='ZYI' where C_NAME=N'遵义';
