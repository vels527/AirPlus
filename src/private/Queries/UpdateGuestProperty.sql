USE CoreAirplusDb
GO

/****** Object:  StoredProcedure [dbo].[UpdateGuestProperty]    Script Date: 05/28/2018 12:59:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TYPE [dbo].[GUESTTYPETABLE] AS TABLE(
	[GuestId] [int] NULL,
	[PropertyId] [int] NULL,
	[HostId] [int] NULL,
	[RequestedCheckIn] [datetime] NULL,
	[RequestedCheckOut] [datetime] NULL,
	[CheckOutCleaning] [datetime] NULL,
	[StatusCode] [int] NULL,
	[Remarks] [varchar](350) NULL
)
GO
IF EXISTS(SELECT 1 FROM SYS.PROCEDURES WHERE NAME='UpdateGuestProperty')
BEGIN

  DROP PROCEDURE [dbo].[UpdateGuestProperty]

END

GO
CREATE PROCEDURE UpdateGuestProperty
@Guest dbo.GUESTTYPETABLE READONLY 
AS
BEGIN

Update GP
SET GP.RCheckIN=GT.REQUESTEDCHECKIN,
GP.RCheckOut=GT.REQUESTEDCHECKOUT,
GP.[status]=GT.STATUSCODE,
GP.REMARKS=GT.REMARKS,
GP.CleaningTime=GT.CHECKOUTCLEANING
FROM reservations GP
JOIN @Guest GT ON
GP.GuestID=GT.GUESTID AND
GP.PropertyID=GT.PROPERTYID 
JOIN properties P ON
P.PropertyId=GP.PropertyId
AND P.HostId=GT.HOSTID 

 
END


GO
