USE [CoreAirplusDb]
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
    P.IcalUrl,
	H.Email,
	H.FirstName
  FROM properties P
    JOIN [hosts] H
      ON P.HostId=H.HostId	
  WHERE LISTINGID IS NOT NULL 
    AND IcalUrl IS NOT NULL

END

GO
