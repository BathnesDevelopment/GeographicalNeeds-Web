use [Geographical Needs]
go

-- The Dimensions being inserted into the database
-- at this point are common across a number of data sets
-- and will be reused may times.


-- Insert the Age Group Dimension into the Database
-- along with the corresponding values.
INSERT INTO Dimension
(
	DimensionName
)
VALUES
(
	'JSA Age Group'
)

DECLARE @DWPAgeGroupDimensionID int = SCOPE_IDENTITY() 

INSERT INTO DimensionValue
(
	DimensionID, 
	DimensionValue
)
VALUES
(
	@DWPAgeGroupDimensionID,
	'16 to 24'
),
(
	@DWPAgeGroupDimensionID,
	'25 to 49'
),
(
	@DWPAgeGroupDimensionID,
	'50 and over'
)



-- Insert the Sex Dimension into the Database
-- along with the corresponding values.
INSERT INTO Dimension
(
	DimensionName
)
VALUES
(
	'Sex'
)

DECLARE @DWPSexDimensionID int = SCOPE_IDENTITY() 

INSERT INTO DimensionValue
(
	DimensionID, 
	DimensionValue
)
VALUES
(
	@DWPSexDimensionID,
	'Male'
),
(
	@DWPSexDimensionID,
	'Female'
),
(
	@DWPSexDimensionID,
	'Not Known'
)



-- Insert the Gender Dimension into the Database
-- along with the corresponding values.
INSERT INTO Dimension
(
	DimensionName
)
VALUES
(
	'Gender Identity'
)

DECLARE @DWPGenderDimensionID int = SCOPE_IDENTITY() 

INSERT INTO DimensionValue
(
	DimensionID, 
	DimensionValue
)
VALUES
(
	@DWPGenderDimensionID,
	'Male'
),
(
	@DWPGenderDimensionID,
	'Female'
),
(
	@DWPGenderDimensionID,
	'Transexual - M/F'
),
(
	@DWPGenderDimensionID,
	'Transexual - F/M'
),
(
	@DWPGenderDimensionID,
	'Undecided'
),
(
	@DWPGenderDimensionID,
	'Intersex'
)



-- Insert the Ethnicity Dimension into the Database
-- along with the corresponding values.
INSERT INTO Dimension
(
	DimensionName
)
VALUES
(
	'Ethnicity'
)

DECLARE @EthnicityDimensionID int = SCOPE_IDENTITY() 

INSERT INTO DimensionValue
(
	DimensionID, 
	DimensionValue
)
VALUES
(
	@EthnicityDimensionID,
	'White'
),
(
	@EthnicityDimensionID,
	'Mixed Multiple'
),
(
	@EthnicityDimensionID,
	'Asian / Asian British'
),
(
	@EthnicityDimensionID,
	'Black/African/Caribbean/Black British'
),
(
	@EthnicityDimensionID,
	'Other'
)


-- Insert the Sexual Orientation Dimension into the Database
-- along with the corresponding values.
INSERT INTO Dimension
(
	DimensionName
)
VALUES
(
	'Sexual Orientation'
)

DECLARE @SexualOrientationDimensionID int = SCOPE_IDENTITY() 

INSERT INTO DimensionValue
(
	DimensionID, 
	DimensionValue
)
VALUES
(
	@SexualOrientationDimensionID,
	'Heterosexual'
),
(
	@SexualOrientationDimensionID,
	'Gay'
),
(
	@SexualOrientationDimensionID,
	'Lesbian'
),
(
	@SexualOrientationDimensionID,
	'Bisexual'
),
(
	@SexualOrientationDimensionID,
	'Other'
),
(
	@SexualOrientationDimensionID,
	'Prefer not to say'
)



-- Insert the Disability Dimension into the Database
-- along with the corresponding values.
INSERT INTO Dimension
(
	DimensionName
)
VALUES
(
	'Disability'
)

DECLARE @DisabilityDimensionID int = SCOPE_IDENTITY() 

INSERT INTO DimensionValue
(
	DimensionID, 
	DimensionValue
)
VALUES
(
	@DisabilityDimensionID,
	'Disabled'
),
(
	@DisabilityDimensionID,
	'Not Disabled'
),
(
	@DisabilityDimensionID,
	'Not Known'
)


