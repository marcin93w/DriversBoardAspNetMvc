CREATE TRIGGER [Trigger]
	ON [dbo].[ItemRates]
	FOR UPDATE
	AS
	BEGIN
		UPDATE Items 
		SET UpScore = UpScore + inserted.Positive - deleted.Positive,
		DownScore = DownScore + CASE WHEN inserted.Positive = 0 THEN 1 ELSE 0 END - CASE WHEN deleted.Positive = 0 THEN 1 ELSE 0 END
		FROM Items 
		JOIN inserted ON inserted.Item_Id = Items.Id
		JOIN deleted ON deleted.Item_Id = Items.Id
	END
