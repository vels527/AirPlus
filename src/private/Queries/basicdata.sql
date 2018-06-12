USE [Airplus]
GO

INSERT INTO [dbo].[Host]
           ([FullName]
           ,[FirstName]
           ,[LastName]
		   ,[UserName]
		   ,[pass]
           ,[Age]
           ,[DOB]
           ,[Email]
           ,[Phone])
     VALUES
           ('Saravanavel Balasubramanian'
           ,'Saravanavel'
           ,'Balasubramanian'
		   ,'saran'
		    ,HASHBYTES('SHA1', 'vels@1982')
           ,41
           ,'1978-07-22 13:40:28.997'
           ,'saran@kustotech.in'
           ,NULL)
GO

USE [Airplus]
GO

INSERT INTO [dbo].[Property]
           ([Listingid]
           ,[PropertyAddress]
           ,[HostId])
     VALUES
           (16676839
           ,'3437 Cooper Drive, Santa Clara, CA 95051, United States'
           ,1)
GO


USE [AIRPLUS]
INSERT INTO [dbo].[StatusCode](StatusValue) VALUES ('Not Specified');
INSERT INTO [dbo].[StatusCode](StatusValue) VALUES ('Welcome Message');
INSERT INTO [dbo].[StatusCode](StatusValue) VALUES ('Reminder to Confirm Message');
INSERT INTO [dbo].[StatusCode](StatusValue) VALUES ('CheckIn Welcome Message');
INSERT INTO [dbo].[StatusCode](StatusValue) VALUES ('Checkout Notification Message');
INSERT INTO [dbo].[StatusCode](StatusValue) VALUES ('Thank You Message');
INSERT INTO [dbo].[StatusCode](StatusValue) VALUES ('Write Review');

--"Not Specified","Welcome Message","Check In Instructions","Checked In", "Check Out Instructions", "Checked Out"
select GETDATE();
select * from Host;
select * from Property
sp_help guest;

USE [Airplus]
GO

INSERT INTO [dbo].[Guest]
           ([FullName]
           ,[FirstName]
           ,[LastName]
           ,[Age]
           ,[DOB]
           ,[Email]
           ,[Phone])
     VALUES
           (<FullName, varchar(250),>
           ,<FirstName, varchar(150),>
           ,<LastName, varchar(150),>
           ,<Age, int,>
           ,<DOB, datetime,>
           ,<Email, varchar(250),>
           ,<Phone, varchar(250),>)
GO


GO

