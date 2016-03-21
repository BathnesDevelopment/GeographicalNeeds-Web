use [Geographical Needs]
go


--------------------------------
--        DROP TABLES         --
--------------------------------

-- If the tables do not exist then this section will error. 
-- For initial runs of the script onto a clean database it
-- can be commented out.
IF OBJECT_ID('dbo.FactInstance', 'U') IS NOT NULL
drop table FactInstance
go

IF OBJECT_ID('dbo.FactDimensionMapping', 'U') IS NOT NULL
drop table FactDimensionMapping
go

IF OBJECT_ID('dbo.FactDimensionSet', 'U') IS NOT NULL
drop table FactDimensionSet
go

IF OBJECT_ID('dbo.DimensionToFact', 'U') IS NOT NULL
drop table DimensionToFact
go

IF OBJECT_ID('dbo.DimensionValue', 'U') IS NOT NULL
drop table DimensionValue
go

IF OBJECT_ID('dbo.Dimension', 'U') IS NOT NULL
drop table Dimension
go

IF OBJECT_ID('dbo.Fact', 'U') IS NOT NULL
drop table Fact
go

IF OBJECT_ID('dbo.Properties', 'U') IS NOT NULL
drop table Properties
go

IF OBJECT_ID('dbo.LSOA', 'U') IS NOT NULL
drop table LSOA
go

IF OBJECT_ID('dbo.Geography', 'U') IS NOT NULL
drop table Geography
go

IF OBJECT_ID('dbo.GeographyType', 'U') IS NOT NULL
drop table GeographyType
go

IF OBJECT_ID('dbo.StagingColumns', 'U') IS NOT NULL
drop table StagingColumns
go

IF OBJECT_ID('dbo.StagingDatasets', 'U') IS NOT NULL
drop table StagingDatasets
go

IF OBJECT_ID('dbo.DataViewColumn', 'U') IS NOT NULL
drop table DataViewColumn
go

IF OBJECT_ID('dbo.DataViews', 'U') IS NOT NULL
drop table DataViews
go

IF OBJECT_ID('dbo.CreateStagingTable', 'P') IS NOT NULL
drop procedure CreateStagingTable
go

IF OBJECT_ID('dbo.AddColumnToTable', 'P') IS NOT NULL
drop procedure AddColumnToTable
go


--------------------------------
--       CREATE TABLES        --
--------------------------------

-- Create Table GeographyType
--
-- This contains the different levels of the Geographys that
-- are used within the system.
CREATE TABLE [dbo].[GeographyType]
(
	GeographyTypeID INT IDENTITY(1,1) PRIMARY KEY,
	GeographyType varchar(100) not null,
	OrderInHierarchy int not null
)

CREATE TABLE [dbo].[Geography]
(
	GeographyID INT IDENTITY(1,1) PRIMARY KEY,
	GeographyTypeID int foreign key references GeographyType(GeographyTypeID),
	MsoaCode varchar(20) not null,
	MsoaName varchar(50) not null, 
	WardCode varchar(20) null,
	WardName varchar(50) null,
	LsoaCode varchar(20) null,
	LsoaName varchar(50) null,
	Postcode varchar(20) null,
	UPRN FLOAT NULL,
	USRN FLOAT NULL,
	NAMENUMBER nvarchar(300) NULL,
	STREET nvarchar(100) NULL,
	LOCATION nvarchar(50) NULL,
	TOWN nvarchar(50) NULL,
	EASTING numeric(22, 4) NULL,
	NORTHING numeric(22, 4) NULL,
	LATITUDE float null,
	LONGITUDE float null,
	SP_GEOMETRY geometry null
)

