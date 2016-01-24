CREATE TRIGGER [TriggerDel]
	ON [dbo].[ItemRates]
	FOR DELETE
	AS
	BEGIN
		UPDATE Items 
		SET UpScore = UpScore - deleted.Positive,
		DownScore = DownScore - CASE WHEN deleted.Positive = 0 THEN 1 ELSE 0 END
		FROM Items 
		JOIN deleted ON deleted.Item_Id = Items.Id
	END
