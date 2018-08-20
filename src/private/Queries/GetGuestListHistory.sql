USE [CoreAirplusDb]
GO

/****** Object:  StoredProcedure [dbo].[GetGuestsListHistory]    Script Date: 05/28/2018 12:59:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(SELECT 1 FROM SYS.PROCEDURES WHERE NAME='GetGuestsListHistory')
BEGIN

  DROP PROCEDURE [dbo].[GetGuestsListHistory]

END

GO
CREATE PROCEDURE [dbo].[GetGuestsListHistory]
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

GO