-- Create Table LSOA
--
-- This contains all of the LSOA areas used by the ONS
-- and which are referenced in their data sets. There is
-- a hierarchy within the data whereby an LSOA is contained 
-- within a Ward which is contained within a MSOA. 
CREATE TABLE [dbo].[LSOA](
	[LsoaID] INT IDENTITY(1,1) PRIMARY KEY,	
	[LsoaCode] varchar(20) not null,
	[LsoaName] varchar(50) not null,
	[WardCode] varchar(20) not null,
	[WardName] varchar(50) not null,
	[MsoaCode] varchar(20) not null,
	[MsoaName] varchar(50) not null, 
	[CommunityArea] varchar(50) not null,
	[PlanningPolicyArea] varchar(50) not null,
	[LACode] varchar(20) not null
)

-- Create Table DimProperties. 
--
-- This contains all of the Properties in BANES as 
-- contained in the LLPG Dataset.It also contains 
-- Latitudes and Longitudes which allow
-- for mappings to be created. There is also a 
-- reference back to the LSOA table to allow for 
-- a more complete geographical lookup.
CREATE TABLE [dbo].[Properties](
	[PropertyID] INT IDENTITY(1,1) PRIMARY KEY,
	[LsoaID] int null foreign key references LSOA(LsoaID),
	[NAMENUMBER] [nvarchar](300) NOT NULL,
	[STREET] [nvarchar](100) NULL,
	[LOCATION] [nvarchar](50) NULL,
	[TOWN] [nvarchar](50) NULL,
	[UPRN] FLOAT NOT NULL UNIQUE,
	[USRN] [numeric](10, 0) NOT NULL,
	[POSTCODE] [nvarchar](8) NULL,
	[EASTING] [numeric](22, 4) NOT NULL,
	[NORTHING] [numeric](22, 4) NOT NULL,
	[LATITUDE] float not null,
	[LONGITUDE] float not null,
	[SP_GEOMETRY] geometry null
) ON [PRIMARY]
GO


-- Create Table Dimension
--
-- This contains all the general dimensions that the data can be sliced by. 
-- This will include a name for the dimension as the sole field initially. 
-- More can be added if necessary. Some Dimensions will be general 
-- ones that can be used across many data sets, such as Gender, 
-- Ethicity etc. and others will be very specific to one data set e.g CRM Status
CREATE TABLE [dbo].[Dimension](
	DimensionID INT IDENTITY(1,1) PRIMARY KEY,	
	DimensionName varchar(100) unique not null
)

-- Create Table DimensionValue
--
-- This contains the specific values that each Dimension can take. 
-- This may be things such as “Male” and “Female” for Gender, 
-- or “16 -24”, “25 – 49” and “50+” for Age Range.
CREATE TABLE [dbo].[DimensionValue]
(
	DimensionValueID INT IDENTITY(1,1) PRIMARY KEY,	
	DimensionID INT not null foreign key references Dimension(DimensionID),
	DimensionValue varchar(1000) not null
	constraint uc_DimensionValue unique (DimensionID, DimensionValue)
)

-- Create Table Fact
--
-- This represents a data set that is being loaded in. This may be 
-- something like “Job Seekers Allowance by Age and LSOA” or “CRM Activity”. 
-- Only the name of the dataset is held at this level, along with the level
-- of geography that the measure is against.
CREATE TABLE [dbo].[Fact]
(
	FactID INT IDENTITY(1,1) PRIMARY KEY,
	FactName varchar(100) unique not null,
	GeographyTypeID int not null foreign key references GeographyType(GeographyTypeID)
)

-- Create Table DimensionToFact
--
-- This is a simple mapping table between each Fact Dimension Set 
-- and the Dimension Value that allows for consistency in the data schema
CREATE TABLE [dbo].[DimensionToFact]
(
	DimensionToFactID INT IDENTITY(1,1) PRIMARY KEY,
	FactID int not null foreign key references Fact(FactID),
	DimensionID int not null foreign key references Dimension(DimensionID)
)

