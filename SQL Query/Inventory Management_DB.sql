-- =========================================================================================================
-- Author: Hanif Azhar
-- Create date: 21 September 2022
-- Description: Inventory Management DB for Internal Use Inside Company

-- Note:
-- Auto increment with "IDENTITY(1,1)"
-- Get current time with "datetime NOT NULL DEFAULT GETDATE()" because timestamp output is 0x0...
-- =========================================================================================================

----------------
--Create Table--
----------------
--Items
CREATE TABLE Items(
	id int IDENTITY(1,1) NOT NULL,
	code varchar(20) NOT NULL,
	name varchar(255) NOT NULL,
	available_quantity decimal(10,2) NOT NULL DEFAULT 0,
	notes text NULL,
	CONSTRAINT PK_Items PRIMARY KEY(id),
	CONSTRAINT U_Code_Items UNIQUE(code)
);

--TransactionTypes
-- Item Inbound(1/ITEM_IN), Item Outbound(2/ITEM_OUT)
CREATE TABLE TransactionTypes(
	id int IDENTITY(1,1) NOT NULL,
	code varchar(20) NOT NULL,
	name varchar(100) NOT NULL,
	CONSTRAINT PK_TransactionTypes PRIMARY KEY(id),
	CONSTRAINT U_Code_TransactionTypes UNIQUE(code)
);

--RoleTypes
-- Admin(1/ADMIN), Purchasing Admin(2/PURCHAS_ADMIN), Department Admin(3/DEPART_ADMIN)
CREATE TABLE RoleTypes(
	id int IDENTITY(1,1) NOT NULL,
	code varchar(20) NOT NULL,
	name varchar(100) NOT NULL,
	CONSTRAINT PK_RoleTypes PRIMARY KEY(id),
	CONSTRAINT U_Code_RoleTypes UNIQUE(code)
);

--Users
CREATE TABLE Users(
	id int IDENTITY(1,1) NOT NULL,
	role_id int NOT NULL DEFAULT 3,
	first_name varchar(50) NOT NULL,
	last_name varchar(50) NULL,
	username varchar(50) NOT NULL,
	email varchar(50) NOT NULL,
	phone_number varchar(15) NOT NULL,
	password_hash binary(64) NOT NULL,
	password_salt uniqueidentifier NOT NULL,
	registered_at datetime NOT NULL DEFAULT GETDATE(),
	CONSTRAINT PK_Users PRIMARY KEY(id),
	CONSTRAINT FK_Users_RoleTypes_Id FOREIGN KEY(role_id) REFERENCES RoleTypes(id),
	CONSTRAINT U_Username_Users UNIQUE(username),
	CONSTRAINT U_Email_Users UNIQUE(email),
	CONSTRAINT U_Mobile_Users UNIQUE(phone_number)
);

--TransactionDetails
CREATE TABLE TransactionDetails(
	id int IDENTITY(1,1) NOT NULL,
	transaction_type_id int NOT NULL,
	item_id int NOT NULL,
	user_id int NOT NULL,
	code varchar(100) NOT NULL,
	quantity decimal(10,2) NOT NULL DEFAULT 0,
	notes text NULL,
	created_at datetime NOT NULL DEFAULT GETDATE(),
	CONSTRAINT PK_TransactionDetails PRIMARY KEY(id),
	CONSTRAINT FK_TransactionDetails_TransactionTypes_Id FOREIGN KEY(transaction_type_id) REFERENCES TransactionTypes(id),
	CONSTRAINT FK_TransactionDetails_Items_Id FOREIGN KEY(item_id) REFERENCES Items(id),
	CONSTRAINT FK_TransactionDetails_Users_Id FOREIGN KEY(user_id) REFERENCES Users(id),
	CONSTRAINT U_Code_TransactionDetails UNIQUE(code)
);

-----------------------
--Initial Insert Data--
-----------------------
-- Item Inbound(1/ITEM_IN), Item Outbound(2/ITEM_OUT)
INSERT INTO
	TransactionTypes(code, name)
VALUES
	('ITEM_IN', 'Item Inbound'),
	('ITEM_OUT', 'Item Outbound');

-- Admin(1/ADMIN), Purchasing Admin(2/PURCHAS_ADMIN), Department Admin(3/DEPART_ADMIN)
INSERT INTO
	RoleTypes(code, name)
VALUES
	('ADMIN', 'Admin'),
	('PURCHAS_ADMIN', 'Purchasing Admin'),
	('DEPART_ADMIN', 'Department Admin');

-- Add items
INSERT INTO
	Items(code, name, available_quantity, notes)
VALUES
	('PAPER_1', 'Paper', 10000, 'A paper'),
	('BOOK_1', 'Book', 20000, 'A book'),
	('PEN_1', 'Pen', 30000, 'A pen');

-- Register User
DECLARE @responseMessage varchar(250)

--User Admin
EXEC spRegister
	@roleId = 1,
	@firstName = 'Admin',
	@lastName = NULL,
	@username = 'admin',
	@email = 'admin.inventory@abc.xyz',
	@phoneNumber = '081234567891',
	@passwordPlainText = 'admin_password',
	@responseMessage = @responseMessage OUTPUT

--User Admin Purchasing
EXEC spRegister
	@roleId = 2,
	@firstName = 'Admin',
	@lastName = 'Purchasing',
	@username = 'admin.purchasing',
	@email = 'admin.purchasing@abc.xyz',
	@phoneNumber = '082234567891',
	@passwordPlainText = 'admin_purchasing_password',
	@responseMessage = @responseMessage OUTPUT

--User Admin HRD
EXEC spRegister
	@roleId = 3,
	@firstName = 'Admin',
	@lastName = 'HRD',
	@username = 'admin.hrd',
	@email = 'admin.hrd@abc.xyz',
	@phoneNumber = '083234567891',
	@passwordPlainText = 'admin_hrd_password',
	@responseMessage = @responseMessage OUTPUT