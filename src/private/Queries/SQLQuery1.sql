select * from guest;
select * from GuestProperty
select * from Host

DECLARE @return_status int; 
EXECUTE @return_status=[dbo].[AuthenticateUser] 'sara','vels@1982'
SELECT @return_status