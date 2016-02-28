CREATE TRIGGER [DriverOccurrenceVotesCountingDeleteTrigger]
	ON [dbo].[DriverOccurrenceVotes]
	FOR DELETE
	AS
	BEGIN
		UPDATE DriverOccurrences 
		SET UpVotesCount = UpVotesCount - deleted.Positive,
		DownVotesCount = DownVotesCount - CASE WHEN deleted.Positive = 0 THEN 1 ELSE 0 END
		FROM DriverOccurrences 
		JOIN deleted ON deleted.Votable_Id = DriverOccurrences.Id
	END
