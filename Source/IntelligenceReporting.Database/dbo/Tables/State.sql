CREATE TABLE dbo.State
(
    Id INT NOT NULL CONSTRAINT PK_State PRIMARY KEY IDENTITY (1, 1),
    SourceId INT NOT NULL,
        CONSTRAINT FK_State_SourceId FOREIGN KEY (SourceId) REFERENCES dbo.Source (Id),
    ExternalId INT NOT NULL,
    StateAbbreviation NVARCHAR(10) NOT NULL,
    StateName NVARCHAR(100) NOT NULL,
    CountryId INT NOT NULL,
        CONSTRAINT FK_State_Country FOREIGN KEY (CountryId) REFERENCES dbo.Country (Id),
    TimeZoneName NVARCHAR(50) NOT NULL
)
GO

CREATE UNIQUE INDEX UX_State_Source ON dbo.State (SourceId, ExternalId)
GO