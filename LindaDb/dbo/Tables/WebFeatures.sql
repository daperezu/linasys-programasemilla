CREATE TABLE dbo.WebFeatures (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    ExternalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),  -- Matches `ProtectedResources.ExternalId`
    Name NVARCHAR(100) NOT NULL,
    Area NVARCHAR(100) NULL,  -- NULL if it's just a menu category
    Controller NVARCHAR(100) NULL,  -- NULL if it's just a menu category
    Action NVARCHAR(100) NULL,  -- NULL if it's just a menu category
    ParentId BIGINT NULL,  -- Supports menu hierarchy
    IsMenu BIT NOT NULL DEFAULT 0,  -- 1 = Menu, 0 = System Feature
    MenuOrder INT NOT NULL DEFAULT 0,  -- Controls ordering of menu items
    IsPublic BIT NOT NULL DEFAULT 0, -- 1 = Publicly accessible, 0 = Requires authentication
    FOREIGN KEY (ParentId) REFERENCES dbo.WebFeatures(Id) ON DELETE NO ACTION
);
