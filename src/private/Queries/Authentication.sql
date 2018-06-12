Use Airplus;

Go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS(SELECT 1 FROM SYS.PROCEDURES WHERE NAME='AuthenticateUser')
BEGIN

  DROP PROCEDURE [dbo].[AuthenticateUser]

END

Go
CREATE PROCEDURE [dbo].[AuthenticateUser]
@username varchar(100),
@pass varchar(100)

AS
BEGIN
DECLARE @COUNT INTEGER
 SELECT @COUNT=COUNT(1) 
   FROM [Airplus].[dbo].[Host]
     WHERE username=@username
	   AND pass=HASHBYTES('SHA1', @pass) 
	   
SELECT @COUNT;
  IF @COUNT>0 
    BEGIN
 SELECT HostId,
		FullName,
		FirstName,
		LastName,
		UserName,
		Email
   FROM [Airplus].[dbo].[Host]
     WHERE username=@username
	   AND pass=HASHBYTES('SHA1', @pass)
  	END	    
	 
END