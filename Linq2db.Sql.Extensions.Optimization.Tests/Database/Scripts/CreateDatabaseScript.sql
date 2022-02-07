IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Linq2dbTestsDatabase')
BEGIN
    CREATE DATABASE [Linq2dbTestsDatabase]
END