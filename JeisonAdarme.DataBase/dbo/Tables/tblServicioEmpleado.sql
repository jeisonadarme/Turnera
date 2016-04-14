CREATE TABLE [dbo].[tblServicioEmpleado] (
    [GUID]           UNIQUEIDENTIFIER NOT NULL,
    [IdEmpleado]     UNIQUEIDENTIFIER NULL,
    [IdTipoServicio] INT              NULL,
    [EsActivo]       BIT              DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([GUID] ASC),
    FOREIGN KEY ([IdTipoServicio]) REFERENCES [dbo].[tblTipoServicio] ([IdTipoServicio]),
    CONSTRAINT [FK__tblServic__IdEmp__2F10007B] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[tblEmpleado] ([GUID])
);



