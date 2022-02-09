MERGE INTO dbo.PropertyClass AS TARGET
USING (VALUES
    (1, 'Residential'),
    (2, 'Commercial'),
    (3, 'Business'),
    (4, 'Rural'),
    (5, 'HolidayRental'),
    (6, 'Land'),
    (7, 'Livestock'),
    (8, 'ClearingSales')
)
AS SOURCE (Id, PropertyClassName)
ON TARGET.Id = SOURCE.Id
WHEN MATCHED THEN UPDATE SET
    TARGET.PropertyClassName = SOURCE.PropertyClassName
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, PropertyClassName)
    VALUES (Id, PropertyClassName)
WHEN NOT MATCHED BY SOURCE THEN DELETE
;