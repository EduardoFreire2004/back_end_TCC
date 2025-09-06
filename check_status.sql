-- Verificação Rápida do Status do Banco
USE APITCC;

PRINT '=== VERIFICAÇÃO RÁPIDA ===';

-- Verificar usuários
PRINT '👥 Usuários:';
SELECT Id, Nome, Email FROM Usuarios;

-- Verificar contagem por tabela
PRINT '📊 Contagem por Tabela:';
SELECT 'Categoria_Insumo' as Tabela, COUNT(*) as Total FROM Categoria_Insumo WHERE UsuarioId = 1
UNION ALL
SELECT 'Tipo_Agrotoxicos', COUNT(*) FROM Tipo_Agrotoxicos WHERE UsuarioId = 1
UNION ALL
SELECT 'Fornecedores', COUNT(*) FROM Fornecedores WHERE UsuarioId = 1
UNION ALL
SELECT 'Lavouras', COUNT(*) FROM Lavouras WHERE UsuarioId = 1
UNION ALL
SELECT 'Insumos', COUNT(*) FROM Insumos WHERE UsuarioId = 1
UNION ALL
SELECT 'Sementes', COUNT(*) FROM Sementes WHERE UsuarioId = 1
UNION ALL
SELECT 'Agrotoxicos', COUNT(*) FROM Agrotoxicos WHERE UsuarioId = 1
UNION ALL
SELECT 'Plantios', COUNT(*) FROM Plantios WHERE UsuarioId = 1
UNION ALL
SELECT 'Aplicacoes', COUNT(*) FROM Aplicacoes WHERE UsuarioId = 1
UNION ALL
SELECT 'Aplicacao_Insumos', COUNT(*) FROM Aplicacao_Insumos WHERE UsuarioId = 1
UNION ALL
SELECT 'Colheitas', COUNT(*) FROM Colheitas WHERE UsuarioId = 1
UNION ALL
SELECT 'Custos', COUNT(*) FROM Custos WHERE UsuarioId = 1
UNION ALL
SELECT 'MovimentacoesEstoque', COUNT(*) FROM MovimentacoesEstoque WHERE UsuarioId = 1;

PRINT '=== FIM DA VERIFICAÇÃO ===';