-- Insert the DWP Benefits Age Ranges Dimension into the Database
-- along with the corresponding values.
INSERT INTO Dimension
(
	DimensionName
)
VALUES
(
	'DWP Benefits Age Ranges'
)

DECLARE @DWPAgeRangeDimensionID int = SCOPE_IDENTITY() 

INSERT INTO DimensionValue
(
	DimensionID, 
	DimensionValue
)
VALUES
(
	@DWPAgeRangeDimensionID,
	'Under 18'
),
(
	@DWPAgeRangeDimensionID,
	'18-24'
),
(
	@DWPAgeRangeDimensionID,
	'25-34'
),
(
	@DWPAgeRangeDimensionID,
	'35-44'
),
(
	@DWPAgeRangeDimensionID,
	'45-49'
),
(
	@DWPAgeRangeDimensionID,
	'50-54'
),
(
	@DWPAgeRangeDimensionID,
	'55-59'
),
(
	@DWPAgeRangeDimensionID,
	'60+'
)


-- Insert the ONS Working Age Ranges Dimension into the Database
-- along with the corresponding values.
INSERT INTO Dimension
(
	DimensionName
)
VALUES
(
	'ONS Working Age Ranges'
)

DECLARE @ONSWorkingRangeDimensionID int = SCOPE_IDENTITY() 

INSERT INTO DimensionValue
(
	DimensionID, 
	DimensionValue
)
VALUES
(
	@ONSWorkingRangeDimensionID,
	'Under 18'
),
(
	@ONSWorkingRangeDimensionID,
	'18-24'
),
(
	@ONSWorkingRangeDimensionID,
	'25-49'
),
(
	@ONSWorkingRangeDimensionID,
	'50+'
)


-- Insert the Decenery Age Ranges Dimension into the Database
-- along with the corresponding values.
INSERT INTO Dimension
(
	DimensionName
)
VALUES
(
	'Decenery Age Ranges'
)

DECLARE @DeceneryAgeRangeDimensionID int = SCOPE_IDENTITY() 

INSERT INTO DimensionValue
(
	DimensionID, 
	DimensionValue
)
VALUES
(
	@DeceneryAgeRangeDimensionID,
	'0-9'
),
(
	@DeceneryAgeRangeDimensionID,
	'10-19'
),
(
	@DeceneryAgeRangeDimensionID,
	'20-29'
),
(
	@DeceneryAgeRangeDimensionID,
	'30-39'
),
(
	@DeceneryAgeRangeDimensionID,
	'40-49'
),
(
	@DeceneryAgeRangeDimensionID,
	'50-59'
),
(
	@DeceneryAgeRangeDimensionID,
	'60-69'
),
(
	@DeceneryAgeRangeDimensionID,
	'70-79'
),
(
	@DeceneryAgeRangeDimensionID,
	'80-89'
),
(
	@DeceneryAgeRangeDimensionID,
	'90-99'
),
(
	@DeceneryAgeRangeDimensionID,
	'99+'
)


-- Insert the Census Age Ranges Dimension into the Database
-- along with the corresponding values.
INSERT INTO Dimension
(
	DimensionName
)
VALUES
(
	'Census Age Ranges'
)

DECLARE @CensusAgeRangeDimensionID int = SCOPE_IDENTITY() 

INSERT INTO DimensionValue
(
	DimensionID, 
	DimensionValue
)
VALUES
(
	@CensusAgeRangeDimensionID,
	'0-4'
),
(
	@CensusAgeRangeDimensionID,
	'5-7'
),
(
	@CensusAgeRangeDimensionID,
	'8-9'
),
(
	@CensusAgeRangeDimensionID,
	'10-14'
),
(
	@CensusAgeRangeDimensionID,
	'15'
),
(
	@CensusAgeRangeDimensionID,
	'16-17'
),
(
	@CensusAgeRangeDimensionID,
	'18-19'
),
(
	@CensusAgeRangeDimensionID,
	'20-24'
),
(
	@CensusAgeRangeDimensionID,
	'25-29'
),
(
	@CensusAgeRangeDimensionID,
	'30-44'
),
(
	@CensusAgeRangeDimensionID,
	'45-59'
),
(
	@CensusAgeRangeDimensionID,
	'60-64'
),
(
	@CensusAgeRangeDimensionID,
	'65-74'
),
(
	@CensusAgeRangeDimensionID,
	'75-84'
),
(
	@CensusAgeRangeDimensionID,
	'85-89'
),
(
	@CensusAgeRangeDimensionID,
	'90 and over'
)


