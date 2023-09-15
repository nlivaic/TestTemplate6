USE master
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TestTemplate6Db')
BEGIN
  CREATE DATABASE TestTemplate6Db;
END;
GO

USE TestTemplate6Db;
GO

IF NOT EXISTS (SELECT 1
                 FROM sys.server_principals
                WHERE [name] = N'TestTemplate6Db_Login' 
                  AND [type] IN ('C','E', 'G', 'K', 'S', 'U'))
BEGIN
    CREATE LOGIN TestTemplate6Db_Login
        WITH PASSWORD = '<DB_PASSWORD>';
END;
GO  

IF NOT EXISTS (select * from sys.database_principals where name = 'TestTemplate6Db_User')
BEGIN
    CREATE USER TestTemplate6Db_User FOR LOGIN TestTemplate6Db_Login;
END;
GO  


EXEC sp_addrolemember N'db_datareader', N'TestTemplate6Db_User';
GO

EXEC sp_addrolemember N'db_datawriter', N'TestTemplate6Db_User';
GO

EXEC sp_addrolemember N'db_ddladmin', N'TestTemplate6Db_User';
GO
