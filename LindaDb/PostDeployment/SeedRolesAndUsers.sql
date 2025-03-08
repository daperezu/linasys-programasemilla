-- =============================================
-- Post-Deployment Script for Seeding LinaSys Roles and Admin User
-- =============================================

-- Insert, Update, or Delete Roles Using MERGE
MERGE INTO AspNetRoles AS target
USING (VALUES 
    ('Administrator', 'ADMINISTRATOR'),
    ('Coordinator', 'COORDINATOR'),
    ('Guide', 'GUIDE'),
    ('Mentor', 'MENTOR'),
    ('Facilitator', 'FACILITATOR'),
    ('Starter', 'STARTER'),
    ('Liaison', 'LIAISON')
) AS source (RoleName, NormalizedRoleName)
ON target.Name = source.RoleName
WHEN NOT MATCHED THEN
    INSERT (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), source.RoleName, source.NormalizedRoleName, NEWID())
WHEN NOT MATCHED BY SOURCE THEN
    DELETE; -- Remove roles that are no longer in the predefined list

-- Ensure Default Admin User Exists
DECLARE @AdminUserId NVARCHAR(450) = (SELECT TOP 1 Id FROM AspNetUsers WHERE UserName = '123456789');

IF @AdminUserId IS NULL
BEGIN
    SET @AdminUserId = NEWID(); -- Generate a unique ID
    INSERT INTO AspNetUsers (
        Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, 
        PasswordHash, 
        SecurityStamp, ConcurrencyStamp, AccessFailedCount, LockoutEnabled, 
        PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled
    )
    VALUES (
        @AdminUserId, '123456789', '123456789', 'admin@linasys.com', 'ADMIN@LINASYS.COM', 1, 
        'AQAAAAIAAYagAAAAELtrv8uTJ3doJ0w6TF1NjAb/1opJilLBd3Hk1FpVpgmSpkg4lDkoBQ6SLww8EOfMag==', -- adminlinasys!0
        'XDXFG5AKFNQM3QRBXIIZKM3AWS2SWFS2', '0eede1d6-fb77-47a6-8987-0cd77abbacc2', 0, 1,
        NULL, 0, 0 -- Ensure required columns are set
    );
END
ELSE
BEGIN
    SET @AdminUserId = (SELECT TOP 1 Id FROM AspNetUsers WHERE UserName = '123456789');
END

-- Assign Administrator Role to Default Admin User using MERGE
DECLARE @AdminRoleId NVARCHAR(450) = (SELECT TOP 1 Id FROM AspNetRoles WHERE Name = 'Administrator');

MERGE INTO AspNetUserRoles AS target
USING (SELECT @AdminUserId AS UserId, @AdminRoleId AS RoleId) AS source
ON target.UserId = source.UserId AND target.RoleId = source.RoleId
WHEN NOT MATCHED THEN
    INSERT (UserId, RoleId) VALUES (source.UserId, source.RoleId);

-- Ensure Admin User Profile Exists
IF NOT EXISTS (SELECT 1 FROM UserProfiles WHERE UserId = @AdminUserId)
BEGIN
    INSERT INTO UserProfiles (UserId, FullName) 
    VALUES (@AdminUserId, 'Admin User');
END
