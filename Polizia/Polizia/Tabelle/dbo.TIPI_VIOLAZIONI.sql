CREATE TABLE [dbo].[TIPI_VIOLAZIONI] (
    [IdViolazione] INT           IDENTITY (1, 1) NOT NULL,
    [Descrizione]  VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([IdViolazione] ASC)
);

