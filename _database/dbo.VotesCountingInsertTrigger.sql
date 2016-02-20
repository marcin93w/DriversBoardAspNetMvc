CREATE TRIGGER [VotesCountingInsertTrigger]
	ON [dbo].[ItemVotes]
	FOR INSERT
	AS
	BEGIN
		UPDATE Items 
		SET UpVotesCount = UpVotesCount + inserted.Positive,
		DownVotesCount = DownVotesCount + CASE WHEN inserted.Positive = 0 THEN 1 ELSE 0 END
		FROM Items JOIN inserted ON inserted.Votable_Id = Items.Id
		WHERE Items.Id = inserted.Votable_Id
	END
