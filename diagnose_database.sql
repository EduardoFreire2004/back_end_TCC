-- Script de Diagnóstico para Banco de Dados APITCC
-- Execute este script para identificar problemas

PRINT '=== DIAGNÓSTICO DO BANCO DE DADOS ===';

-- 1. Verificar se o banco existe
IF DB_ID('APITCC') IS NOT NULL
BEGIN
    PRINT '✅ Banco APITCC existe';
    
    USE APITCC;
    
    -- 2. Verificar tabelas existentes
    PRINT '📋 Tabelas existentes:';
    SELECT TABLE_NAME, TABLE_TYPE 
    FROM INFORMATION_SCHEMA.TABLES 
    ORDER BY TABLE_NAME;
    
    -- 3. Verificar se tabela Usuarios existe
    IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Usuarios')
    BEGIN
        PRINT '✅ Tabela Usuarios existe';
        
        -- 4. Verificar usuários existentes
        PRINT '👥 Usuários existentes:';
        SELECT Id, Nome, Email FROM Usuarios;
        
        -- 5. Verificar se UsuarioId = 1 existe
        IF EXISTS (SELECT * FROM Usuarios WHERE Id = 1)
        BEGIN
            PRINT '✅ UsuarioId = 1 existe';
        END
        ELSE
        BEGIN
            PRINT '❌ UsuarioId = 1 NÃO existe';
            PRINT '💡 Crie o usuário primeiro:';
            PRINT 'INSERT INTO Usuarios (Id, Nome, Email, Senha, Telefone, DataCadastro, Ativo)';
            PRINT 'VALUES (1, ''Usuário Teste'', ''teste@email.com'', ''senha123'', ''(11) 99999-9999'', GETDATE(), 1);';
        END
    END
    ELSE
    BEGIN
        PRINT '❌ Tabela Usuarios NÃO existe';
        PRINT '💡 Execute as migrations do Entity Framework primeiro';
    END
    
    -- 6. Verificar outras tabelas importantes
    PRINT '🔍 Verificando tabelas principais:';
    
    DECLARE @tabelas TABLE (nome VARCHAR(100), existe BIT);
    
    INSERT INTO @tabelas VALUES
    ('Categoria_Insumo', CASE WHEN EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Categoria_Insumo') THEN 1 ELSE 0 END),
    ('Tipo_Agrotoxicos', CASE WHEN EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tipo_Agrotoxicos') THEN 1 ELSE 0 END),
    ('Fornecedores', CASE WHEN EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Fornecedores') THEN 1 ELSE 0 END),
    ('Lavouras', CASE WHEN EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Lavouras') THEN 1 ELSE 0 END);
    
    SELECT nome, 
           CASE WHEN existe = 1 THEN '✅ Existe' ELSE '❌ Não existe' END as status
    FROM @tabelas;
    
END
ELSE
BEGIN
    PRINT '❌ Banco APITCC NÃO existe';
    PRINT '💡 Crie o banco primeiro:';
    PRINT 'CREATE DATABASE APITCC;';
END

PRINT '=== FIM DO DIAGNÓSTICO ===';










