﻿CREATE TRIGGER [VotesCountingUpdateTrigger]
	ON [dbo].[ItemVotes]
	FOR UPDATE
	AS
	BEGIN
		UPDATE Items 
		SET UpVotesCount = UpVotesCount + inserted.Positive - deleted.Positive,
		DownVotesCount = DownVotesCount + CASE WHEN inserted.Positive = 0 THEN 1 ELSE 0 END - CASE WHEN deleted.Positive = 0 THEN 1 ELSE 0 END
		FROM Items 
		JOIN inserted ON inserted.Votable_Id = Items.Id
		JOIN deleted ON deleted.Votable_Id = Items.Id
	END
