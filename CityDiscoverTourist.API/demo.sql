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
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
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

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220430124354_Initial', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220430124459_Entity', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [ActivityLogs] (
    [Id] int NOT NULL IDENTITY,
    [CustomerId] nvarchar(450) NULL,
    [UserId] nvarchar(max) NULL,
    [Action] nvarchar(max) NULL,
    [Controller] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [IpAddress] nvarchar(max) NULL,
    [CreatedDate] datetime2 NULL,
    CONSTRAINT [PK_ActivityLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ActivityLogs_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id])
);
GO

CREATE TABLE [Cities] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Status] int NOT NULL,
    CONSTRAINT [PK_Cities] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Contacts] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [UrlSocial] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [LocationTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Status] int NOT NULL,
    CONSTRAINT [PK_LocationTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [OwnerPaymentPeriods] (
    [Id] int NOT NULL IDENTITY,
    [CreatedDate] datetime2 NULL,
    [EndDate] datetime2 NULL,
    CONSTRAINT [PK_OwnerPaymentPeriods] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [QuestItemTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Status] int NOT NULL,
    CONSTRAINT [PK_QuestItemTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [QuestTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Status] nvarchar(max) NULL,
    [DurationMode] nvarchar(max) NULL,
    [DistanceMode] nvarchar(max) NULL,
    CONSTRAINT [PK_QuestTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Wallets] (
    [Id] int NOT NULL IDENTITY,
    [Total] real NOT NULL,
    [CurrencyUnit] nvarchar(max) NULL,
    CONSTRAINT [PK_Wallets] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Commissions] (
    [Id] int NOT NULL IDENTITY,
    [MinAmount] int NOT NULL,
    [MaxAmount] int NOT NULL,
    [MinCount] int NOT NULL,
    [MaxCount] int NOT NULL,
    [Percentage] int NOT NULL,
    [QuestTypeId] int NOT NULL,
    CONSTRAINT [PK_Commissions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Commissions_QuestTypes_QuestTypeId] FOREIGN KEY ([QuestTypeId]) REFERENCES [QuestTypes] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [QuestOwners] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [Password] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [WalletId] int NOT NULL,
    CONSTRAINT [PK_QuestOwners] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_QuestOwners_Wallets_WalletId] FOREIGN KEY ([WalletId]) REFERENCES [Wallets] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Transactions] (
    [Id] int NOT NULL IDENTITY,
    [Total] real NOT NULL,
    [CreatedDate] datetime2 NULL,
    [TypeTransaction] nvarchar(max) NULL,
    [Status] nvarchar(max) NULL,
    [WalletId] int NOT NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Transactions_Wallets_WalletId] FOREIGN KEY ([WalletId]) REFERENCES [Wallets] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Quests] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Price] real NOT NULL,
    [EstimatedTime] nvarchar(max) NULL,
    [EstimatedDistance] nvarchar(max) NULL,
    [AvailableTime] datetime2 NULL,
    [Status] nvarchar(max) NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    [QuestTypeId] int NOT NULL,
    [QuestOwnerId] int NOT NULL,
    CONSTRAINT [PK_Quests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Quests_QuestOwners_QuestOwnerId] FOREIGN KEY ([QuestOwnerId]) REFERENCES [QuestOwners] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Quests_QuestTypes_QuestTypeId] FOREIGN KEY ([QuestTypeId]) REFERENCES [QuestTypes] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Areas] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Status] int NOT NULL,
    [CityId] int NOT NULL,
    [QuestId] int NOT NULL,
    CONSTRAINT [PK_Areas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Areas_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Areas_Quests_QuestId] FOREIGN KEY ([QuestId]) REFERENCES [Quests] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Competitions] (
    [Id] int NOT NULL IDENTITY,
    [QuestId] int NOT NULL,
    [CompetitionCode] nvarchar(max) NULL,
    CONSTRAINT [PK_Competitions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Competitions_Quests_QuestId] FOREIGN KEY ([QuestId]) REFERENCES [Quests] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [OwnerPayments] (
    [Id] int NOT NULL IDENTITY,
    [TotalAmount] real NOT NULL,
    [TotalCount] int NOT NULL,
    [Commission] real NOT NULL,
    [Status] int NOT NULL,
    [OwnerPaymentPeriodId] int NULL,
    [PaymentPeriodId] int NOT NULL,
    [QuestId] int NOT NULL,
    [OwnerId] int NOT NULL,
    [TransactionId] int NOT NULL,
    CONSTRAINT [PK_OwnerPayments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OwnerPayments_OwnerPaymentPeriods_OwnerPaymentPeriodId] FOREIGN KEY ([OwnerPaymentPeriodId]) REFERENCES [OwnerPaymentPeriods] ([Id]),
    CONSTRAINT [FK_OwnerPayments_QuestOwners_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [QuestOwners] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OwnerPayments_Quests_QuestId] FOREIGN KEY ([QuestId]) REFERENCES [Quests] ([Id]),
    CONSTRAINT [FK_OwnerPayments_Transactions_TransactionId] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions] ([Id])
);
GO

