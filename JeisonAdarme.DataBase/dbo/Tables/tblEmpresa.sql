CREATE TABLE [dbo].[tblEmpresa] (
    [GUID]           UNIQUEIDENTIFIER NOT NULL,
    [IdUsuario]      NVARCHAR (128)   NULL,
    [Nombre]         NVARCHAR (100)   NOT NULL,
    [TokenUnico]     NVARCHAR (20)    NOT NULL,
    [UrlImagen]      NVARCHAR (MAX)   NULL,
    [NumeroTelefono] NVARCHAR (20)    NULL,
    [NumeroCelular]  NVARCHAR (20)    NULL,
    [Direccion]      NVARCHAR (100)   NULL,
    [Longitud]       DECIMAL (18)     NULL,
    [Latitud]        DECIMAL (18)     NULL,
    [Descripcion]    NVARCHAR (500)   NULL,
    PRIMARY KEY CLUSTERED ([GUID] ASC),
    FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[tblUsuario] ([Id])
);