-- Insert the Quinery Age Ranges Dimension into the Database
-- along with the corresponding values.
INSERT INTO Dimension
(
	DimensionName
)
VALUES
(
	'Quinery Age Ranges'
)

DECLARE @QuineryAgeRangeDimensionID int = SCOPE_IDENTITY() 

INSERT INTO DimensionValue
(
	DimensionID, 
	DimensionValue
)
VALUES
(
	@QuineryAgeRangeDimensionID,
	'0-4'
),
(
	@QuineryAgeRangeDimensionID,
	'5-9'
),
(
	@QuineryAgeRangeDimensionID,
	'10-14'
),
(
	@QuineryAgeRangeDimensionID,
	'15-19'
),
(
	@QuineryAgeRangeDimensionID,
	'20-24'
),
(
	@QuineryAgeRangeDimensionID,
	'25-29'
),
(
	@QuineryAgeRangeDimensionID,
	'30-34'
),
(
	@QuineryAgeRangeDimensionID,
	'35-39'
),
(
	@QuineryAgeRangeDimensionID,
	'40-44'
),
(
	@QuineryAgeRangeDimensionID,
	'45-49'
),
(
	@QuineryAgeRangeDimensionID,
	'50-54'
),
(
	@QuineryAgeRangeDimensionID,
	'55-59'
),
(
	@QuineryAgeRangeDimensionID,
	'60-64'
),
(
	@QuineryAgeRangeDimensionID,
	'65-69'
),
(
	@QuineryAgeRangeDimensionID,
	'70-74'
),
(
	@QuineryAgeRangeDimensionID,
	'75-79'
),
(
	@QuineryAgeRangeDimensionID,
	'80-84'
),
(
	@QuineryAgeRangeDimensionID,
	'85-89'
),
(
	@QuineryAgeRangeDimensionID,
	'90-94'
),
(
	@QuineryAgeRangeDimensionID,
	'95+'
)

-- Insert the Quinery Age Ranges Dimension into the Database
-- along with the corresponding values.
INSERT INTO Dimension
(
	DimensionName
)
VALUES
(
	'Age'
)

DECLARE @AgeDimensionID int = SCOPE_IDENTITY() 

