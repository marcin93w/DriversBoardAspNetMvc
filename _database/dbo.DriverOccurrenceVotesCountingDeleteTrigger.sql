CREATE TRIGGER [DriverOccurrenceVotesCountingDeleteTrigger]
	ON [dbo].[DriverOccurrenceVotes]
	FOR DELETE
	AS
	BEGIN
		UPDATE DriverOccurrences 
		SET UpVotesCount = UpVotesCount - deleted.Positive,
		DownVotesCount = DownVotesCount - CASE WHEN deleted.Positive = 0 THEN 1 ELSE 0 END
		FROM DriverOccurrences 
		JOIN deleted ON deleted.Votable_Id = DriverOccurrences.Id;
        
		UPDATE Drivers
		SET UpVotesCount = Drivers.UpVotesCount - deleted.Positive,
		DownVotesCount = Drivers.DownVotesCount - CASE WHEN deleted.Positive = 0 THEN 1 ELSE 0 END
		FROM Drivers
		JOIN DriverOccurrences ON Drivers.Id = DriverOccurrences.Driver_Id
		JOIN deleted ON deleted.Votable_Id = DriverOccurrences.Id
		WHERE DriverOccurrences.Id = deleted.Votable_Id;
	END