CREATE TABLE dbo.SaleLifeEarnings
(
    Id INT NOT NULL CONSTRAINT PK_SaleLifeEarnings PRIMARY KEY NONCLUSTERED IDENTITY (1, 1),
    OfficeId INT NOT NULL,
        CONSTRAINT FK_SaleLifeEarnings_Office FOREIGN KEY (OfficeId) REFERENCES dbo.Office (Id),
    SaleLifeId INT NOT NULL,
        CONSTRAINT FK_SaleLifeEarnings_SaleLife FOREIGN KEY (SaleLifeId) REFERENCES dbo.SaleLife (Id),
    StaffId INT NOT NULL,
        CONSTRAINT FK_SaleLifeEarnings_Staff FOREIGN KEY (StaffId) REFERENCES dbo.Staff (Id),
    RoleId INT NOT NULL,
        CONSTRAINT FK_SaleLifeEarnings_Role FOREIGN KEY (RoleId) REFERENCES dbo.Role (Id),
    SplitPercent DECIMAL(18,15) NOT NULL,
    GrossSplit DECIMAL(18,2) NOT NULL,
    NetSplit DECIMAL(18,2) NOT NULL,

    CONSTRAINT CX_SaleLifeEarnings UNIQUE CLUSTERED (OfficeId, StaffId, SaleLifeId, Id),
)
GO
