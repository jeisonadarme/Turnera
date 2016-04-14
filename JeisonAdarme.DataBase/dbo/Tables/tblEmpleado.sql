CREATE TABLE [dbo].[tblEmpleado] (
    [GUID]          UNIQUEIDENTIFIER NOT NULL,
    [IdUsuario]     NVARCHAR (128)   NULL,
    [Nombre]        NVARCHAR (100)   NOT NULL,
    [NumeroCelular] NVARCHAR (20)    NOT NULL,
    [EsActivo]      BIT              CONSTRAINT [DF__tblEmplea__EsAct__286302EC] DEFAULT ((1)) NULL,
    [IdEmpresa]     UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK__tblEmple__15B69B8EB6D8DEDF] PRIMARY KEY CLUSTERED ([GUID] ASC),
    FOREIGN KEY ([IdEmpresa]) REFERENCES [dbo].[tblEmpresa] ([GUID]),
    CONSTRAINT [FK__tblEmplea__IdUsu__2A4B4B5E] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[tblUsuario] ([Id])
);



