USE [CoreAirplusDb]
GO
/****** Object:  StoredProcedure [dbo].[GetGuestsList]    Script Date: 8/21/2018 4:12:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetGuestsList]
@Listing INTEGER
AS
BEGIN
select 
  G.GuestId,
  GP.PropertyId,
  P.HostId,
  G.FullName,
  G.FirstName,
  ISNULL(convert(varchar,GP.RCheckIn,101)+' '+convert(varchar,GP.RCheckIn,8),'') AS RequestedCheckIn,
  ISNULL(convert(varchar,GP.RCheckOut,101)+' '+convert(varchar,GP.RCheckOut,8),'') AS RequestedCheckOut,
  convert(varchar, GP.CheckIn, 101) AS CheckIN,
   convert(varchar, GP.CheckOut, 101) AS CheckOut,
   ISNULL(GP.[status],'') AS [Status],
   ISNULL(convert(varchar,GP.CleaningTime,101)+' '+convert(varchar,GP.CleaningTime,8),'') AS CleaningCompanyTiming,
   ISNULL(GP.REMARKS,'') AS Remarks
from guests G
  left join reservations GP
    on G.GuestId=GP.GuestId
  inner join properties P
    on GP.PropertyId=P.PropertyId
Where P.Listingid=@Listing  
order by GP.CheckIn	DESC

END