CREATE TABLE [Rewards] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [ReceivedDate] datetime2 NULL,
    [ExpiredDate] datetime2 NULL,
    [CustomerId] nvarchar(450) NULL,
    [QuestId] int NOT NULL,
    [Status] nvarchar(max) NULL,
    CONSTRAINT [PK_Rewards] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Rewards_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Rewards_Quests_QuestId] FOREIGN KEY ([QuestId]) REFERENCES [Quests] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Locations] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Longitude] nvarchar(max) NULL,
    [Latitude] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [Status] nvarchar(max) NULL,
    [AreaId] int NOT NULL,
    [LocationTypeId] int NOT NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Locations_Areas_AreaId] FOREIGN KEY ([AreaId]) REFERENCES [Areas] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Locations_LocationTypes_LocationTypeId] FOREIGN KEY ([LocationTypeId]) REFERENCES [LocationTypes] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [CustomerQuests] (
    [Id] int NOT NULL IDENTITY,
    [BeginPoint] nvarchar(max) NULL,
    [EndPoint] nvarchar(max) NULL,
    [CreatedDate] datetime2 NULL,
    [Rating] int NOT NULL,
    [FeedBack] nvarchar(max) NULL,
    [CustomerId] nvarchar(450) NULL,
    [CompetitionId] int NOT NULL,
    CONSTRAINT [PK_CustomerQuests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CustomerQuests_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_CustomerQuests_Competitions_CompetitionId] FOREIGN KEY ([CompetitionId]) REFERENCES [Competitions] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [QuestItems] (
    [Id] int NOT NULL IDENTITY,
    [QuestItemTypeId] int NOT NULL,
    [LocationId] int NOT NULL,
    [QuestId] int NOT NULL,
    [Content] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Duration] int NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    [QrCode] nvarchar(max) NULL,
    [TriggerMode] int NOT NULL,
    [RightAnswer] nvarchar(max) NULL,
    [AnswerImageUrl] nvarchar(max) NULL,
    CONSTRAINT [PK_QuestItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_QuestItems_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_QuestItems_QuestItemTypes_QuestItemTypeId] FOREIGN KEY ([QuestItemTypeId]) REFERENCES [QuestItemTypes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_QuestItems_Quests_QuestId] FOREIGN KEY ([QuestId]) REFERENCES [Quests] ([Id])
);
GO

