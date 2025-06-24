--PATIENT TABLE SELETC BY ALL
CREATE OR ALTER PROCEDURE [DBO].[PR_Patient_SelectAll]
AS
BEGIN
		SELECT
				[DBO].[Patient].[PatientID],
				[DBO].[Patient].[Name],
				[DBO].[Patient].[Phone],
				[DBO].[Patient].[Email],
				[DBO].[Patient].[Gender],
				[DBO].[Patient].[DateOfBirth],
				[DBO].[Patient].[Address],
				[DBO].[Patient].[City],
				[DBO].[Patient].[State],
				[DBO].[Patient].[IsActive],
				[DBO].[Patient].[Created],
				[DBO].[Patient].[Modified],
				[DBO].[Patient].[UserID]

		FROM [DBO].[Patient]
END



--PATIENT TABLE SELECT BY ALL
CREATE OR ALTER PROCEDURE [DBO].[PR_Patient_SelectByPK]

@PATIENTID			INT

AS
BEGIN
		SELECT
				[DBO].[Patient].[PatientID],
				[DBO].[Patient].[Name],
				[DBO].[Patient].[Phone],
				[DBO].[Patient].[Email],
				[DBO].[Patient].[Gender],
				[DBO].[Patient].[DateOfBirth],
				[DBO].[Patient].[Address],
				[DBO].[Patient].[City],
				[DBO].[Patient].[State],
				[DBO].[Patient].[IsActive],
				[DBO].[Patient].[Created],
				[DBO].[Patient].[Modified],
				[DBO].[Patient].[UserID]

		FROM [DBO].[Patient]

		WHERE [DBO].[Patient].[PatientID] = @PATIENTID
END



--PATIENT TABLE INSER PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Patient_Insert]

@NAME				NVARCHAR(100),
@DATEOFBIRTH		DATETIME,
@GENDER				NVARCHAR(10),
@EMAIL				NVARCHAR(100),
@PHONE				NVARCHAR(100),
@ADDRESS			NVARCHAR(250),
@CITY				NVARCHAR(100),
@STATE				NVARCHAR(100),
@ISACTIVE			BIT,
@CREATED			DATETIME,
@MODIFIED			DATETIME,
@USERID				INT

AS
BEGIN
	INSERT INTO [DBO].[Patient]
	(
		[DBO].[Patient].[Name],
		[DBO].[Patient].[DateOfBirth],
		[DBO].[Patient].[Gender],
		[DBO].[Patient].[Email],
		[DBO].[Patient].[Phone],
		[DBO].[Patient].[Address],
		[DBO].[Patient].[City],
		[DBO].[Patient].[State],
		[DBO].[Patient].[IsActive],
		[DBO].[Patient].[Created],
		[DBO].[Patient].[Modified],
		[DBO].[Patient].[UserID]
	)
	VALUES
	(
		@NAME,
		@DATEOFBIRTH,
		@GENDER,
		@EMAIL,
		@PHONE,
		@ADDRESS,
		@CITY,
		@STATE,
		@ISACTIVE,
		@CREATED,
		@MODIFIED,
		@USERID
	)
END



--PATIENT TABLE UPDATE BY PK
CREATE OR ALTER PROCEDURE [DBO].[PR_Patient_UpdateByPK]

@PATIENTID				INT,
@NAME					NVARCHAR(100),
@DATEOFBIRTH			DATETIME,
@GENDER					NVARCHAR(10),
@EMAIL					NVARCHAR(100),
@PHONE					NVARCHAR(100),
@ADDRESS				NVARCHAR(250),
@CITY					NVARCHAR(100),
@STATE					NVARCHAR(100),
@ISACTIVE				BIT,
@CREATED				DATETIME,
@MODIFIED				DATETIME,
@USERID					INT

AS
BEGIN
		UPDATE [DBO].[Patient]
		
		SET [DBO].[Patient].[Name] = @NAME,
			[DBO].[Patient].[DateOfBirth] = @DATEOFBIRTH,
			[DBO].[Patient].[Gender] = @GENDER,
			[DBO].[Patient].[Email] = @EMAIL,
			[DBO].[Patient].[Phone] = @PHONE,
			[DBO].[Patient].[Address] = @ADDRESS,
			[DBO].[Patient].[City] = @CITY,
			[DBO].[Patient].[State] = @STATE,
			[DBO].[Patient].[IsActive] = @ISACTIVE,
			[DBO].[Patient].[Created] = @CREATED,
			[DBO].[Patient].[Modified] = @MODIFIED,
			[DBO].[Patient].[UserID] = @USERID

		WHERE [DBO].[Patient].[PatientID] = @PATIENTID

END



--PATIENT TABLE DELETE BY PK
CREATE OR ALTER PROCEDURE [DBO].[PR_Patient_DeleteByPK]

@PATIENTID			INT

AS
BEGIN
	DELETE
	FROM [DBO].[Patient]
	WHERE  [DBO].[Patient].[PatientID] = @PATIENTID
END
