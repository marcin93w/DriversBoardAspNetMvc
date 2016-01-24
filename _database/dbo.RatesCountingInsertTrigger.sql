CREATE TRIGGER [RatesCountingTrigger]
	ON [dbo].[ItemRates]
	FOR INSERT
	AS
	BEGIN
		UPDATE Items 
		SET UpScore = UpScore + inserted.
		DownScore = DownScore + 1
		FROM Items JOIN inserted ON inserted.Item_Id = Items.Id
		WHERE Items.Id = inserted.Item.Id
	END
