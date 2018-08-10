USE [Airplus]
GO

/****** Object:  StoredProcedure [dbo].[GetSettings]    Script Date: 05/28/2018 12:59:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(SELECT 1 FROM SYS.PROCEDURES WHERE NAME='GetSettings')
BEGIN

  DROP PROCEDURE [dbo].[GetSettings]

END

GO
CREATE PROCEDURE [dbo].[GetSettings]
@user VARCHAR(100)
AS
BEGIN
Select 
  H.FullName,
  H.FirstName,
  H.LastName,
  H.Age,
  H.Email,
  H.Phone,
  P.Property_Id,
  P.ListingId,
  P.PropertyAddress,
  P.ICSURL
from Host H
  Left Join Property P
    ON H.HostId = P.HostId
Where H.username=@user 
END

GO

