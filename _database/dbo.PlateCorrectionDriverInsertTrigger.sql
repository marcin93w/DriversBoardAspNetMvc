CREATE TRIGGER [PlateCorrectionDriverInsertTrigger]
	ON [dbo].[Drivers]
	FOR INSERT
	AS
	BEGIN
		UPDATE Drivers 
		SET Plate = REPLACE(inserted.Plate, ' ', '')
		FROM inserted
		WHERE Drivers.Id = inserted.Id
	END
