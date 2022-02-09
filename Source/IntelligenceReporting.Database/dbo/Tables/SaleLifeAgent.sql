CREATE TABLE dbo.SaleLifeAgent
(
    Id INT NOT NULL CONSTRAINT PK_SaleLifeAgent PRIMARY KEY NONCLUSTERED IDENTITY (1, 1),
    OfficeId INT NOT NULL,
        CONSTRAINT FK_SaleLifeAgent_Office FOREIGN KEY (OfficeId) REFERENCES dbo.Office (Id),
    SaleLifeId INT NOT NULL,
        CONSTRAINT FK_SaleLifeAgent_SaleLife FOREIGN KEY (SaleLifeId) REFERENCES dbo.SaleLife (Id),
    StaffId INT NOT NULL,
        CONSTRAINT FK_SaleLifeAgent_Staff FOREIGN KEY (StaffId) REFERENCES dbo.Staff (Id),
    IsPrimary BIT NOT NULL,

    CONSTRAINT CX_SaleLifeAgent UNIQUE CLUSTERED (OfficeId, StaffId, SaleLifeId, Id),
)
GO
