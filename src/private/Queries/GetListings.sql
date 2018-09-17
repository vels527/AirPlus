CREATE OR ALTER PROCEDURE GetListings
AS
BEGIN
SELECT PropertyId,ListingId FROM Listings
ORDER BY 
PropertyId,
ListingId
END
