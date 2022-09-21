-- =============================================
-- Author:		<Hanif Azhar>
-- Create date: <21 September 2022>
-- Description:	<Procedures>
-- =============================================

--Register Procedure
CREATE PROCEDURE spRegister
	@roleId int,
	@firstName varchar(50),
	@lastName varchar(50),
	@username varchar(50),
	@email varchar(50),
	@phoneNumber varchar(15),
	@passwordPlainText varchar(50),
	@responseMessage varchar(250) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @passwordSalt uniqueidentifier = NEWID()

	BEGIN TRY
		INSERT INTO
			Users (role_id, first_name, last_name, username, email, phone_number, password_hash, password_salt)
		VALUES
			(@roleId, @firstName, @lastName, @username, @email, @phoneNumber, HASHBYTES('SHA2_512', @passwordPlainText + CAST(@passwordSalt AS varchar(36))), @passwordSalt)
		
		SET @responseMessage = 'Successfully register'
	END TRY
	BEGIN CATCH
		SET @responseMessage = ERROR_MESSAGE() 
	END CATCH
END
GO

--Login Procedure
CREATE PROCEDURE spLogin
	@username varchar(50),
	@email varchar(50),
	@passwordPlainText varchar(50),
	@responseMessage varchar(250) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @userId int

	IF EXISTS (SELECT TOP 1 id FROM Users WHERE username = @username OR email = @email)
	BEGIN
		SET @userId = (SELECT id FROM Users WHERE (username = @username OR email = @email) AND password_hash = HASHBYTES('SHA2_512', @passwordPlainText + CAST(password_salt AS varchar(36))))

		IF (@userId IS NULL)
			SET @responseMessage = 'Incorrect password'
		ELSE
			SET @responseMessage = 'User successfully logged in'
	END
	ELSE
		SET @responseMessage = 'Invalid login'
END
GO

--Check User by Username or Email Procedure
CREATE PROCEDURE spCheckUserByUsernameEmail
	@username varchar(50),
	@email varchar(50),
	@responseMessage varchar(250) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @userId int

	IF EXISTS (SELECT TOP 1 id FROM Users WHERE username = @username OR email = @email)
		SET @responseMessage = 'Success'
	ELSE
		SET @responseMessage = 'Invalid username or email'
END
GO

--Change User Password Procedure
CREATE PROCEDURE spChangeUserPassword
	@username varchar(50),
	@email varchar(50),
	@passwordPlainText varchar(50),
	@responseMessage varchar(250) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @userId int
	DECLARE @passwordSalt uniqueidentifier = NEWID()

	SET @userId = (SELECT id FROM Users WHERE (username = @username OR email = @email) AND password_hash = HASHBYTES('SHA2_512', @passwordPlainText + CAST(password_salt AS varchar(36))))

	IF (@userId IS NULL)
		SET @responseMessage = 'Incorrect password'
	ELSE
		UPDATE
			Users
		SET
			password_salt = @passwordSalt,
			password_hash = HASHBYTES('SHA2_512', @passwordPlainText + CAST(@passwordSalt AS varchar(36)))
		WHERE
			username = @username OR email = @email

		SET @responseMessage = 'Successfully changed password'
END
GO

--Item Transaction Procedure
CREATE PROCEDURE spItemTransaction
	@transactionTypeId int,
	@itemId int,
	@userId int,
	@quantity decimal(10,2),
	@notes text,
	@responseMessage varchar(250) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @availableQuantity decimal(10,2)

	BEGIN TRY
		SET @availableQuantity = (SELECT available_quantity FROM Items WHERE id = @itemId)
		SET @availableQuantity = @availableQuantity - @quantity

		IF (@availableQuantity <= 0)
			SET @responseMessage = 'Transaction error! Quantity in table Items <= 0'
		ELSE
			--Insert into TransactionDetails
			INSERT INTO
				TransactionDetails(transaction_type_id, item_id, user_id, code, quantity, notes)
			VALUES
				(@transactionTypeId, @itemId, @userId, 'TD_[' +
				(SELECT code FROM TransactionTypes WHERE id = @transactionTypeId) + ']_[' +
				(SELECT code FROM Items WHERE id = @itemId) + ']_[' +
				CONVERT(varchar, GETDATE(), 120) + ']', @quantity, @notes);

			--Update available quantity in Items
			UPDATE Items SET available_quantity = @availableQuantity WHERE id = @itemId
			
			SET @responseMessage = 'Transaction success'
	END TRY
	BEGIN CATCH
		SET @responseMessage = ERROR_MESSAGE() 
	END CATCH
END
GO