INSERT INTO DimensionValue
(
	DimensionID, 
	DimensionValue
)
VALUES
(
 @AgeDimensionID,
 '0'
),
(
 @AgeDimensionID,
 '1'
),
(
 @AgeDimensionID,
 '2'
),
(
 @AgeDimensionID,
 '3'
),
(
 @AgeDimensionID,
 '4'
),
(
 @AgeDimensionID,
 '5'
),
(
 @AgeDimensionID,
 '6'
),
(
 @AgeDimensionID,
 '7'
),
(
 @AgeDimensionID,
 '8'
),
(
 @AgeDimensionID,
 '9'
),
(
 @AgeDimensionID,
 '10'
),
(
 @AgeDimensionID,
 '11'
),
(
 @AgeDimensionID,
 '12'
),
(
 @AgeDimensionID,
 '13'
),
(
 @AgeDimensionID,
 '14'
),
(
 @AgeDimensionID,
 '15'
),
(
 @AgeDimensionID,
 '16'
),
(
 @AgeDimensionID,
 '17'
),
(
 @AgeDimensionID,
 '18'
),
(
 @AgeDimensionID,
 '19'
),
(
 @AgeDimensionID,
 '20'
),
(
 @AgeDimensionID,
 '21'
),
(
 @AgeDimensionID,
 '22'
),
(
 @AgeDimensionID,
 '23'
),
(
 @AgeDimensionID,
 '24'
),
(
 @AgeDimensionID,
 '25'
),
(
 @AgeDimensionID,
 '26'
),
(
 @AgeDimensionID,
 '27'
),
(
 @AgeDimensionID,
 '28'
),
(
 @AgeDimensionID,
 '29'
),
(
 @AgeDimensionID,
 '30'
),
(
 @AgeDimensionID,
 '31'
),
(
 @AgeDimensionID,
 '32'
),
(
 @AgeDimensionID,
 '33'
),
(
 @AgeDimensionID,
 '34'
),
(
 @AgeDimensionID,
 '35'
),
(
 @AgeDimensionID,
 '36'
),
(
 @AgeDimensionID,
 '37'
),
(
 @AgeDimensionID,
 '38'
),
(
 @AgeDimensionID,
 '39'
),
(
 @AgeDimensionID,
 '40'
),
(
 @AgeDimensionID,
 '41'
),
(
 @AgeDimensionID,
 '42'
),
(
 @AgeDimensionID,
 '43'
),
(
 @AgeDimensionID,
 '44'
),
(
 @AgeDimensionID,
 '45'
),
(
 @AgeDimensionID,
 '46'
),
(
 @AgeDimensionID,
 '47'
),
(
 @AgeDimensionID,
 '48'
),
(
 @AgeDimensionID,
 '49'
),
(
 @AgeDimensionID,
 '50'
),
(
 @AgeDimensionID,
 '51'
),
(
 @AgeDimensionID,
 '52'
),
(
 @AgeDimensionID,
 '53'
),
(
 @AgeDimensionID,
 '54'
),
(
 @AgeDimensionID,
 '55'
),
(
 @AgeDimensionID,
 '56'
),
(
 @AgeDimensionID,
 '57'
),
(
 @AgeDimensionID,
 '58'
),
(
 @AgeDimensionID,
 '59'
),
(
 @AgeDimensionID,
 '60'
),
(
 @AgeDimensionID,
 '61'
),
(
 @AgeDimensionID,
 '62'
),
(
 @AgeDimensionID,
 '63'
),
(
 @AgeDimensionID,
 '64'
),
(
 @AgeDimensionID,
 '65'
),
(
 @AgeDimensionID,
 '66'
),
(
 @AgeDimensionID,
 '67'
),
(
 @AgeDimensionID,
 '68'
),
(
 @AgeDimensionID,
 '69'
),
(
 @AgeDimensionID,
 '70'
),
(
 @AgeDimensionID,
 '71'
),
(
 @AgeDimensionID,
 '72'
),
(
 @AgeDimensionID,
 '73'
),
(
 @AgeDimensionID,
 '74'
),
(
 @AgeDimensionID,
 '75'
),
(
 @AgeDimensionID,
 '76'
),
(
 @AgeDimensionID,
 '77'
),
(
 @AgeDimensionID,
 '78'
),
(
 @AgeDimensionID,
 '79'
),
(
 @AgeDimensionID,
 '80'
),
(
 @AgeDimensionID,
 '81'
),
(
 @AgeDimensionID,
 '82'
),
(
 @AgeDimensionID,
 '83'
),
(
 @AgeDimensionID,
 '84'
),
(
 @AgeDimensionID,
 '85'
),
(
 @AgeDimensionID,
 '86'
),
(
 @AgeDimensionID,
 '87'
),
(
 @AgeDimensionID,
 '88'
),
(
 @AgeDimensionID,
 '89'
),
(
 @AgeDimensionID,
 '90'
),
(
 @AgeDimensionID,
 '91'
),
(
 @AgeDimensionID,
 '92'
),
(
 @AgeDimensionID,
 '93'
),
(
 @AgeDimensionID,
 '94'
),
(
 @AgeDimensionID,
 '95'
),
(
 @AgeDimensionID,
 '96'
),
(
 @AgeDimensionID,
 '97'
),
(
 @AgeDimensionID,
 '98'
),
(
 @AgeDimensionID,
 '99'
),
(
 @AgeDimensionID,
 '100'
),
(
 @AgeDimensionID,
 '101'
),
(
 @AgeDimensionID,
 '102'
),
(
 @AgeDimensionID,
 '103'
),
(
 @AgeDimensionID,
 '104'
),
(
 @AgeDimensionID,
 '105'
),
(
 @AgeDimensionID,
 '106'
),
(
 @AgeDimensionID,
 '107'
),
(
 @AgeDimensionID,
 '108'
),
(
 @AgeDimensionID,
 '109'
),
(
 @AgeDimensionID,
 '110'
),
(
 @AgeDimensionID,
 '111'
),
(
 @AgeDimensionID,
 '112'
),
(
 @AgeDimensionID,
 '113'
),
(
 @AgeDimensionID,
 '114'
),
(
 @AgeDimensionID,
 '115'
),
(
 @AgeDimensionID,
 '116'
),
(
 @AgeDimensionID,
 '117'
),
(
 @AgeDimensionID,
 '118'
)
