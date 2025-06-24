--APPOINTMENT TABLE SELECT ALL PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Appointment_SelectAll]
AS
BEGIN
	SELECT 
			[DBO].[Appointment].[AppointmentID],
			[DBO].[Appointment].[DoctorID],
			[DBO].[Doctor].[Name],
			[DBO].[Appointment].[PatientID],
			[DBO].[Patient].[Name],
			[DBO].[Appointment].[AppointmentDate],
			[DBO].[Appointment].[AppointmentStatus],
			[DBO].[Appointment].[Description],
			[DBO].[Appointment].[SpecialRemarks],
			[DBO].[Appointment].[Created],
			[DBO].[Appointment].[Modified],
			[DBO].[Appointment].[UserID],
			[DBO].[Appointment].[TotalConsultedAmount]

		FROM [DBO].[Appointment]

		INNER JOIN [DBO].[Doctor]
		ON [DBO].[Appointment].[DoctorID] = [DBO].[Doctor].[DoctorID]

		INNER JOIN [DBO].[Patient]
		ON [DBO].[Appointment].[PatientID] = [DBO].[Patient].[PatientID]
END



--APPOINMENT TABLE SELECT BY PK
CREATE OR ALTER PROCEDURE [DBO].[PR_Appointment_SelectByPK]

@APPOINTMENTID				INT

AS
BEGIN
	SELECT 
			[DBO].[Appointment].[AppointmentID],
			[DBO].[Appointment].[DoctorID],
			[DBO].[Doctor].[Name],
			[DBO].[Appointment].[PatientID],
			[DBO].[Patient].[Name],
			[DBO].[Appointment].[AppointmentDate],
			[DBO].[Appointment].[AppointmentStatus],
			[DBO].[Appointment].[Description],
			[DBO].[Appointment].[SpecialRemarks],
			[DBO].[Appointment].[Created],
			[DBO].[Appointment].[Modified],
			[DBO].[Appointment].[UserID],
			[DBO].[Appointment].[TotalConsultedAmount]

		FROM [DBO].[Appointment]

		INNER JOIN [DBO].[Doctor]
		ON [DBO].[Appointment].[DoctorID] = [DBO].[Doctor].[DoctorID]

		INNER JOIN [DBO].[Patient]
		ON [DBO].[Appointment].[PatientID] = [DBO].[Patient].[PatientID]

		WHERE [DBO].[Appointment].[AppointmentID] = @APPOINTMENTID
END



--APPOINMENT TABLE INSERT PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Appointment_Insert]

@DOCTORID					INT,
@PATIENTID					INT,
@APPOINMENTDATE				DATETIME,
@APPOINMENTSTATUS			NVARCHAR(20),
@DESCRIPTION				NVARCHAR(250),
@SPECIALREMARKS				NVARCHAR(100),
@CREATED					DATETIME,
@MODIFIED					DATETIME,
@USERID						INT,
@TOTALCONSULTEDAMOUNT		DECIMAL(5, 2)

AS
BEGIN

	INSERT INTO [DBO].[Appointment]
	(
		[DBO].[Appointment].[DoctorID], 
		[DBO].[Appointment].[PatientID], 
		[DBO].[Appointment].[AppointmentDate],
		[DBO].[Appointment].[AppointmentStatus],
		[DBO].[Appointment].[Description],
		[DBO].[Appointment].[SpecialRemarks],
		[DBO].[Appointment].[Created],
		[DBO].[Appointment].[Modified],
		[DBO].[Appointment].[UserID],
		[DBO].[Appointment].[TotalConsultedAmount]
	)
	VALUES
	(
		@DOCTORID,
		@PATIENTID,
		@APPOINMENTDATE,
		@APPOINMENTSTATUS,
		@DESCRIPTION,
		@SPECIALREMARKS,
		@CREATED,
		@MODIFIED,
		@USERID,
		@TOTALCONSULTEDAMOUNT
	)
END


--APPOINMENT TABLE UPDATE BY PK PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Appointment_UpdateByPK]

@APPOINMENTID					INT,
@DOCTORID						INT,
@PATIENTID						INT,
@APPOINMENTDATE					DATETIME,
@APPOINMENTSTATUS				NVARCHAR(20),
@DESCRIPTION					NVARCHAR(250),
@SPECIALREMARKS					NVARCHAR(100),
@CREATED						DATETIME,
@MODIFIED						DATETIME,
@USERID							INT,
@TOTALCONSULTEDAMOUNT			DECIMAL(5, 2)

AS
BEGIN
	
	UPDATE [DBO].[Appointment]
	
	SET	[DBO].[Appointment].[DoctorID] = @DOCTORID,
		[DBO].[Appointment].[PatientID] = @PATIENTID ,
		[DBO].[Appointment].[AppointmentDate] = @APPOINMENTDATE,
		[DBO].[Appointment].[AppointmentStatus] = @APPOINMENTSTATUS,
		[DBO].[Appointment].[Description] = @DESCRIPTION,
		[DBO].[Appointment].[SpecialRemarks] = @SPECIALREMARKS,
		[DBO].[Appointment].[Created] = @CREATED,
		[DBO].[Appointment].[Modified] = @MODIFIED,
		[DBO].[Appointment].[UserID] = @USERID,
		[DBO].[Appointment].[TotalConsultedAmount] = @TOTALCONSULTEDAMOUNT

	WHERE [DBO].[Appointment].[AppointmentID] = @APPOINMENTID
END



--APPOINMENT TABLE DELETE BY PK PROCEDURE
CREATE OR ALTER PROCEDURE [DBO].[PR_Appointment_DeleteByPK]

@APPOINMENTID			INT

AS
BEGIN
	DELETE
	FROM [DBO].[Appointment]
	WHERE [DBO].[Appointment].[AppointmentID] = @APPOINMENTID
END


