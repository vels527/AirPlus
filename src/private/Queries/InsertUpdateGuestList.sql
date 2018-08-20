USE [CoreAirplusDb]
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
  INSERT INTO Guests(FullName,FirstName,LastName,Age,DOB,Email,Phone,Tag)
  SELECT 
    G.FullName,
	G.FirstName,
	null,
	null,
	null,
	G.Email,
	G.Phone,
	null
  FROM
    @Guest G
      LEFT JOIN guests GT
	    ON G.FullName=GT.FullName 
		  AND G.FirstName=GT.FirstName
		  AND ISNULL(G.Email,'')=ISNULL(GT.Email,'')
		  AND ISNULL(G.phone,'')=ISNULL(GT.phone,'')
  WHERE
    GT.FullName is null 

  INSERT INTO reservations(GuestId,PropertyId,CleaningCompanyId,CheckIn,CheckOut,RCheckIn,RCheckOut,CleaningTime,status,CreatedTime,REMARKS)
  SELECT 
    GT.GuestId,
	P.PropertyId,
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
	  JOIN guests GT
	    ON G.FirstName=GT.FirstName
		  AND G.FullName=GT.FullName
		  AND G.Email=GT.Email
		  AND G.Phone=GT.Phone
	  LEFT JOIN reservations GP
	    ON GT.GuestId=GP.GuestId
	  JOIN properties P
	    ON G.ListingID=P.Listingid
  WHERE
    GP.GuestId is null

END


GO


