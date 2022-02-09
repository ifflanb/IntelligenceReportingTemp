CREATE TABLE dbo.Office
(
    Id INT NOT NULL CONSTRAINT PK_Office PRIMARY KEY IDENTITY (1, 1),
    SourceId INT NOT NULL,
        CONSTRAINT FK_Office_SourceId FOREIGN KEY (SourceId) REFERENCES dbo.Source (Id),
    ExternalId INT NOT NULL,
    LastUpdated DATETIMEOFFSET NOT NULL,
    OfficeName NVARCHAR(100) NOT NULL,
    BrandId INT NULL,
        CONSTRAINT FK_Office_Brand FOREIGN KEY (BrandId) REFERENCES dbo.Brand (Id),
    StateId INT NULL,
        CONSTRAINT FK_Office_State FOREIGN KEY (StateId) REFERENCES dbo.State (Id),
    MultiOfficeId INT NULL,
        CONSTRAINT FK_Office_MultiOffice FOREIGN KEY (MultiOfficeId) REFERENCES dbo.MultiOffice (Id),
	IsActive BIT NOT NULL DEFAULT(0),
)
GO

CREATE UNIQUE INDEX UX_OfficeSource ON dbo.Office (SourceId, ExternalId)
GO
