MERGE INTO dbo.Role AS TARGET
USING (VALUES
    (1 , 'Lister'),
    (2 , 'Seller'),
    (7 , 'Referrer'),
    (8 , 'Other')
)
AS SOURCE (Id, RoleName)
ON TARGET.Id = SOURCE.Id
WHEN MATCHED THEN UPDATE SET
    TARGET.RoleName = SOURCE.RoleName
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, RoleName)
    VALUES (Id, RoleName)
WHEN NOT MATCHED BY SOURCE THEN DELETE
;
