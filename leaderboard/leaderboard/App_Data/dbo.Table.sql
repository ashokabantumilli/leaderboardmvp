CREATE TABLE [dbo].[LeaderBoardTable]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [CompeteName] NVARCHAR(50) NOT NULL, 
    [H2HContestName] NVARCHAR(50) NOT NULL, 
    [Points] INT NOT NULL, 
    [Referee] NVARCHAR(50) NOT NULL
)
