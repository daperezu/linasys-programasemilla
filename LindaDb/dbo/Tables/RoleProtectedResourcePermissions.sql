CREATE TABLE dbo.RoleProtectedResourcePermissions (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    RoleId NVARCHAR(450) NOT NULL,  
    ProtectedResourceId BIGINT NOT NULL,  -- Points to `ProtectedResources`
    FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProtectedResourceId) REFERENCES dbo.ProtectedResources(Id) ON DELETE CASCADE
);
