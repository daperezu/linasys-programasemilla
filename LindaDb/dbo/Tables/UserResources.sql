﻿CREATE TABLE [dbo].[UserResources] (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,  
    ResourceId BIGINT NOT NULL,  
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    FOREIGN KEY (ResourceId) REFERENCES Resources(Id) ON DELETE CASCADE
);