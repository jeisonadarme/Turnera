CREATE TABLE [dbo].[tblCliente] (
    [GUID]               UNIQUEIDENTIFIER NOT NULL,
    [Nombre]             NVARCHAR (100)   NULL,
    [Email]              NVARCHAR (100)   NULL,
    [PushRegistrationId] NVARCHAR (MAX)   NULL,
    PRIMARY KEY CLUSTERED ([GUID] ASC)
);