CREATE TABLE [Payments] (
    [Id] int NOT NULL IDENTITY,
    [PaymentMethod] nvarchar(max) NULL,
    [Status] nvarchar(max) NULL,
    [CustomerQuestId] int NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Payments_CustomerQuests_CustomerQuestId] FOREIGN KEY ([CustomerQuestId]) REFERENCES [CustomerQuests] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [CustomerTasks] (
    [Id] int NOT NULL IDENTITY,
    [CurrentPoint] real NOT NULL,
    [Status] nvarchar(max) NULL,
    [CreatedDate] datetime2 NULL,
    [QuestItemId] int NOT NULL,
    [CustomerQuestId] int NOT NULL,
    CONSTRAINT [PK_CustomerTasks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CustomerTasks_CustomerQuests_CustomerQuestId] FOREIGN KEY ([CustomerQuestId]) REFERENCES [CustomerQuests] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CustomerTasks_QuestItems_QuestItemId] FOREIGN KEY ([QuestItemId]) REFERENCES [QuestItems] ([Id])
);
GO

CREATE TABLE [Suggestions] (
    [Id] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NULL,
    [Status] nvarchar(max) NULL,
    [QuestItemId] int NOT NULL,
    CONSTRAINT [PK_Suggestions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Suggestions_QuestItems_QuestItemId] FOREIGN KEY ([QuestItemId]) REFERENCES [QuestItems] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [CustomerAnswers] (
    [Id] int NOT NULL IDENTITY,
    [CustomerTaskId] int NOT NULL,
    [QuestItemId] int NOT NULL,
    [AnswerImageUrl] nvarchar(max) NULL,
    CONSTRAINT [PK_CustomerAnswers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CustomerAnswers_CustomerTasks_CustomerTaskId] FOREIGN KEY ([CustomerTaskId]) REFERENCES [CustomerTasks] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CustomerAnswers_QuestItems_QuestItemId] FOREIGN KEY ([QuestItemId]) REFERENCES [QuestItems] ([Id])
);
GO

CREATE TABLE [Notes] (
    [Id] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NULL,
    [Image] nvarchar(max) NULL,
    [CustomerTaskId] int NOT NULL,
    CONSTRAINT [PK_Notes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Notes_CustomerTasks_CustomerTaskId] FOREIGN KEY ([CustomerTaskId]) REFERENCES [CustomerTasks] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_ActivityLogs_CustomerId] ON [ActivityLogs] ([CustomerId]);
GO

CREATE INDEX [IX_Areas_CityId] ON [Areas] ([CityId]);
GO

CREATE INDEX [IX_Areas_QuestId] ON [Areas] ([QuestId]);
GO

CREATE INDEX [IX_Commissions_QuestTypeId] ON [Commissions] ([QuestTypeId]);
GO

CREATE INDEX [IX_Competitions_QuestId] ON [Competitions] ([QuestId]);
GO

CREATE INDEX [IX_CustomerAnswers_CustomerTaskId] ON [CustomerAnswers] ([CustomerTaskId]);
GO

CREATE INDEX [IX_CustomerAnswers_QuestItemId] ON [CustomerAnswers] ([QuestItemId]);
GO

CREATE INDEX [IX_CustomerQuests_CompetitionId] ON [CustomerQuests] ([CompetitionId]);
GO

CREATE INDEX [IX_CustomerQuests_CustomerId] ON [CustomerQuests] ([CustomerId]);
GO

CREATE INDEX [IX_CustomerTasks_CustomerQuestId] ON [CustomerTasks] ([CustomerQuestId]);
GO

CREATE INDEX [IX_CustomerTasks_QuestItemId] ON [CustomerTasks] ([QuestItemId]);
GO

CREATE INDEX [IX_Locations_AreaId] ON [Locations] ([AreaId]);
GO

CREATE INDEX [IX_Locations_LocationTypeId] ON [Locations] ([LocationTypeId]);
GO

CREATE INDEX [IX_Notes_CustomerTaskId] ON [Notes] ([CustomerTaskId]);
GO

CREATE INDEX [IX_OwnerPayments_OwnerId] ON [OwnerPayments] ([OwnerId]);
GO

