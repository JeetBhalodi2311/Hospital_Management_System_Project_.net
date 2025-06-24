-- DEPARTMENT TABLE SELECT ALL PROCEDURE 
CREATE OR ALTER PROCEDURE [DBO].[PR_Department_SelectAll]
AS
BEGIN

	SELECT	
			[DBO].[Department].[DepartmentID],
			[DBO].[Department].[DepartmentName],
			[DBO].[Department].[IsActive],
			[DBO].[Department].[Created],
			[DBO].[Department].[Modified], 
			[DBO].[Department].[Description],
			[DBO].[Department].[UserID]
			
	FROM [DBO].[Department]
END


--DEPARTMENT TABLE SELECT BY PK PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Department_SelectByPK]

@DEPARTMENTID		INT

AS
BEGIN
		SELECT	
				[DBO].[Department].[DepartmentID],
				[DBO].[Department].[DepartmentName],
				[DBO].[Department].[IsActive],
				[DBO].[Department].[Created],
				[DBO].[Department].[Modified], 
				[DBO].[Department].[Description],
				[DBO].[Department].[UserID]
			
		FROM [DBO].[Department]
			
		WHERE [DBO].[Department].[DepartmentID] = @DEPARTMENTID
END


--DEPARTMENT TABLE INSERT PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Department_Insert]

@DEPARTMENTNAME			NVARCHAR(100),
@DESCRIPTION			NVARCHAR(100),
@ISACTIVE				BIT,
@CREATED				DATETIME,
@MODIFIED				DATETIME,
@USERID					INT

AS
BEGIN

	INSERT INTO [DBO].[Department]
	(
		
		[DBO].[Department].[DepartmentName],
		[DBO].[Department].[Description],
		[DBO].[Department].[IsActive],
		[DBO].[Department].[Created],
		[DBO].[Department].[Modified],
		[DBO].[Department].[UserID]
	)
	VALUES
	(
		
		@DEPARTMENTNAME,
		@DESCRIPTION,
		@ISACTIVE,
		@CREATED,
		@MODIFIED,
		@USERID
	)
END


--DEPARTMENT TABLE UPDATE BY PK PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Department_UpdateByPK]

@DEPARTMENTID			INT,
@DEPARTMENTNAME		NVARCHAR(100),
@DESCRIPTION			NVARCHAR(100),
@ISACTIVE				BIT,
@CREATED				DATETIME,
@MODIFIED				DATETIME,
@USERID					INT

AS
BEGIN
	
	UPDATE [DBO].[Department]

	SET	[DBO].[Department].[DepartmentName] = @DEPARTMENTNAME,
		[DBO].[Department].[IsActive] = @ISACTIVE,
		[DBO].[Department].[Created] = @CREATED,
		[DBO].[Department].[Modified] = @MODIFIED,
		[DBO].[Department].[Description] = @DESCRIPTION,
		[DBO].[Department].[UserID] = @USERID

	WHERE [DBO].[Department].[DepartmentID] = @DEPARTMENTID
END


--DEPARTMENT TABLE DELETE BY PK PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Department_DeleteByPK]

@DEPARTMENTID			INT

AS
BEGIN

	DELETE 
	FROM [DBO].[Department]
	WHERE [DBO].[Department].[DepartmentID] = @DEPARTMENTID

END
