USE [Airplus]
GO

/****** Object:  StoredProcedure [dbo].[GetGuestsList]    Script Date: 05/28/2018 12:59:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[GetGuestsList]
@user VARCHAR(100)
AS
BEGIN
select 
  G.Guest_Id,
  GP.Property_Id,
  P.HostId,
  G.FullName,
  G.FirstName,
  convert(varchar, GP.CheckIn, 101) AS CheckIN,
   convert(varchar, GP.CheckOut, 101) AS CheckOut,
   ISNULL(SC.StatusValue,'Empty') AS StatusCode
from Guest G
  left join GuestProperty GP
    on G.Guest_Id=GP.Guest_Id
  left join StatusCode SC
    on GP.CStatus=SC.StatusCode_Id
  inner join Property P
    on GP.Property_Id=P.Property_Id
  inner join Host H
    on P.HostId=H.HostId
	  AND H.username=@user
order by GP.CheckIn	  
	  

select StatusCode_id,
  StatusValue
from StatusCode

END

GO


