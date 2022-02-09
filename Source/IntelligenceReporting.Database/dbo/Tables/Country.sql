CREATE TABLE dbo.Country
(
    Id INT NOT NULL CONSTRAINT PK_Country PRIMARY KEY IDENTITY (1, 1),
    SourceId INT NOT NULL,
        CONSTRAINT FK_Country_SourceId FOREIGN KEY (SourceId) REFERENCES dbo.Source (Id),
    ExternalId INT NOT NULL,
    CountryName NVARCHAR(100) NOT NULL,
    IsoCountryCode CHAR(2) NOT NULL
)
GO

CREATE UNIQUE INDEX UX_CountrySource ON dbo.Country (SourceId, ExternalId)
GO
