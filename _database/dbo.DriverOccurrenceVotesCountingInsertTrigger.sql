CREATE TRIGGER [DriverOccurrenceVotesCountingInsertTrigger]
	ON [dbo].[DriverOccurrenceVotes]
	FOR INSERT
	AS
	BEGIN
		UPDATE DriverOccurrences 
		SET UpVotesCount = UpVotesCount + inserted.Positive,
		DownVotesCount = DownVotesCount + CASE WHEN inserted.Positive = 0 THEN 1 ELSE 0 END
		FROM DriverOccurrences JOIN inserted ON inserted.Votable_Id = DriverOccurrences.Id
		WHERE DriverOccurrences.Id = inserted.Votable_Id;
        
		UPDATE Drivers 
		SET UpVotesCount = Drivers.UpVotesCount + inserted.Positive,
		DownVotesCount = Drivers.DownVotesCount + CASE WHEN inserted.Positive = 0 THEN 1 ELSE 0 END
		FROM Drivers
		JOIN DriverOccurrences ON Drivers.Id = DriverOccurrences.Driver_Id
		JOIN inserted ON inserted.Votable_Id = DriverOccurrences.Id
		WHERE DriverOccurrences.Id = inserted.Votable_Id;
	END