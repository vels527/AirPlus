USE [CoreAirplusDb]
GO

/****** Object:  StoredProcedure [dbo].[GetGuestsList]    Script Date: 05/28/2018 12:59:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(SELECT 1 FROM SYS.PROCEDURES WHERE NAME='GetGuestsList')
BEGIN

  DROP PROCEDURE [dbo].[GetGuestsList]

END

GO
CREATE PROCEDURE [dbo].[GetGuestsList]
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
	  AND P.ListingId=@Listing
Where (GP.CStatus	is null 
        or GP.CStatus=1)
  OR ((GP.CStatus	is not null 
        or GP.CStatus<>1)
          AND (DATEDIFF(day,GP.CheckOut,GETDATE())<14))    	  
order by GP.CheckIn	DESC
	  

select StatusCode_id,
  StatusValue
from StatusCode

END

GO

