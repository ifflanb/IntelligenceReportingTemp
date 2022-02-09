CREATE TABLE dbo.Staff
(
    Id INT NOT NULL CONSTRAINT PK_Staff PRIMARY KEY IDENTITY (1, 1),
    SourceId INT NOT NULL,
        CONSTRAINT FK_Staff_SourceId FOREIGN KEY (SourceId) REFERENCES dbo.Source (Id),
    ExternalId INT NOT NULL,
    LastUpdated DATETIMEOFFSET NOT NULL,
    OfficeId INT NOT NULL,
        CONSTRAINT FK_Staff_Office FOREIGN KEY (OfficeId) REFERENCES dbo.Office (Id),
    StaffName NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT(0),
)
GO

CREATE UNIQUE INDEX UX_Staff_Source ON dbo.Staff (SourceId, ExternalId)
GO