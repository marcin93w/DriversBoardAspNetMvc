CREATE TRIGGER [DriverOccurrenceVotesCountingUpdateTrigger]
	ON [dbo].[DriverOccurrenceVotes]
	FOR UPDATE
	AS
	BEGIN
		UPDATE DriverOccurrences 
		SET UpVotesCount = UpVotesCount + inserted.Positive - deleted.Positive,
		DownVotesCount = DownVotesCount + CASE WHEN inserted.Positive = 0 THEN 1 ELSE 0 END - CASE WHEN deleted.Positive = 0 THEN 1 ELSE 0 END
		FROM DriverOccurrences 
		JOIN inserted ON inserted.Votable_Id = DriverOccurrences.Id
		JOIN deleted ON deleted.Votable_Id = DriverOccurrences.Id
	END
