DECLARE @StartNum int = 1
DECLARE @EndNum int = 10000

DROP TABLE IF EXISTS #Numbers

;WITH [Range] AS (
     SELECT @StartNum AS [Number]
     UNION ALL
     SELECT [Number] = [Number] + 1 FROM [Range] WHERE [Number] < @EndNum
     )

SELECT [Number]
INTO #Numbers
FROM [Range]
OPTION (MAXRECURSION 10000)

DROP TABLE IF EXISTS IntTable
CREATE TABLE IntTable ([Id] INT PRIMARY KEY IDENTITY (1, 1), [Value] int NOT NULL)

    INSERT INTO IntTable([Value])
SELECT [Number] FROM #Numbers
ORDER BY [Number]

DROP TABLE IF EXISTS StringTable
CREATE TABLE StringTable ([Id] INT PRIMARY KEY IDENTITY (1, 1), [Value] nvarchar(200) NOT NULL)

INSERT INTO StringTable([Value])
SELECT CAST([Number] AS nvarchar(200)) FROM #Numbers
ORDER BY [Number]

DROP TABLE IF EXISTS BigIntTable
CREATE TABLE BigIntTable ([Id] INT PRIMARY KEY IDENTITY (1, 1), [Value] bigint NOT NULL)

INSERT INTO BigIntTable([Value])
SELECT CAST([Number] AS bigint) FROM #Numbers
ORDER BY [Number]

DROP TABLE IF EXISTS GuidTable
CREATE TABLE GuidTable ([Id] INT PRIMARY KEY IDENTITY (1, 1), [Value] uniqueidentifier NOT NULL)

INSERT INTO GuidTable([Value])
SELECT CONVERT(UNIQUEIDENTIFIER, CONVERT(VARBINARY(16), [Number], 1)) FROM #Numbers
ORDER BY [Number]