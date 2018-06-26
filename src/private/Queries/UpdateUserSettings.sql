USE [AirplusDEV]
GO

/****** Object:  StoredProcedure [dbo].[UpdateUserSettings]    Script Date: 05/28/2018 12:59:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(SELECT 1 FROM SYS.PROCEDURES WHERE NAME='UpdateUserSettings')
BEGIN

  DROP PROCEDURE [dbo].[UpdateUserSettings]

END

IF EXISTS(SELECT 1 FROM SYS.TABLE_TYPES WHERE [NAME]='LISTTYPETABLE')
BEGIN
	DROP TYPE [DBO].[LISTTYPETABLE]
END

CREATE TYPE [dbo].[LISTTYPETABLE] AS TABLE(
	[Property_Id] [int] NULL,
	[ListingId] [int] NULL,
	[PropertyAddress] [varchar](500) NULL,
	[ICSURL] [varchar](MAX) NULL
)
GO

GO
CREATE PROCEDURE UpdateUserSettings
 @FullName VARCHAR(250),
 @FirstName VARCHAR(150),
 @LastName VARCHAR(150),
 @username VARCHAR(100),
 @Age INT,
 @Email VARCHAR(250),
 @Phone VARCHAR(250),
 @List dbo.LISTTYPETABLE READONLY 
AS
BEGIN

  DECLARE @HostId INT

  UPDATE [dbo].[Host]
    SET  [FullName] = @FullName
        ,[FirstName] = @FirstName
        ,[LastName] = @LastName
        ,[Age] = @Age
        ,[Email] = @Email
        ,[Phone] = @Phone
    WHERE [username] = @username
 
  SELECT 
    @HostId=MAX(HostId) 
  FROM [dbo].[Host]
  WHERE [username] = @username
  
  UPDATE P
    SET P.ICSURL=L.ICSURL,
	    P.PropertyAddress=L.PropertyAddress,
		P.Listingid=L.ListingId
  FROM PROPERTY P
    LEFT JOIN @List L
	  ON P.Property_Id=L.Property_Id
  WHERE P.HostId=@HostId
END


GO
