CREATE TABLE [dbo].[ANAGRAFICHE] (
    [IdAnagrafica] INT          IDENTITY (1, 1) NOT NULL,
    [Cognome]      VARCHAR (50) NULL,
    [Nome]         VARCHAR (50) NULL,
    [Indirizzo]    VARCHAR (80) NULL,
    [Città]        VARCHAR (58) NULL,
    [CAP]          VARCHAR (10) NULL,
    [Cod_Fisc]     VARCHAR (16) NULL,
    PRIMARY KEY CLUSTERED ([IdAnagrafica] ASC)
);

