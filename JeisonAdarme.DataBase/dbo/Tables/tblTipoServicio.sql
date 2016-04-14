CREATE TABLE [dbo].[tblTipoServicio] (
    [IdTipoServicio]     INT              IDENTITY (1, 1) NOT NULL,
    [NombreTipoServicio] NVARCHAR (100)   NOT NULL,
    [Complejidad]        INT              NULL,
    [TiempoEstimado]     INT              NULL,
    [EsActivo]           BIT              NULL,
    [Precio]             DECIMAL (18, 2)  NULL,
    [IdEmpresa]          UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([IdTipoServicio] ASC),
    FOREIGN KEY ([IdEmpresa]) REFERENCES [dbo].[tblEmpresa] ([GUID])
);



