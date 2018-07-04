create database Airplus;
GO

use Airplus;
Create Table Guest(
Guest_Id INT IDENTITY(1,1) PRIMARY KEY,
AirplusId BIGINT UNIQUE,
FullName VARCHAR(250),
FirstName VARCHAR(150),
LastName VARCHAR(150),
Age INT,
DOB DateTime,
Email VARCHAR(250),
Phone VARCHAR(250),
REMARKS VARCHAR(350),
ListingId INT NOT NULL,
CheckIn DATETIME NOT NULL,
Tag INT
);
Create Table Host(
HostId INT IDENTITY(1,1) PRIMARY KEY,
FullName VARCHAR(250),
FirstName VARCHAR(150),
LastName VARCHAR(150),
username VARCHAR(100),
pass BINARY(64),
Age INT,
DOB DateTime,
Email VARCHAR(250),
Phone VARCHAR(250)
);
Create Table Property(
Property_Id INT IDENTITY(1,1) PRIMARY KEY,
Listingid INT,
PropertyAddress VARCHAR(500),
ICSURL VARCHAR(MAX),
HostId INT,
CONSTRAINT FK_Property_Host FOREIGN KEY (HostId) REFERENCES Host (HostId) 
);
Create Table CleaningCompany(
CompanyId INT IDENTITY(1,1) PRIMARY KEY,
CompanyName VARCHAR(200),
CompanyAddress VARCHAR(500)
);
Create Table GuestProperty(
Guest_Id INT,
Property_Id INT,
CCompanyId INT,
CheckIn DATETIME,
CheckOut DATETIME,
RequestedCheckIn DATETIME,
RequestedCheckOut DATETIME,
CCompanyTiming DATETIME,
CStatus VARCHAR(100),
RecordTIme DATETIME,
CONSTRAINT FK_Guest_Host FOREIGN KEY (Guest_Id) REFERENCES Guest (Guest_Id),
CONSTRAINT FK_Guest_Property FOREIGN KEY (Property_Id) REFERENCES Property (Property_Id),
CONSTRAINT FK_Guest_Company FOREIGN KEY (CCompanyId) REFERENCES CleaningCompany (CompanyId),
CONSTRAINT PK_Guest_Property PRIMARY KEY (Guest_Id,Property_Id,CheckIn,CheckOut)
);

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