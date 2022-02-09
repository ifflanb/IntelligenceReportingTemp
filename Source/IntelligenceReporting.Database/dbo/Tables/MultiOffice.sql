CREATE TABLE dbo.MultiOffice
(
    Id INT NOT NULL CONSTRAINT PK_MultiOffice PRIMARY KEY IDENTITY (1, 1),
    SourceId INT NOT NULL,
        CONSTRAINT FK_MultiOfficeSourceId FOREIGN KEY (SourceId) REFERENCES dbo.Source (Id),
    ExternalId INT NOT NULL,
    MultiOfficeName NVARCHAR(100) NOT NULL
)
GO

CREATE UNIQUE INDEX UX_MultiOfficeSource ON dbo.MultiOffice (SourceId, ExternalId)
GO
