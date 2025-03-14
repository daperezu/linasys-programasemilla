CREATE TABLE dbo.ProtectedResources (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
    ExternalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),  -- Used for linking different entity types
    ResourceType INT NOT NULL,  -- '1:Project', '2:WebFeature'
    Name NVARCHAR(255) NOT NULL
    UNIQUE (ExternalId)  -- Ensures each ExternalId is unique across entities likes projects, webfeatures, and so on.
);
