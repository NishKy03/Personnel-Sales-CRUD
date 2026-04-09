CREATE LOGIN [IIS AppPool\PersonnelSalesManagement] FROM WINDOWS;

USE PersonnelSalesManagementDb;

CREATE USER [IIS AppPool\PersonnelSalesManagement] FOR LOGIN [IIS AppPool\PersonnelSalesManagement];

ALTER ROLE db_datareader ADD MEMBER [IIS AppPool\PersonnelSalesManagement];

ALTER ROLE db_datawriter ADD MEMBER [IIS AppPool\PersonnelSalesManagement];