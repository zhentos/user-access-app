CREATE TABLE [dbo].[Users]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,       
    FirstName NVARCHAR(50) NOT NULL,         
    LastName NVARCHAR(50) NOT NULL,         
    Email NVARCHAR(100) NOT NULL UNIQUE,     
    [Password] NVARCHAR(255) NOT NULL,       
    PasswordSalt NVARCHAR(255) NOT NULL,    
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    LastModifiedDate DATETIME2 DEFAULT GETDATE(),
    IsActive BIT NOT NULL DEFAULT 1
)