CREATE INDEX [IX_OwnerPayments_OwnerPaymentPeriodId] ON [OwnerPayments] ([OwnerPaymentPeriodId]);
GO

CREATE INDEX [IX_OwnerPayments_QuestId] ON [OwnerPayments] ([QuestId]);
GO

CREATE UNIQUE INDEX [IX_OwnerPayments_TransactionId] ON [OwnerPayments] ([TransactionId]);
GO

CREATE UNIQUE INDEX [IX_Payments_CustomerQuestId] ON [Payments] ([CustomerQuestId]);
GO

CREATE INDEX [IX_QuestItems_LocationId] ON [QuestItems] ([LocationId]);
GO

CREATE INDEX [IX_QuestItems_QuestId] ON [QuestItems] ([QuestId]);
GO

CREATE INDEX [IX_QuestItems_QuestItemTypeId] ON [QuestItems] ([QuestItemTypeId]);
GO

CREATE UNIQUE INDEX [IX_QuestOwners_WalletId] ON [QuestOwners] ([WalletId]);
GO

CREATE INDEX [IX_Quests_QuestOwnerId] ON [Quests] ([QuestOwnerId]);
GO

CREATE INDEX [IX_Quests_QuestTypeId] ON [Quests] ([QuestTypeId]);
GO

CREATE INDEX [IX_Rewards_CustomerId] ON [Rewards] ([CustomerId]);
GO

CREATE INDEX [IX_Rewards_QuestId] ON [Rewards] ([QuestId]);
GO

CREATE INDEX [IX_Suggestions_QuestItemId] ON [Suggestions] ([QuestItemId]);
GO

CREATE INDEX [IX_Transactions_WalletId] ON [Transactions] ([WalletId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220430134217_Modify', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [QuestItems] ADD [ItemId] int NULL;
GO

ALTER TABLE [QuestItems] ADD [QuestItemId] int NOT NULL DEFAULT 0;
GO

CREATE INDEX [IX_QuestItems_ItemId] ON [QuestItems] ([ItemId]);
GO

ALTER TABLE [QuestItems] ADD CONSTRAINT [FK_QuestItems_QuestItems_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [QuestItems] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220430140153_a', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ActivityLogs]') AND [c].[name] = N'UserId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ActivityLogs] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [ActivityLogs] DROP COLUMN [UserId];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220430141545_RemoveUserIdFieldInLogs', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Quests] DROP CONSTRAINT [FK_Quests_QuestOwners_QuestOwnerId];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Quests]') AND [c].[name] = N'QuestOwnerId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Quests] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Quests] ALTER COLUMN [QuestOwnerId] int NULL;
GO

ALTER TABLE [Quests] ADD CONSTRAINT [FK_Quests_QuestOwners_QuestOwnerId] FOREIGN KEY ([QuestOwnerId]) REFERENCES [QuestOwners] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220430142036_AllowNull', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[QuestItems]') AND [c].[name] = N'QuestItemId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [QuestItems] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [QuestItems] ALTER COLUMN [QuestItemId] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220430145301_AllowNull2', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Areas] DROP CONSTRAINT [FK_Areas_Quests_QuestId];
GO

DROP INDEX [IX_Areas_QuestId] ON [Areas];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Areas]') AND [c].[name] = N'QuestId');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Areas] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Areas] DROP COLUMN [QuestId];
GO

ALTER TABLE [CustomerQuests] ADD [TeamCode] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220503124527_ModifyEntiy', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Quests] ADD [AreaId] int NOT NULL DEFAULT 0;
GO

CREATE INDEX [IX_Quests_AreaId] ON [Quests] ([AreaId]);
GO

