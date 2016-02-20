CREATE TRIGGER [CommentVotesCountingInsertTrigger]
	ON [dbo].[CommentVotes]
	FOR INSERT
	AS
	BEGIN
		UPDATE Comments 
		SET UpVotesCount = UpVotesCount + inserted.Positive,
		DownVotesCount = DownVotesCount + CASE WHEN inserted.Positive = 0 THEN 1 ELSE 0 END
		FROM Comments JOIN inserted ON inserted.Votable_Id = Comments.Id
		WHERE Comments.Id = inserted.Votable_Id
	END
