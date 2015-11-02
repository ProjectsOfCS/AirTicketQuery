USE [C:\MYGITREPOSITORY\STUDYSAMPLES\AIRTICKETQUERY\AIRTICKETQUERY\DATA\AIRTICKET.MDF]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
if exists(select 1 from sysobjects where name ='FlightInfo')
DROP TABLE [dbo].[FlightInfo];

CREATE TABLE [dbo].[FlightInfo] (    
    C_From NVARCHAR (10) NOT NULL,
	C_To NVARCHAR (10) NOT NULL,
	C_Departure datetime NOT NULL,
	C_DateSource NVARCHAR (20) NOT NULL,
    C_Airline NVARCHAR (200)  NULL, 
    C_FlightNo NVARCHAR(200) NULL,
	C_DEPTIME NVARCHAR(200) NULL,
	C_ARRTIME NVARCHAR(200) NULL,
	C_TotalTime NVARCHAR(200) NULL,
	C_FirstClass float NULL,
	C_Business float NULL,
	C_Economy float NULL,
	C_Price float NULL,
	C_Remark NVARCHAR(2000) NULL,
	C_ID  INT IDENTITY (1, 1) NOT NULL,
	C_ADD_TIME datetime not null default(getdate()), 
    CONSTRAINT [PK_FlightInfo] PRIMARY KEY ([C_From], [C_To], [C_Departure], [C_ID], [C_DateSource])
);

