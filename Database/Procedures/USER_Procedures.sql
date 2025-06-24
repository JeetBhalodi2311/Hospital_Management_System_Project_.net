--USER TABLE SELECT ALL PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_User_SelectAll]
AS
BEGIN
	SELECT	
			[DBO].[User].[UserID],
			[DBO].[User].[UserName],
			[DBO].[User].[Password],
			[DBO].[User].[Email],
			[DBO].[User].[MobileNo],
			[DBO].[User].[IsActive],
			[DBO].[User].[Created],
			[DBO].[User].[Modified]
		
	FROM [DBO].[User]

END


-- USER TABLE SELECT BY ID PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_User_SelectByPK]
	
	@USERID INT

AS
BEGIN
	
	SELECT	
			[DBO].[User].[UserID],
			[DBO].[User].[UserName],
			[DBO].[User].[Password],
			[DBO].[User].[Email],
			[DBO].[User].[MobileNo],
			[DBO].[User].[IsActive],
			[DBO].[User].[Created],
			[DBO].[User].[Modified]
			
	FROM [DBO].[User]
	
	WHERE [DBO].[User].[UserID] = @USERID
END


-- USER TABLE INSERT PROPCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_User_Insert]
	
	@USRENAME		NVARCHAR(100),
	@PASSWORD		NVARCHAR(100),
	@EMAIL			NVARCHAR(100),
	@MOBILENO		NVARCHAR(100),
	@ISACTIVE		BIT,
	@CREATED		DATETIME,
	@MODIFIED		DATETIME
	
AS
BEGIN
	
	INSERT INTO [DBO].[User]
	(
		[DBO].[User].[UserName],
		[DBO].[User].[Password],
		[DBO].[User].[Email],
		[DBO].[User].[MobileNo],
		[DBO].[User].[IsActive],
		[DBO].[User].[Created],
		[DBO].[User].[Modified]
	)
	VALUES
	(
		@USRENAME,
		@PASSWORD,
		@EMAIL,
		@MOBILENO,
		@ISACTIVE,
		@CREATED,
		@MODIFIED
	)
END


--USER TABLE UPDATE BY PK PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_User_UpdateByPK]
	
	@USERID			INT,
	@USRENAME		NVARCHAR(100),
	@PASSWORD		NVARCHAR(100),
	@EMAIL			NVARCHAR(100),
	@MOBILENO		NVARCHAR(100),
	@ISACTIVE		BIT,
	@CREATED		DATETIME,
	@MODIFIED		DATETIME

AS
BEGIN
	UPDATE [DBO].[User]

	SET
		[DBO].[User].[UserName] = @USRENAME,
		[DBO].[User].[Password] = @PASSWORD,
		[DBO].[User].[Email] = @EMAIL,
		[DBO].[User].[MobileNo] = @MOBILENO,
		[DBO].[User].[IsActive] = @ISACTIVE,
		[DBO].[User].[Created] = @CREATED,
		[DBO].[User].[Modified] = @MODIFIED
	
	WHERE [DBO].[User].[UserID] = @USERID
END


--USER TABLE DELETE BY PK PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_User_DeleteByPK]
	
	@USERID			INT

AS
BEGIN
	
	DELETE 
	FROM [DBO].[User]
	WHERE [DBO].[User].[UserID] = @USERID

END
	