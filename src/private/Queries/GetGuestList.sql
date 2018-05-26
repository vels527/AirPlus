Use Airplus;

Go
DROP PROCEDURE [dbo].[GetGuestsList]
GO


CREATE PROCEDURE [dbo].[GetGuestsList]
@user VARCHAR(100)
AS
BEGIN
select G.FullName,
  G.FirstName,
  convert(varchar, GP.CheckIn, 101) AS CheckIN,
   convert(varchar, GP.CheckOut, 101) AS CheckOut
from Guest G
  left join GuestProperty GP
    on G.Guest_Id=GP.Guest_Id
  inner join Property P
    on GP.Property_Id=P.Property_Id
  inner join Host H
    on P.HostId=H.HostId
	  AND H.username=@user
END

--select * from guest
--select * from GuestProperty
--select * from Property;
--select * from host;