ALTER TABLE [Quests] ADD CONSTRAINT [FK_Quests_Areas_AreaId] FOREIGN KEY ([AreaId]) REFERENCES [Areas] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220503130001_RelationAreaQuest', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OwnerPayments]') AND [c].[name] = N'Status');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [OwnerPayments] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [OwnerPayments] ALTER COLUMN [Status] nvarchar(max) NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LocationTypes]') AND [c].[name] = N'Status');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [LocationTypes] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [LocationTypes] ALTER COLUMN [Status] nvarchar(max) NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Cities]') AND [c].[name] = N'Status');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Cities] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Cities] ALTER COLUMN [Status] nvarchar(max) NULL;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Areas]') AND [c].[name] = N'Status');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Areas] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Areas] ALTER COLUMN [Status] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220503131556_ChangeDataTypeOfStatus', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [QuestOwners] ADD [Status] nvarchar(max) NULL;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[QuestItemTypes]') AND [c].[name] = N'Status');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [QuestItemTypes] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [QuestItemTypes] ALTER COLUMN [Status] nvarchar(max) NULL;
GO

ALTER TABLE [QuestItems] ADD [Status] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220503141218_Entities', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[QuestItems]') AND [c].[name] = N'QuestItemId');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [QuestItems] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [QuestItems] DROP COLUMN [QuestItemId];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220504024751_RemoveQuestItemId', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [QuestItems] DROP CONSTRAINT [FK_QuestItems_QuestItems_ItemId];
GO

DROP INDEX [IX_QuestItems_ItemId] ON [QuestItems];
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[QuestItems]') AND [c].[name] = N'ItemId');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [QuestItems] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [QuestItems] DROP COLUMN [ItemId];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220504034409_Default', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [QuestItems] ADD [ItemId] int NULL;
GO

CREATE INDEX [IX_QuestItems_ItemId] ON [QuestItems] ([ItemId]);
GO

ALTER TABLE [QuestItems] ADD CONSTRAINT [FK_QuestItems_QuestItems_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [QuestItems] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220504034502_DefaultValue', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [CustomerQuests] DROP CONSTRAINT [FK_CustomerQuests_Competitions_CompetitionId];
GO

DROP TABLE [Competitions];
GO

DROP INDEX [IX_CustomerQuests_CompetitionId] ON [CustomerQuests];
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CustomerQuests]') AND [c].[name] = N'CompetitionId');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [CustomerQuests] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [CustomerQuests] DROP COLUMN [CompetitionId];
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CustomerQuests]') AND [c].[name] = N'TeamCode');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [CustomerQuests] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [CustomerQuests] DROP COLUMN [TeamCode];
GO

ALTER TABLE [CustomerTasks] ADD [CountSuggestion] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220525143313_UpdateEntity', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [CustomerQuests] ADD [QuestId] int NOT NULL DEFAULT 0;
GO

CREATE INDEX [IX_CustomerQuests_QuestId] ON [CustomerQuests] ([QuestId]);
GO

ALTER TABLE [CustomerQuests] ADD CONSTRAINT [FK_CustomerQuests_Quests_QuestId] FOREIGN KEY ([QuestId]) REFERENCES [Quests] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220525143728_AddKeyForQuestTable', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Quests] ADD [ImagePath] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220525144019_AddAttrForQuestTable', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [QuestTypes] ADD [ImagePath] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220530154734_AddAttr', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Quests]') AND [c].[name] = N'AvailableTime');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Quests] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [Quests] ALTER COLUMN [AvailableTime] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220531094357_ChangeDataType', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [CustomerQuests] ADD [Status] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220602120531_AddStatusField', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [CustomerTasks] ADD [IsFinished] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [CustomerQuests] ADD [IsFinished] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220603030929_AddFieldIsFinished', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [AspNetUsers] ADD [ImagePath] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220607141419_AddImagePathForUser', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [CustomerAnswers] ADD [CustomerReply] nvarchar(max) NULL;
GO

ALTER TABLE [CustomerAnswers] ADD [Note] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220607155520_AddFieldForCustomerAnswer', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [CustomerTasks] ADD [CountWrongAnswer] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220608124507_CountWrongAnswer', N'6.0.5');
GO

COMMIT;
GO