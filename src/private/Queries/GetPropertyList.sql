USE [Airplus]
GO

/****** Object:  StoredProcedure [dbo].[GetPropertyList]    Script Date: 05/28/2018 12:59:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(SELECT 1 FROM SYS.PROCEDURES WHERE NAME='GetPropertyList')
BEGIN

  DROP PROCEDURE [dbo].[GetPropertyList]

END

GO
CREATE PROCEDURE [dbo].[GetPropertyList]
AS
BEGIN

  SELECT 
    P.LISTINGID,
    P.ICSURL,
	H.Email,
	H.FirstName
  FROM PROPERTY P
    JOIN [Host] H
      ON P.HostId=H.HostId	
  WHERE LISTINGID IS NOT NULL 
    AND ICSURL IS NOT NULL

END

GO
