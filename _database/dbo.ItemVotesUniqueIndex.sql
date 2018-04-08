CREATE UNIQUE NONCLUSTERED INDEX [IX_ItemVotes_Column]
    ON [dbo].[ItemVotes]([Votable_Id] ASC, [User_Id] ASC);