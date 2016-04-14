CREATE TABLE [dbo].[tblServicio] (
    [GUID]              UNIQUEIDENTIFIER NOT NULL,
    [IdEmpleado]        UNIQUEIDENTIFIER NULL,
    [IdTipoServicio]    INT              NULL,
    [IdEstadoServicio]  INT              NULL,
    [FechaPeticion]     DATETIME         NULL,
    [FechaTomado]       DATETIME         NULL,
    [FechaFinalizado]   DATETIME         NULL,
    [NivelSatisfaccion] INT              NULL,
    [CodigoServicio]    NVARCHAR (50)    NULL,
    [IdCliente]         UNIQUEIDENTIFIER NULL,
    [IdEmpresa]         UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([GUID] ASC),
    FOREIGN KEY ([IdCliente]) REFERENCES [dbo].[tblCliente] ([GUID]),
    FOREIGN KEY ([IdEmpresa]) REFERENCES [dbo].[tblEmpresa] ([GUID]),
    FOREIGN KEY ([IdEstadoServicio]) REFERENCES [dbo].[mas_tblEstadoServicio] ([IdEstadoServicio]),
    FOREIGN KEY ([IdTipoServicio]) REFERENCES [dbo].[tblTipoServicio] ([IdTipoServicio]),
    CONSTRAINT [FK__tblServic__IdEmp__2B3F6F97] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[tblEmpleado] ([GUID])
);







