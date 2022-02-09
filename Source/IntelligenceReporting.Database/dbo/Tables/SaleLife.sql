CREATE TABLE dbo.SaleLife
(
    Id INT NOT NULL CONSTRAINT PK_SaleLife PRIMARY KEY NONCLUSTERED IDENTITY (1, 1),
    SourceId INT NOT NULL,
        CONSTRAINT FK_SaleLife_SourceId FOREIGN KEY (SourceId) REFERENCES dbo.Source (Id),
    ExternalId INT NOT NULL,
    OfficeId INT NOT NULL,
        CONSTRAINT FK_SaleLife_Office FOREIGN KEY (OfficeId) REFERENCES dbo.Office (Id),
    Address NVARCHAR(100) NOT NULL,
    LastUpdated DATETIMEOFFSET NOT NULL,
    ReportMethodOfSaleId INT NOT NULL,
        CONSTRAINT FK_SaleLife_ReportMethodOfSaleId FOREIGN KEY (ReportMethodOfSaleId) REFERENCES dbo.ReportMethodOfSale (Id),
    LandAreaTypeId INT NULL,
        CONSTRAINT FK_SaleLife_LandAreaTypeId FOREIGN KEY (LandAreaTypeId) REFERENCES dbo.LandAreaType (Id),
    LandArea DECIMAL(18, 6) NULL,
    StatusId INT NOT NULL,
        CONSTRAINT FK_SaleLife_StatusId FOREIGN KEY (StatusId) REFERENCES dbo.Status (Id),
    IsUnitTitle BIT NOT NULL DEFAULT(0),
    AppraisalDate DATE NULL,
    ListingDate DATE NULL,
    AuctionDate DATE NULL,
    ConditionalDate DATE NULL,
    UnconditionalDate DATE NULL,
    SettlementDate DATE NULL,
    SettlementActioned DATETIMEOFFSET NULL,
    AdminComplete DATE NULL,
    AdminCompleteActioned DATETIMEOFFSET NULL,
    GrossCommission DECIMAL(18, 2) NULL,
    FoundationContribution DECIMAL(18, 2) NULL,
    OfficeGrossIncome DECIMAL(18, 2) NULL,
    [ListingTypeId] INT NOT NULL, 
    [PropertyClassId] INT NOT NULL, 
        CONSTRAINT FK_SaleLife_PropertyClassId FOREIGN KEY (PropertyClassId) REFERENCES dbo.PropertyClass (Id),

    CONSTRAINT CX_SaleLife UNIQUE CLUSTERED (OfficeId, Id),
)
GO

CREATE UNIQUE INDEX UX_SaleLife_Source ON dbo.SaleLife (SourceId, ExternalId)
GO

