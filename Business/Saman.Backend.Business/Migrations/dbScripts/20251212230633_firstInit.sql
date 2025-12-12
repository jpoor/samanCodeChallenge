IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'samandb')
BEGIN
    CREATE DATABASE [samandb];
END;
GO

USE [samandb];
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'cor') IS NULL EXEC(N'CREATE SCHEMA [cor];');
GO

IF SCHEMA_ID(N'log') IS NULL EXEC(N'CREATE SCHEMA [log];');
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [IdentificationCode] nvarchar(450) NOT NULL,
    [ReferralCode] nvarchar(6) NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NULL,
    [ProfileImageFile] nvarchar(max) NULL,
    [CreatorId] nvarchar(max) NOT NULL,
    [CreationDate] datetime2 NOT NULL DEFAULT (GETDATE()),
    [Referrer_Id] nvarchar(450) NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(450) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUsers_AspNetUsers_Referrer_Id] FOREIGN KEY ([Referrer_Id]) REFERENCES [AspNetUsers] ([Id])
);
GO

CREATE TABLE [cor].[Category] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [PathByName] nvarchar(1000) NULL,
    [PathById] nvarchar(1000) NULL,
    [Level] int NOT NULL,
    [Parent_Id] int NULL,
    [CreatorId] nvarchar(50) NOT NULL,
    [CreationDate] datetime2 NOT NULL,
    [UpdaterId] nvarchar(50) NULL,
    [UpdateDate] datetime2 NULL,
    [Deactive] bit NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Category_Category_Parent_Id] FOREIGN KEY ([Parent_Id]) REFERENCES [cor].[Category] ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [log].[Log] (
    [Id] bigint NOT NULL IDENTITY,
    [Operation] tinyint NOT NULL,
    [EntityName] nvarchar(100) NOT NULL,
    [EntityId] nvarchar(10) NOT NULL,
    [EntityLog] nvarchar(max) NOT NULL,
    [User_Id] nvarchar(450) NOT NULL,
    [CreatorId] nvarchar(50) NOT NULL,
    [CreationDate] datetime2 NOT NULL,
    [UpdaterId] nvarchar(50) NULL,
    [UpdateDate] datetime2 NULL,
    [Deactive] bit NOT NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Log_AspNetUsers_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [cor].[Product] (
    [Id] int NOT NULL IDENTITY,
    [SKU] nvarchar(100) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [OnHand] int NOT NULL,
    [Sequence] int NOT NULL,
    [Category_Id] int NOT NULL,
    [CreatorId] nvarchar(50) NOT NULL,
    [CreationDate] datetime2 NOT NULL,
    [UpdaterId] nvarchar(50) NULL,
    [UpdateDate] datetime2 NULL,
    [Deactive] bit NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Product_Category_Category_Id] FOREIGN KEY ([Category_Id]) REFERENCES [cor].[Category] ([Id]) ON DELETE NO ACTION
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
VALUES (N'r01superadmin', NULL, N'SuperAdmin', N'SUPERADMIN'),
(N'r02admin', NULL, N'Admin', N'ADMIN'),
(N'r03support', NULL, N'Support', N'SUPPORT');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'CreationDate', N'CreatorId', N'Email', N'EmailConfirmed', N'FirstName', N'IdentificationCode', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'ProfileImageFile', N'ReferralCode', N'Referrer_Id', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
    SET IDENTITY_INSERT [AspNetUsers] ON;
INSERT INTO [AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [CreationDate], [CreatorId], [Email], [EmailConfirmed], [FirstName], [IdentificationCode], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [ProfileImageFile], [ReferralCode], [Referrer_Id], [SecurityStamp], [TwoFactorEnabled], [UserName])
VALUES (N'usridsuperadmin', 0, N'3a23c325-271e-4387-ba65-63d8497e57b4', '2025-12-12T23:06:32.4612878Z', N'0', NULL, CAST(0 AS bit), N'Super Admin', N'OWePCGHUqKgKrAjWSw', NULL, CAST(1 AS bit), NULL, NULL, N'SUPERADMIN', N'AQAAAAIAAYagAAAAEI8nEg+XoAuBQ0kxNJy3h1sIVpzNQxn7i/TaC5NvpxoN89dJP79nZcQzHPfVZLF+pw==', NULL, CAST(0 AS bit), NULL, N'TVDZKE', NULL, N'N4DNDTNCCHQALRBOVJOYXG3ACWHESEKK', CAST(0 AS bit), N'superadmin');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'CreationDate', N'CreatorId', N'Email', N'EmailConfirmed', N'FirstName', N'IdentificationCode', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'ProfileImageFile', N'ReferralCode', N'Referrer_Id', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
    SET IDENTITY_INSERT [AspNetUsers] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
    SET IDENTITY_INSERT [AspNetUserRoles] ON;
INSERT INTO [AspNetUserRoles] ([RoleId], [UserId])
VALUES (N'r01superadmin', N'usridsuperadmin');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
    SET IDENTITY_INSERT [AspNetUserRoles] OFF;
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE UNIQUE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]) WHERE NormalizedEmail IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_AspNetUsers_IdentificationCode] ON [AspNetUsers] ([IdentificationCode]);
GO

CREATE UNIQUE INDEX [IX_AspNetUsers_PhoneNumber] ON [AspNetUsers] ([PhoneNumber]) WHERE PhoneNumber IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_AspNetUsers_ReferralCode] ON [AspNetUsers] ([ReferralCode]);
GO

CREATE INDEX [IX_AspNetUsers_Referrer_Id] ON [AspNetUsers] ([Referrer_Id]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_Category_Name] ON [cor].[Category] ([Name]);
GO

CREATE INDEX [IX_Category_Parent_Id] ON [cor].[Category] ([Parent_Id]);
GO

CREATE INDEX [IX_Category_PathById] ON [cor].[Category] ([PathById]);
GO

CREATE INDEX [IX_Category_PathByName] ON [cor].[Category] ([PathByName]);
GO

CREATE INDEX [IX_Log_EntityId] ON [log].[Log] ([EntityId]);
GO

CREATE INDEX [IX_Log_EntityName] ON [log].[Log] ([EntityName]);
GO

CREATE INDEX [IX_Log_User_Id] ON [log].[Log] ([User_Id]);
GO

CREATE INDEX [IX_Product_Category_Id] ON [cor].[Product] ([Category_Id]);
GO

CREATE INDEX [IX_Product_Name] ON [cor].[Product] ([Name]);
GO

CREATE UNIQUE INDEX [IX_Product_SKU] ON [cor].[Product] ([SKU]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251212230633_firstInit', N'8.0.22');
GO

COMMIT;
GO

