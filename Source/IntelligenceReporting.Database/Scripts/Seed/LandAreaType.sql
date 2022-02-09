MERGE INTO dbo.LandAreaType AS TARGET
USING (VALUES
    (1, 'Square meters'),
    (2, 'Acre'),
    (3, 'Hectare')
)
AS SOURCE (Id, LandAreaTypeName)
ON TARGET.Id = SOURCE.Id
WHEN MATCHED THEN UPDATE SET
    TARGET.LandAreaTypeName = SOURCE.LandAreaTypeName
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, LandAreaTypeName)
    VALUES (Id, LandAreaTypeName)
WHEN NOT MATCHED BY SOURCE THEN DELETE
;
