USE [Airplus]
GO

/****** Object:  StoredProcedure [dbo].[GetGuestsListForToday]    Script Date: 05/28/2018 12:59:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(SELECT 1 FROM SYS.PROCEDURES WHERE NAME='GetGuestsListForToday')
BEGIN

  DROP PROCEDURE [dbo].[GetGuestsListForToday]

END

GO
CREATE PROCEDURE [dbo].[GetGuestsListForToday]
@Listing INTEGER
AS
BEGIN
select 
  G.Guest_Id,
  GP.Property_Id,
  P.HostId,
  G.FullName,
  G.FirstName,
  ISNULL(convert(varchar,GP.RequestedCheckIn,101)+' '+convert(varchar,GP.RequestedCheckIn,8),'') AS RequestedCheckIn,
  ISNULL(convert(varchar,GP.RequestedCheckOut,101)+' '+convert(varchar,GP.RequestedCheckOut,8),'') AS RequestedCheckOut,
  convert(varchar, GP.CheckIn, 101) AS CheckIN,
   convert(varchar, GP.CheckOut, 101) AS CheckOut,
   ISNULL(SC.StatusValue,'Empty') AS StatusCode,
   ISNULL(convert(varchar,GP.CCompanyTiming,101)+' '+convert(varchar,GP.CCompanyTiming,8),'') AS CleaningCompanyTiming,
   ISNULL(GP.REMARKS,'') AS Remarks
from Guest G
  left join GuestProperty GP
    on G.Guest_Id=GP.Guest_Id
  left join StatusCode SC
    on GP.CStatus=SC.StatusCode_Id
  inner join Property P
    on GP.Property_Id=P.Property_Id
Where DATEDIFF(day,GETDATE(),GP.CheckIn)<2
  AND DATEDIFF(day,GETDATE(),GP.CheckIn)>=0 
  AND P.Listingid=@Listing  
order by GP.CheckIn	DESC
	


select 
  G.Guest_Id,
  GP.Property_Id,
  P.HostId,
  G.FullName,
  G.FirstName,
  ISNULL(convert(varchar,GP.RequestedCheckIn,101)+' '+convert(varchar,GP.RequestedCheckIn,8),'') AS RequestedCheckIn,
  ISNULL(convert(varchar,GP.RequestedCheckOut,101)+' '+convert(varchar,GP.RequestedCheckOut,8),'') AS RequestedCheckOut,
  convert(varchar, GP.CheckIn, 101) AS CheckIN,
   convert(varchar, GP.CheckOut, 101) AS CheckOut,
   ISNULL(SC.StatusValue,'Empty') AS StatusCode,
   ISNULL(convert(varchar,GP.CCompanyTiming,101)+' '+convert(varchar,GP.CCompanyTiming,8),'') AS CleaningCompanyTiming,
   ISNULL(GP.REMARKS,'') AS Remarks
from Guest G
  left join GuestProperty GP
    on G.Guest_Id=GP.Guest_Id
  left join StatusCode SC
    on GP.CStatus=SC.StatusCode_Id
  inner join Property P
    on GP.Property_Id=P.Property_Id
Where DATEDIFF(day,GETDATE(),GP.CheckOut)<2 	  
  AND DATEDIFF(day,GETDATE(),GP.CheckOut)>=0	
  AND P.Listingid=@Listing  
order by GP.CheckOut DESC
	

select StatusCode_id,
  StatusValue
from StatusCode

END

GO


