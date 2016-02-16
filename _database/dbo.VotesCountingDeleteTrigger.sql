CREATE TRIGGER [VotesCountingDeleteTrigger]
	ON [dbo].[ItemVotes]
	FOR DELETE
	AS
	BEGIN
		UPDATE Items 
		SET UpVotesCount = UpVotesCount - deleted.Positive,
		DownVotesCount = DownVotesCount - CASE WHEN deleted.Positive = 0 THEN 1 ELSE 0 END
		FROM Items 
		JOIN deleted ON deleted.Item_Id = Items.Id
	END
