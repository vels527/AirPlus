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
		    ,HASHBYTES('SHA2_512', 'vels@1982')
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

