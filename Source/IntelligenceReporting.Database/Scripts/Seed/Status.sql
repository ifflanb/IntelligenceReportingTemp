MERGE INTO dbo.Status AS TARGET
USING (VALUES
    (1 , 'Prospect / Not currently listed'),
    (2 , 'Appraisal'),
    (3 , 'Listing'),
    (4 , 'Conditional'),
    (5 , 'Unconditional'),
    (6 , 'Settled'),
    (7 , 'Management listing'),
    (8 , 'Withdrawn listing'),
    (9 , 'Withdrawn appraisal'),
    (10, 'Fallen sale')
)
AS SOURCE (Id, StatusName)
ON TARGET.Id = SOURCE.Id
WHEN MATCHED THEN UPDATE SET
    TARGET.StatusName = SOURCE.StatusName
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, StatusName)
    VALUES (Id, StatusName)
WHEN NOT MATCHED BY SOURCE THEN DELETE
;
