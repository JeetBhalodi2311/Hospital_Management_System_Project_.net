--DOCTERDEPARTMENT TABLE SELECT ALL
CREATE OR ALTER PROCEDURE [DBO].[PR_DoctorDepartment_SelectAll]
AS
BEGIN
	SELECT 
			[DBO].[DoctorDepartment].[DoctorDepartmentID],
			[DBO].[DoctorDepartment].[DepartmentID],
			[DBO].[Department].[DepartmentName],
			[DBO].[DoctorDepartment].[DoctorID],
			[DBO].[Doctor].[Name],
			[DBO].[DoctorDepartment].[Created],
			[DBO].[DoctorDepartment].[Modified],
			[DBO].[DoctorDepartment].[UserID],
			[DBO].[User].[UserName]

	FROM [DBO].[DoctorDepartment]

	INNER JOIN [DBO].[User]
	
	ON [DBO].[DoctorDepartment].[UserID] = [DBO].[User].[UserName]

	INNER JOIN [DBO].[Department]

	ON [DBO].[DoctorDepartment].[DepartmentID] = [DBO].[Department].[DepartmentID]
	
	INNER JOIN [DBO].[Doctor]

	ON [DBO].[DoctorDepartment].[DoctorID] = [DBO].[Doctor].[DoctorID] 
END


--DOCTORDEPARTMENT TABLE SELECT BY PK
CREATE OR ALTER PROCEDURE [DBO].[PR_DoctorDepartment_SelectByPK]

@DOCTORDEPARTMENTID			INT

AS
BEGIN
	SELECT 
			[DBO].[DoctorDepartment].[DoctorDepartmentID],
			[DBO].[DoctorDepartment].[DepartmentID],
			[DBO].[Department].[DepartmentName],
			[DBO].[DoctorDepartment].[DoctorID],
			[DBO].[Doctor].[Name],
			[DBO].[DoctorDepartment].[Created],
			[DBO].[DoctorDepartment].[Modified],
			[DBO].[DoctorDepartment].[UserID],
			[DBO].[User].[UserName]

	FROM [DBO].[DoctorDepartment]

	INNER JOIN [DBO].[User]
	
	ON [DBO].[DoctorDepartment].[UserID] = [DBO].[User].[UserName]

	INNER JOIN [DBO].[Department]

	ON [DBO].[DoctorDepartment].[DepartmentID] = [DBO].[Department].[DepartmentID]
	
	INNER JOIN [DBO].[Doctor]

	ON [DBO].[DoctorDepartment].[DoctorID] = [DBO].[Doctor].[DoctorID]
	
	WHERE [DBO].[DoctorDepartment].[DoctorDepartmentID] = @DOCTORDEPARTMENTID

END


--DOCTORDEPARTMENT TABLE INSERT
CREATE OR ALTER PROCEDURE [DBO].[PR_DoctorDepartment_Insert]

@DOCTORID				INT,
@DEPARTMENTID			INT,
@CREATED				DATETIME,
@MODIFIED				DATETIME,
@USERID					INT

AS
BEGIN
	
	INSERT INTO [DBO].[DoctorDepartment]
	(
		[DBO].[DoctorDepartment].[DoctorID],
		[DBO].[DoctorDepartment].[DepartmentID],
		[DBO].[DoctorDepartment].[Created],
		[DBO].[DoctorDepartment].[Modified],
		[DBO].[DoctorDepartment].[UserID]
	)
	VALUES 
	(
		@DOCTORID,
		@DEPARTMENTID,
		@CREATED,
		@MODIFIED,
		@USERID
	)
END


--DOCTORCEPARTMENT UPDATE BY PK 
CREATE OR ALTER PROCEDURE [DBO].[PR_DoctorDepartment_UpdateByPK]

@DOCTORDEPARTMENTID			INT,
@DOCTORID					INT,
@DEPARTMENTID				INT,
@CREATED					DATETIME,
@MODIFIED					DATETIME,
@USERID						INT

AS
BEGIN
	UPDATE [DBO].[DoctorDepartment]
	SET [DBO].[DoctorDepartment].[DoctorID] = @DOCTORID,
		[DBO].[DoctorDepartment].[DepartmentID] = @DEPARTMENTID,
		[DBO].[DoctorDepartment].[Created] = @CREATED,
		[DBO].[DoctorDepartment].[Modified] = @MODIFIED,
		[DBO].[DoctorDepartment].[UserID] = @USERID

	WHERE [DBO].[DoctorDepartment].[DoctorDepartmentID] = @DOCTORDEPARTMENTID
END


--DOCTOEDEPARTMENT DELETE BY PK
CREATE OR ALTER PROCEDURE [DBO].[PR_DoctorDepartment_DeleteByPK]

@DOCTORDEPARTMENTID			INT

AS
BEGIN
	DELETE
	FROM [DBO].[DoctorDepartment]
	WHERE [DBO].[DoctorDepartment].[DoctorDepartmentID] = @DOCTORDEPARTMENTID
END
