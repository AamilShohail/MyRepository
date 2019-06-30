
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/18/2019 07:05:34
-- Generated from EDMX file: E:\dotNet CodeDream Team\TourOperator\Models\DreamTourModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DreamTour];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__PackageAc__AccID__336AA144]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PackageAccommodation] DROP CONSTRAINT [FK__PackageAc__AccID__336AA144];
GO
IF OBJECT_ID(N'[dbo].[FK__PackageAc__PackI__345EC57D]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PackageAccommodation] DROP CONSTRAINT [FK__PackageAc__PackI__345EC57D];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Accommodation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Accommodation];
GO
IF OBJECT_ID(N'[dbo].[Locations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locations];
GO
IF OBJECT_ID(N'[dbo].[Package]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Package];
GO
IF OBJECT_ID(N'[dbo].[PackageAccommodation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PackageAccommodation];
GO
IF OBJECT_ID(N'[dbo].[SystemUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemUser];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'SystemUsers'
CREATE TABLE [dbo].[SystemUsers] (
    [UserID] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(50)  NULL,
    [LastName] nvarchar(50)  NULL,
    [DateOfBirth] datetime  NULL,
    [Gender] nvarchar(10)  NULL,
    [Email] nvarchar(250)  NULL,
    [Password] nvarchar(max)  NULL,
    [ContactNumber] nchar(15)  NULL,
    [ResetPasswordCoded] nvarchar(100)  NULL,
    [DisplayPicture] nvarchar(max)  NULL
);
GO

-- Creating table 'Locations'
CREATE TABLE [dbo].[Locations] (
    [LocationID] int  NOT NULL,
    [Title] varchar(100)  NOT NULL,
    [Lat] varchar(10)  NOT NULL,
    [Long] varchar(10)  NOT NULL,
    [Province] varchar(50)  NOT NULL,
    [District] varchar(50)  NOT NULL,
    [Address] varchar(200)  NULL,
    [ImagePath] varchar(200)  NULL
);
GO

-- Creating table 'Packages'
CREATE TABLE [dbo].[Packages] (
    [PackageID] int IDENTITY(1,1) NOT NULL,
    [PackageType] varchar(50)  NULL,
    [Status] varchar(50)  NULL,
    [TourFare] decimal(19,4)  NULL
);
GO

-- Creating table 'Accommodations'
CREATE TABLE [dbo].[Accommodations] (
    [AccID] int IDENTITY(1,1) NOT NULL,
    [Latitude] nchar(10)  NOT NULL,
    [Longitude] nchar(10)  NOT NULL,
    [AccType] nchar(15)  NOT NULL,
    [ContactNumber] nvarchar(max)  NOT NULL,
    [Cost] decimal(19,4)  NOT NULL
);
GO

-- Creating table 'SystemUsers_Administration'
CREATE TABLE [dbo].[SystemUsers_Administration] (
    [AdminUserName] nchar(15)  NULL,
    [UserID] int  NOT NULL
);
GO

-- Creating table 'SystemUsers_Tourist'
CREATE TABLE [dbo].[SystemUsers_Tourist] (
    [TouristType] nchar(10)  NULL,
    [IsEmailVerified] bit  NULL,
    [ActivationCode] uniqueidentifier  NULL,
    [Country] nchar(30)  NULL,
    [UserID] int  NOT NULL
);
GO

-- Creating table 'Packages_Default'
CREATE TABLE [dbo].[Packages_Default] (
    [DefaultPackages] varchar(100)  NULL,
    [PackageID] int  NOT NULL
);
GO

-- Creating table 'Packages_Custom_Built'
CREATE TABLE [dbo].[Packages_Custom_Built] (
    [CustomizablePackages] nchar(10)  NULL,
    [PackageID] int  NOT NULL
);
GO

-- Creating table 'PackageAccommodation'
CREATE TABLE [dbo].[PackageAccommodation] (
    [Accommodations_AccID] int  NOT NULL,
    [Packages_PackageID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [UserID] in table 'SystemUsers'
ALTER TABLE [dbo].[SystemUsers]
ADD CONSTRAINT [PK_SystemUsers]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [LocationID] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([LocationID] ASC);
GO

-- Creating primary key on [PackageID] in table 'Packages'
ALTER TABLE [dbo].[Packages]
ADD CONSTRAINT [PK_Packages]
    PRIMARY KEY CLUSTERED ([PackageID] ASC);
GO

-- Creating primary key on [AccID] in table 'Accommodations'
ALTER TABLE [dbo].[Accommodations]
ADD CONSTRAINT [PK_Accommodations]
    PRIMARY KEY CLUSTERED ([AccID] ASC);
GO

-- Creating primary key on [UserID] in table 'SystemUsers_Administration'
ALTER TABLE [dbo].[SystemUsers_Administration]
ADD CONSTRAINT [PK_SystemUsers_Administration]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [UserID] in table 'SystemUsers_Tourist'
ALTER TABLE [dbo].[SystemUsers_Tourist]
ADD CONSTRAINT [PK_SystemUsers_Tourist]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [PackageID] in table 'Packages_Default'
ALTER TABLE [dbo].[Packages_Default]
ADD CONSTRAINT [PK_Packages_Default]
    PRIMARY KEY CLUSTERED ([PackageID] ASC);
GO

-- Creating primary key on [PackageID] in table 'Packages_Custom_Built'
ALTER TABLE [dbo].[Packages_Custom_Built]
ADD CONSTRAINT [PK_Packages_Custom_Built]
    PRIMARY KEY CLUSTERED ([PackageID] ASC);
GO

-- Creating primary key on [Accommodations_AccID], [Packages_PackageID] in table 'PackageAccommodation'
ALTER TABLE [dbo].[PackageAccommodation]
ADD CONSTRAINT [PK_PackageAccommodation]
    PRIMARY KEY CLUSTERED ([Accommodations_AccID], [Packages_PackageID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Accommodations_AccID] in table 'PackageAccommodation'
ALTER TABLE [dbo].[PackageAccommodation]
ADD CONSTRAINT [FK_PackageAccommodation_Accommodation]
    FOREIGN KEY ([Accommodations_AccID])
    REFERENCES [dbo].[Accommodations]
        ([AccID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Packages_PackageID] in table 'PackageAccommodation'
ALTER TABLE [dbo].[PackageAccommodation]
ADD CONSTRAINT [FK_PackageAccommodation_Package]
    FOREIGN KEY ([Packages_PackageID])
    REFERENCES [dbo].[Packages]
        ([PackageID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PackageAccommodation_Package'
CREATE INDEX [IX_FK_PackageAccommodation_Package]
ON [dbo].[PackageAccommodation]
    ([Packages_PackageID]);
GO

-- Creating foreign key on [UserID] in table 'SystemUsers_Administration'
ALTER TABLE [dbo].[SystemUsers_Administration]
ADD CONSTRAINT [FK_Administration_inherits_SystemUser]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[SystemUsers]
        ([UserID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserID] in table 'SystemUsers_Tourist'
ALTER TABLE [dbo].[SystemUsers_Tourist]
ADD CONSTRAINT [FK_Tourist_inherits_SystemUser]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[SystemUsers]
        ([UserID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PackageID] in table 'Packages_Default'
ALTER TABLE [dbo].[Packages_Default]
ADD CONSTRAINT [FK_Default_inherits_Package]
    FOREIGN KEY ([PackageID])
    REFERENCES [dbo].[Packages]
        ([PackageID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PackageID] in table 'Packages_Custom_Built'
ALTER TABLE [dbo].[Packages_Custom_Built]
ADD CONSTRAINT [FK_Custom_Built_inherits_Package]
    FOREIGN KEY ([PackageID])
    REFERENCES [dbo].[Packages]
        ([PackageID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------