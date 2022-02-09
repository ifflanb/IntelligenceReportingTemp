CREATE TABLE dbo.Brand
(
    Id INT NOT NULL CONSTRAINT PK_Brand PRIMARY KEY IDENTITY (1, 1),
    SourceId INT NOT NULL,
        CONSTRAINT FK_BrandSourceId FOREIGN KEY (SourceId) REFERENCES dbo.Source (Id),
    ExternalId INT NOT NULL,
    BrandName NVARCHAR(100) NOT NULL
)
GO

CREATE UNIQUE INDEX UX_BrandSource ON dbo.Brand (SourceId, ExternalId)
GO
