CREATE TRIGGER [CommentVotesCountingDeleteTrigger]
	ON [dbo].[CommentVotes]
	FOR DELETE
	AS
	BEGIN
		UPDATE Comments 
		SET UpVotesCount = UpVotesCount - deleted.Positive,
		DownVotesCount = DownVotesCount - CASE WHEN deleted.Positive = 0 THEN 1 ELSE 0 END
		FROM Comments 
		JOIN deleted ON deleted.Votable_Id = Comments.Id
	END
