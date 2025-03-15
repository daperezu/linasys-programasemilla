CREATE TABLE dbo.UserProtectedResourcePermissions (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,  
    ProtectedResourceId BIGINT NOT NULL,  -- Points to `ProtectedResources`
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProtectedResourceId) REFERENCES dbo.ProtectedResources(Id) ON DELETE CASCADE
);
