CREATE DATABASE PersonnelSalesManagementDb;

USE PersonnelSalesManagementDb;

CREATE TABLE Personnels (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(50) NOT NULL,
	Age INT NOT NULL,
	Phone VARCHAR(50) NOT NULL
);

CREATE TABLE Sales (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Report_Date DATE NOT NULL,
	Sales_Amount DECIMAL(10,2) NOT NULL,
	Personnel_Id INT NOT NULL,
	CONSTRAINT FK_Sales_Personnels FOREIGN KEY (Personnel_Id)
		REFERENCES Personnels(Id)
		ON DELETE CASCADE
);

INSERT INTO Personnels (Name, Age, Phone) VALUES
('John Wick', 25, '01145678970'),
('Thomas Edison', 22, '01433267789'),
('Mark Xabec',19,'0189091126'),
('Ali Tyson', 24, '01699213403'),
('Lee Ming', 21, '01277972190');

INSERT INTO Sales (Report_Date, Sales_Amount, Personnel_Id) VALUES
('2026-04-02',120.00, 1),
('2026-04-03',130.00, 1),
('2026-04-06',110.00, 1),
('2026-04-07',140.00, 1),
('2026-04-08',170.00, 1),
('2026-04-02',200.00, 2),
('2026-04-03',180.00, 2),
('2026-04-06',170.00, 2),
('2026-04-07',190.00, 2),
('2026-04-08',200.00, 2),
('2026-04-03',130.00, 3),
('2026-04-06',125.00, 3),
('2026-04-07',140.00, 3),
('2026-04-08',167.00, 3),
('2026-04-09',118.00, 3),
('2026-04-01',189.00, 4),
('2026-04-02',163.00, 4),
('2026-04-03',177.00, 4),
('2026-04-06',185.00, 4),
('2026-04-09',190.00, 4),
('2026-04-02',120.00, 5),
('2026-04-03',120.00, 5),
('2026-04-06',130.00, 5),
('2026-04-08',170.00, 5),
('2026-04-09',150.00, 5);