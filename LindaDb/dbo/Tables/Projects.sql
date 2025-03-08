CREATE TABLE [dbo].[Projects] (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description TEXT NULL,
    CreatedBy NVARCHAR(450) NOT NULL,
    FOREIGN KEY (CreatedBy) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);