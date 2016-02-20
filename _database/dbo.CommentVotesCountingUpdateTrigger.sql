CREATE TRIGGER [CommentVotesCountingUpdateTrigger]
	ON [dbo].[CommentVotes]
	FOR UPDATE
	AS
	BEGIN
		UPDATE Comments 
		SET UpVotesCount = UpVotesCount + inserted.Positive - deleted.Positive,
		DownVotesCount = DownVotesCount + CASE WHEN inserted.Positive = 0 THEN 1 ELSE 0 END - CASE WHEN deleted.Positive = 0 THEN 1 ELSE 0 END
		FROM Comments 
		JOIN inserted ON inserted.Votable_Id = Comments.Id
		JOIN deleted ON deleted.Votable_Id = Comments.Id
	END
