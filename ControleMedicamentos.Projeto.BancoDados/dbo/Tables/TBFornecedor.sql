CREATE TABLE [dbo].[TBFornecedor] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Nome]     VARCHAR (300) NULL,
    [Telefone] VARCHAR (50)  NULL,
    [Email]    VARCHAR (100) NULL,
    [Cidade]   VARCHAR (150) NULL,
    [Estado]   VARCHAR (100) NULL,
    CONSTRAINT [PK_TBFornecedor] PRIMARY KEY CLUSTERED ([Id] ASC)
);

