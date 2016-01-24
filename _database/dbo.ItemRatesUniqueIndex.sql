CREATE UNIQUE NONCLUSTERED INDEX [IX_ItemRates_Column]
    ON [dbo].[ItemRates]([Item_Id] ASC, [User_Id] ASC);