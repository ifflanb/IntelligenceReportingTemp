MERGE INTO dbo.ReportMethodOfSale AS TARGET
USING (VALUES
    (1 , 'Auction'),
    (2 , 'Tender'),
    (3 , 'Exclusive'),
    (4 , 'General')
)
AS SOURCE (Id, ReportMethodOfSaleName)
ON TARGET.Id = SOURCE.Id
WHEN MATCHED THEN UPDATE SET
    TARGET.ReportMethodOfSaleName = SOURCE.ReportMethodOfSaleName
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, ReportMethodOfSaleName)
    VALUES (Id, ReportMethodOfSaleName)
WHEN NOT MATCHED BY SOURCE THEN DELETE
;
