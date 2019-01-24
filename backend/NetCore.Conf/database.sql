CREATE TABLE [dbo].[Todos]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Created] DATETIME NOT NULL, 
    [Text] NVARCHAR(MAX) NOT NULL, 
    [Done] BIT NOT NULL
)

CREATE TABLE [dbo].[Votes] (
    [QuestionId] INT            NOT NULL,
    [Username]   NVARCHAR (512) NOT NULL,
    [Usefull]    INT            NOT NULL,
    [Crazy]      INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Username] ASC, [QuestionId] ASC)
);
