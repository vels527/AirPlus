Go
CREATE OR ALTER PROCEDURE SAVEAVAILABILITY
@LISTINGID BIGINT,
@CALENDARDATE DATETIME,
@ISAVAILABLE BIT,
@PRICE MONEY
AS
BEGIN
  DECLARE @PROPERTYID INTEGER

  select @PROPERTYID=PropertyId 
  from properties
  where ListingId=@LISTINGID
  
  INSERT INTO CalendarPrices VALUES(@LISTINGID,@CALENDARDATE,@ISAVAILABLE,@PRICE,@PROPERTYID);
END



