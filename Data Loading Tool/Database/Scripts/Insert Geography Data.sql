use [Geographical Needs]
go


USE [Geographical Needs]
GO

USE [Geographical Needs]
GO

INSERT INTO [dbo].[GeographyType]
           ([GeographyType]
           ,[OrderInHierarchy])
     VALUES
			('Property',1),
			('Postcode',2),
			('LSOA',3),
			('Ward',4),
			('MSOA',5)

GO



INSERT INTO [dbo].[Geography]
           ([GeographyTypeID]
           ,[MsoaCode]
           ,[MsoaName]
           )
select distinct	
	5, 
	[MSOA11 Code], 
	[MSOA Local Name] 
FROM
	sandbox.dbo.StageLSOAReference
where [MSOA11 Code] <> ''
GO


INSERT INTO [dbo].[Geography]
           ([GeographyTypeID]
           ,[MsoaCode]
           ,[MsoaName]
		   ,WardCode
		   ,WardName
           )
select distinct	
	4, 
	[MSOA11 Code], 
	[MSOA Local Name],
	[2013 Electoral Ward Code], 
	[2013 Electoral Ward Name] 
FROM
	sandbox.dbo.StageLSOAReference
where [MSOA11 Code] <> ''
GO



INSERT INTO [dbo].[Geography]
           ([GeographyTypeID]
           ,[MsoaCode]
           ,[MsoaName]
		   ,WardCode
		   ,WardName
		   ,LsoaCode 
		   ,LsoaName 
           )
select distinct	
	3, 
	[MSOA11 Code], 
	[MSOA Local Name],
	[2013 Electoral Ward Code], 
	[2013 Electoral Ward Name],
	[LSOA11 code],
	[LSOA11 Local Name] 
FROM
	sandbox.dbo.StageLSOAReference
where [MSOA11 Code] <> ''
GO

INSERT INTO [dbo].[Geography]
           ([GeographyTypeID]
           ,[MsoaCode]
           ,[MsoaName]
		   ,WardCode
		   ,WardName
		   ,LsoaCode 
		   ,LsoaName 
		   ,Postcode
           )
select distinct	
	2, 
	[MSOA11 Code], 
	[MSOA Local Name],
	[2013 Electoral Ward Code], 
	[2013 Electoral Ward Name],
	R.[LSOA11 code],
	R.[LSOA11 Local Name],
	L.[Postcode – single space]

FROM
	sandbox.dbo.StageLSOAReference R
inner join
	sandbox.dbo.PostcodeToLsoaLookup L
on R.[LSOA11 code] = L.[LSOA11 code]
where [MSOA11 Code] <> ''
GO


INSERT INTO [dbo].[Geography]
           ([GeographyTypeID]
           ,[MsoaCode]
           ,[MsoaName]
		   ,WardCode
		   ,WardName
		   ,LsoaCode 
		   ,LsoaName 
		   ,Postcode		   
		   ,NAMENUMBER
		   ,STREET
		   ,LOCATION
		   ,TOWN
		   ,UPRN
		   ,USRN
		   ,EASTING
		   ,NORTHING
		   ,LATITUDE
		   ,LONGITUDE
		   ,SP_GEOMETRY
           )
select 	
	1, 
	[MSOA11 Code], 
	[MSOA Local Name],
	[2013 Electoral Ward Code], 
	[2013 Electoral Ward Name],
	R.[LSOA11 code],
	R.[LSOA11 Local Name],
	L.[Postcode – single space],
	NAMENUMBER,
	STREET,
	LOC,
	TOWN,
	UPRN,
	USRN,
	EASTING,
	NORTHING,
	sandbox.dbo.NEtoLL(EASTING, NORTHING, 'Lat'),
	sandbox.dbo.NEtoLL(EASTING, NORTHING, 'Lng'),
	[SP_GEOMETRY]
	
FROM
sandbox.dbo.llpg_addresses_withGeom A
left join 
	sandbox.dbo.PostcodeToLsoaLookup L
on
	A.POSTCODE = L.[Postcode – single space]	
inner join
	sandbox.dbo.StageLSOAReference R
on R.[LSOA11 code] = L.[LSOA11 code]
where [MSOA11 Code] <> ''







---- Get all the data into the LSOA geography table
---- from the staging LSOA table point. The staging point
---- contains a direct copy of the Excel spreadsheet maintained
---- by the internal BANES team.

--INSERT INTO LSOA
--(
--	LsoaCode,
--	LsoaName,
--	WardCode,
--	WardName,
--	MsoaCode,
--	MsoaName,
--	CommunityArea,
--	PlanningPolicyArea,	
--	LACode
--)
--SELECT 
--	[LSOA11 code],
--	[LSOA11 Local Name],
--	[2013 Electoral Ward Code],
--	[2013 Electoral Ward Name],
--	[MSOA11 Code],
--	[MSOA Local Name],
--	[Community Area],
--	[Planning Policy Area],
--	[LA11 Code]
--FROM
--	sandbox.dbo.StageLSOAReference


---- Get all the data into the Properties geography
---- table and create links from each of the Properties to the
---- LSOAs inserted above.
--INSERT INTO Properties 
--(
--	NAMENUMBER,
--	STREET,
--	LOCATION,
--	TOWN,
--	UPRN,
--	USRN,
--	POSTCODE, 
--	EASTING,
--	NORTHING,
--	LATITUDE,
--	LONGITUDE,
--	SP_GEOMETRY,
--	LsoaID
--)
--SELECT 
--	NAMENUMBER,
--	STREET,
--	LOC,
--	TOWN,
--	UPRN,
--	USRN,
--	POSTCODE, 
--	EASTING,
--	NORTHING,
--	sandbox.dbo.NEtoLL(EASTING, NORTHING, 'Lat'),
--	sandbox.dbo.NEtoLL(EASTING, NORTHING, 'Lng'),
--	[SP_GEOMETRY],
--	B.LsoaID
--FROM
--	sandbox.dbo.llpg_addresses_withGeom A
--left join 
--	sandbox.dbo.PostcodeToLsoaLookup L
--on
--	A.POSTCODE = L.[Postcode – single space]
--inner join 
--	LSOA B
--on B.LsoaCode = L.[LSOA11 code]
