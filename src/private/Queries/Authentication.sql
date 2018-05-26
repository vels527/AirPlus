Use Airplus;

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
	   AND pass=HASHBYTES('SHA2_512', @pass) 
  IF @COUNT>0 
    BEGIN
	  RETURN 1
  	END	    
	RETURN 0  
END