-- Create Table FactDimensionSet
--
-- This creates a record for each possible combination of Dimension Values
-- that the Fact can be stored against. This ends up being a cross product
-- of the Dimension values. A String representation of each of the combinations
-- is manintained for ease.
CREATE TABLE [dbo].[FactDimensionSet]
(
	FactDimensionSetID INT IDENTITY(1,1) PRIMARY KEY,
	FactID int not null foreign key references Fact(FactID),
	DimString varchar(200) not null
)

-- Create Table FactDimensionMapping
--
-- This is a simple mapping table between each Fact Dimension Set 
-- and the Dimension Value that allows for consistency in the data schema
CREATE TABLE [dbo].[FactDimensionMapping]
(
	FactDimensionMappingID INT IDENTITY(1,1) PRIMARY KEY,
	FactDimensionSetID int not null foreign key references FactDimensionSet(FactDimensionSetID),
	DimensionValueID int not null foreign key references DimensionValue(DimensionValueID)
)

-- Create Table FactInstance
--
-- The values for the Facts are held in this table referencing the Dimensions 
-- that they are for. At present this only holds integer values as Facts but it
-- could be expanded to include other values.
CREATE TABLE [dbo].[FactInstance]
(
	FactInstanceID INT IDENTITY(1,1) PRIMARY KEY,
	FactDimensionSetID int not null foreign key references FactDimensionSet(FactDimensionSetID),
	GeographyID int null foreign key references Geography(GeographyID),
	Value int not null,
	LoadReference varchar(100) not null
)

-- Create Table StagingDatasets
--
-- A list of all the Staging Datasets held in the database
CREATE TABLE [dbo].[StagingDatasets]
(
	StagingDatasetID INT IDENTITY(1,1) PRIMARY KEY,
	DatasetName varchar(100) unique not null
)

-- Create Table StagingColumns
--
-- A list of all the columns contained withing Staging Datasets 
CREATE TABLE [dbo].[StagingColumns]
(
	StagingColumnID INT IDENTITY(1,1) PRIMARY KEY,
	StagingDatasetID INT NOT NULL foreign key references StagingDatasets(StagingDatasetID),
	ColumnName varchar(100) not null
	constraint uc_StagingColumn unique (StagingDatasetID, ColumnName)
)

-- Create Table DataViews
--
-- A list of all the Views created by the System that are held in the database
CREATE TABLE [dbo].[DataViews]
(
	DataViewID INT IDENTITY(1,1) PRIMARY KEY,
	ViewName varchar(100) unique not null
)

-- Create Table DataViewColumn
--
-- A list of all the columns contained withing Views 
CREATE TABLE [dbo].[DataViewColumn]
(
	DataViewColumnID INT IDENTITY(1,1) PRIMARY KEY,
	DataViewID INT NOT NULL foreign key references DataViews(DataViewID),
	ColumnName varchar(100) not null
	constraint uc_DataViewColumn unique (DataViewID, ColumnName)
)
go



-- Procedure to create a Table given on a name. This Table will be used for the 
-- Staging of data by the system before it gets loaded in to the main database
CREATE PROCEDURE [dbo].[CreateStagingTable] 
	-- Add the parameters for the stored procedure here
	@tableName nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    declare @sqlToExecute varchar(100) = 'create table ' + @tableName + '( UploadRef varchar(100) not null )';

	Exec (@sqlToExecute)
		
END

GO


-- Procedure to add a column to a table that has already been 
-- created. 
CREATE PROCEDURE AddColumnToTable 
	-- Add the parameters for the stored procedure here
	@tableName varchar(100) , 
	@columnName varchar(100)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @sqlToExecute varchar(100) = 'alter table ' + @tableName + ' add [' + @columnName + '] varchar(max)';

	Exec (@sqlToExecute)
	
END
GO


-- Trigger which creates a Staging table whenever a record is added to the Staging Dataset
-- table.
create trigger StagingInsert 
on StagingDatasets 
after Insert 
as 
	declare @temp varchar(100)
	select @temp = I.DatasetName from inserted I
	
	EXEC	[dbo].[CreateStagingTable]
			@tableName  = @temp
go
