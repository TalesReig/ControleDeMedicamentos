CREATE TABLE [dbo].[TBFuncionario] (
    [Id]    INT           IDENTITY (1, 1) NOT NULL,
    [Nome]  VARCHAR (300) NOT NULL,
    [Login] VARCHAR (100) NOT NULL,
    [Senha] VARCHAR (20)  NOT NULL,
    CONSTRAINT [PK_TBFuncionario] PRIMARY KEY CLUSTERED ([Id] ASC)
);

