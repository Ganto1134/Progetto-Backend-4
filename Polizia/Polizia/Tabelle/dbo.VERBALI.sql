CREATE TABLE [dbo].[VERBALI] (
    [IdVerbale]               INT             IDENTITY (1, 1) NOT NULL,
    [IdAnagrafica]            INT             NOT NULL,
    [IdViolazione]            INT             NOT NULL,
    [DataViolazione]          DATETIME2 (7)   NOT NULL,
    [IndirizzoViolazione]     VARCHAR (80)    NOT NULL,
    [Nominativo_Agente]       VARCHAR (50)    NOT NULL,
    [DataTrascrizioneVerbale] DATE            NOT NULL,
    [Importo]                 DECIMAL (10, 2) NOT NULL,
    [DecurtamentoPunti]       INT             NULL,
    PRIMARY KEY CLUSTERED ([IdVerbale] ASC),
    FOREIGN KEY ([IdAnagrafica]) REFERENCES [dbo].[ANAGRAFICHE] ([IdAnagrafica]),
    FOREIGN KEY ([IdViolazione]) REFERENCES [dbo].[TIPI_VIOLAZIONI] ([IdViolazione])
);

