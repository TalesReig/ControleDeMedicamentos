CREATE TABLE [dbo].[TBPaciente] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Nome]      VARCHAR (300) NOT NULL,
    [CartaoSUS] VARCHAR (20)  NOT NULL,
    CONSTRAINT [PK_TBPaciente] PRIMARY KEY CLUSTERED ([Id] ASC)
);

