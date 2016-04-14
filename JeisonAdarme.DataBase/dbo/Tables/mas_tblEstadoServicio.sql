CREATE TABLE [dbo].[mas_tblEstadoServicio] (
    [IdEstadoServicio]     INT            IDENTITY (1, 1) NOT NULL,
    [NombreEstadoServicio] NVARCHAR (100) NULL,
    [EsActivo]             BIT            DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([IdEstadoServicio] ASC)
);

