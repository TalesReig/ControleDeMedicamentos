CREATE TABLE [dbo].[TBRequisicao] (
    [Id]                    INT      IDENTITY (1, 1) NOT NULL,
    [Funcionario_Id]        INT      NOT NULL,
    [Paciente_Id]           INT      NOT NULL,
    [Medicamento_Id]        INT      NOT NULL,
    [QuantidadeMedicamento] INT      NOT NULL,
    [Data]                  DATETIME NOT NULL,
    CONSTRAINT [PK_TBRequisicao] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TBRequisicao_TBFuncionario] FOREIGN KEY ([Funcionario_Id]) REFERENCES [dbo].[TBFuncionario] ([Id]),
    CONSTRAINT [FK_TBRequisicao_TBMedicamento] FOREIGN KEY ([Medicamento_Id]) REFERENCES [dbo].[TBMedicamento] ([Id]),
    CONSTRAINT [FK_TBRequisicao_TBPaciente] FOREIGN KEY ([Paciente_Id]) REFERENCES [dbo].[TBPaciente] ([Id])
);

