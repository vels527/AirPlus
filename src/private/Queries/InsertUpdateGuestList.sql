USE [AirplusDEV]
GO

/****** Object:  StoredProcedure [dbo].[InsertUpdateGuestList]    Script Date: 05/28/2018 12:59:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(SELECT 1 FROM SYS.PROCEDURES WHERE NAME='InsertUpdateGuestList')
BEGIN

  DROP PROCEDURE [dbo].[InsertUpdateGuestList]

END

IF EXISTS(SELECT 1 FROM SYS.TABLE_TYPES WHERE [NAME]='GUESTPROPERTYTYPETABLE')
BEGIN
	DROP TYPE [DBO].[GUESTPROPERTYTYPETABLE]
END

CREATE TYPE [dbo].[GUESTPROPERTYTYPETABLE] AS TABLE(
	[FullName] [varchar](250) NULL,
	[FirstName] [varchar](150) NULL,
	[Email] [varchar](250) NULL,
	[Phone] [varchar](250) NULL,
	[ListingID] [int] NULL,
	[CHECKIN] [datetime] NULL,
	[CHECKOUT] [datetime] NULL	
)
GO

GO
CREATE PROCEDURE InsertUpdateGuestList
 @Guest dbo.GUESTPROPERTYTYPETABLE READONLY 
AS
BEGIN
  INSERT INTO Guest(AirplusId,FullName,FirstName,LastName,Age,DOB,Email,Phone,ListingID,CheckIN,Tag)
  SELECT 
	null,
    G.FullName,
	G.FirstName,
	null,
	null,
	null,
	G.Email,
	G.Phone,
	G.ListingID,
	G.CHECKIN,
	null
  FROM
    @Guest G
      LEFT JOIN Guest GT
	    ON G.FullName=GT.FullName 
		  AND G.FirstName=GT.FirstName
		  AND G.ListingID=GT.ListingID
		  AND G.CHECKIN=GT.CHECKIN
  WHERE
    GT.FullName is null 

  INSERT INTO GuestProperty(Guest_Id,Property_Id,CCompanyId,CheckIn,CheckOut,RequestedCheckIn,RequestedCheckOut,CCompanyTiming,CStatus,RecordTIme,REMARKS)
  SELECT 
    GT.Guest_Id,
	P.Property_Id,
	null,
	G.CHECKIN,
	G.CHECKOUT,
	null,
	null,
	null,
	null,
	GETDATE(),
	null
  FROM
    @Guest G
	  JOIN Guest GT
	    ON G.FirstName=GT.FirstName
		  AND G.FullName=GT.FullName
		  AND G.ListingID=GT.ListingID
		  AND G.CHECKIN=GT.CHECKIN
	  LEFT JOIN GuestProperty GP
	    ON GT.Guest_Id=GP.Guest_Id
	  JOIN Property P
	    ON G.ListingID=P.Listingid
  WHERE
    GP.Guest_Id is null

END


GO


