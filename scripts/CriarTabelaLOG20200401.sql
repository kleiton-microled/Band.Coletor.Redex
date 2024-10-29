USE [REDEX]

CREATE TABLE [dbo].[TB_LOG_COLETOR](
	AUTONUM int IDENTITY(1,1) NOT NULL,
	[DATA] datetime,
	ORIGEM [nvarchar](255),
	MENSAGEM [nvarchar](1000),
	STACK_TRACE nvarchar(4000),
	USUARIO int,
PRIMARY KEY CLUSTERED 
(
	AUTONUM ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TB_LOG_COLETOR] ADD  DEFAULT (getdate()) FOR [DATA]
GO

EXEC sys.sp_addextendedproperty @name=N'ORIGEM', @value=N'Tela de Origem' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TB_LOG_COLETOR', @level2type=N'COLUMN',@level2name=N'ORIGEM'
GO

EXEC sys.sp_addextendedproperty @name=N'MENSAGEM', @value=N'Mensagem de Erro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TB_LOG_COLETOR', @level2type=N'COLUMN',@level2name=N'MENSAGEM'
GO

EXEC sys.sp_addextendedproperty @name=N'STACK_TRACE', @value=N'Stack do Erro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TB_LOG_COLETOR', @level2type=N'COLUMN',@level2name=N'STACK_TRACE'
GO




