--DOCTOR TABLE SELECT ALL PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Doctor_SelectAll]
AS
BEGIN
	
	SELECT	
			 [DBO].[Doctor].[DoctorID]
			,[DBO].[Doctor].[Name]
			,[DBO].[Doctor].[Phone]
			,[DBO].[Doctor].[Email]
			,[DBO].[Doctor].[Qualification]
			,[DBO].[Doctor].[Specialization]
			,[DBO].[Doctor].[IsActive]
			,[DBO].[Doctor].[Created]
			,[DBO].[Doctor].[Modified]
			,[DBO].[Doctor].[UserID]

	FROM [DBO].[Doctor]

END
	

--DOCTOR TABLE SELECT BY PK PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Doctor_SelectByPK]

@DOCTORID				INT

AS
BEGIN

		SELECT	
				[DBO].[Doctor].[DoctorID]
				,[DBO].[Doctor].[Name]
				,[DBO].[Doctor].[Phone]
				,[DBO].[Doctor].[Email]
				,[DBO].[Doctor].[Qualification]
				,[DBO].[Doctor].[Specialization]
				,[DBO].[Doctor].[IsActive]
				,[DBO].[Doctor].[Created]
				,[DBO].[Doctor].[Modified]
				,[DBO].[Doctor].[UserID]

		FROM [DBO].[Doctor]

		WHERE [DBO].[Doctor].[DoctorID] = @DOCTORID
END


--DOCTOE TABLE INSERT TABLE PROCUDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Doctor_Insert]

@NAME				 NVARCHAR(100),
@PHONE				 NVARCHAR(20),
@EMAIL				 NVARCHAR(100),
@QUALIFICATION		 NVARCHAR(100),
@SPECIALIZATION		 NVARCHAR(100),
@ISACTIVE			 BIT, 
@CREATED			 DATETIME,
@MODIFIED			 DATETIME,
@USERID				 INT

AS
BEGIN
	INSERT INTO [DBO].[Doctor]
	(
			 [DBO].[Doctor].[Name]
			,[DBO].[Doctor].[Phone]
			,[DBO].[Doctor].[Email]
			,[DBO].[Doctor].[Qualification]
			,[DBO].[Doctor].[Specialization]
			,[DBO].[Doctor].[IsActive]
			,[DBO].[Doctor].[Created]
			,[DBO].[Doctor].[Modified]
			,[DBO].[Doctor].[UserID]
		
	)
	VALUES
	(
		@NAME,
		@PHONE,
		@EMAIL,
		@QUALIFICATION,
		@SPECIALIZATION,
		@ISACTIVE,
		@CREATED,
		@MODIFIED,
		@USERID
	)
END
	

--DOCTOR TABLE UPDATE BY PK PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Department_UpdateByPK]

@DOCTORID			 INT,
@NAME				 NVARCHAR(100),
@PHONE				 NVARCHAR(20),
@EMAIL				 NVARCHAR(100),
@QUALIFICATION		 NVARCHAR(100),
@SPECIALIZATION		 NVARCHAR(100),
@ISACTIVE			 BIT, 
@CREATED			 DATETIME,
@MODIFIED			 DATETIME,
@USERID				 INT

AS
BEGIN
	UPDATE [DBO].[Doctor]

	SET [DBO].[Doctor].[Name] = @NAME,
		[DBO].[Doctor].[Phone] = @PHONE,
		[DBO].[Doctor].[Email] = @EMAIL,
		[DBO].[Doctor].[Qualification] = @QUALIFICATION,
		[DBO].[Doctor].[Specialization] = @SPECIALIZATION,
		[DBO].[Doctor].[IsActive] = @ISACTIVE,
		[DBO].[Doctor].[Created] = @CREATED,
		[DBO].[Doctor].[Modified] = @MODIFIED,
		[DBO].[Doctor].[UserID] = @USERID

	WHERE [DBO].[Doctor].[DoctorID] = @DOCTORID
END


--DOCTOR TABLE DELETE BY PK PROCEDURE
CREATE OR  ALTER PROCEDURE [DBO].[PR_Doctor_DeleteByPK]

@DOCTORID INT

AS
BEGIN
	DELETE
	FROM [DBO].[Doctor]
	WHERE [DBO].[Doctor].[DoctorID] = @DOCTORID
END
