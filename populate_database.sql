-- Script para popular o banco de dados com dados de exemplo
-- Todos os registros serão associados ao UsuarioId = 1
-- Nomes seguem o padrão: nomes descritivos e realistas

USE APITCC;
GO

-- 1. POPULAR CATEGORIAS DE INSUMOS (5 registros)
INSERT INTO Categoria_Insumo (UsuarioId, descricao) VALUES
(1, 'Fertilizantes Nitrogenados'),
(1, 'Fertilizantes Fosfatados'),
(1, 'Fertilizantes Potássicos'),
(1, 'Micronutrientes'),
(1, 'Corretivos de Solo');

-- 2. POPULAR TIPOS DE AGROTÓXICOS (5 registros)
INSERT INTO Tipo_Agrotoxicos (UsuarioId, descricao) VALUES
(1, 'Herbicidas'),
(1, 'Fungicidas'),
(1, 'Inseticidas'),
(1, 'Acaricidas'),
(1, 'Nematicidas');

-- 3. POPULAR FORNECEDORES (5 registros)
INSERT INTO Fornecedores (UsuarioId, nome, cnpj, telefone) VALUES
(1, 'Agro Supply Ltda', '12.345.678/0001-01', '(11) 99999-0001'),
(1, 'Rural Comércio S.A.', '23.456.789/0001-02', '(11) 99999-0002'),
(1, 'Fertilizantes Brasil', '34.567.890/0001-03', '(11) 99999-0003'),
(1, 'Sementes Premium', '45.678.901/0001-04', '(11) 99999-0004'),
(1, 'Defensivos Naturais', '56.789.012/0001-05', '(11) 99999-0005');

-- 4. POPULAR LAVOURAS (5 registros)
INSERT INTO Lavouras (UsuarioId, area, nome, latitude, longitude) VALUES
(1, 150.5, 'Fazenda São João', -23.5505, -46.6333),
(1, 200.0, 'Sítio Boa Vista', -23.5600, -46.6400),
(1, 180.3, 'Chácara Primavera', -23.5700, -46.6500),
(1, 120.7, 'Rancho Alegre', -23.5800, -46.6600),
(1, 250.1, 'Estância Verde', -23.5900, -46.6700);

-- 5. POPULAR INSUMOS (5 registros)
INSERT INTO Insumos (UsuarioId, categoriaInsumoID, fornecedorID, nome, unidade_Medida, data_Cadastro, qtde, preco) VALUES
(1, 1, 1, 'Ureia 45%', 'kg', GETDATE(), 1000.0, 2.50),
(1, 2, 2, 'Superfosfato', 'kg', GETDATE(), 800.0, 3.20),
(1, 3, 3, 'Cloreto de Potássio', 'kg', GETDATE(), 600.0, 4.80),
(1, 4, 4, 'Sulfato de Zinco', 'kg', GETDATE(), 200.0, 15.90),
(1, 5, 5, 'Calcário Dolomítico', 'kg', GETDATE(), 5000.0, 0.85);

-- 6. POPULAR SEMENTES (5 registros)
INSERT INTO Sementes (UsuarioId, fornecedorID, nome, tipo, marca, qtde, data_Cadastro, preco) VALUES
(1, 4, 'Soja RR', 'Soja', 'Pioneer', 500.0, GETDATE(), 180.00),
(1, 4, 'Milho BT', 'Milho', 'Syngenta', 300.0, GETDATE(), 220.00),
(1, 4, 'Feijão Carioca', 'Feijão', 'Agroceres', 150.0, GETDATE(), 95.00),
(1, 4, 'Arroz Irrigado', 'Arroz', 'BRS', 400.0, GETDATE(), 75.00),
(1, 4, 'Trigo BRS', 'Trigo', 'Embrapa', 250.0, GETDATE(), 120.00);

-- 7. POPULAR AGROTÓXICOS (5 registros)
INSERT INTO Agrotoxicos (UsuarioId, fornecedorID, tipoID, nome, unidade_Medida, data_Cadastro, qtde, preco) VALUES
(1, 1, 1, 'Roundup Ready', 'L', GETDATE(), 50.0, 45.80),
(1, 2, 2, 'Mancozeb 800', 'kg', GETDATE(), 25.0, 28.50),
(1, 3, 3, 'Deltametrina', 'L', GETDATE(), 30.0, 65.20),
(1, 4, 4, 'Enxofre 800', 'kg', GETDATE(), 40.0, 12.30),
(1, 5, 5, 'Carbofuran', 'kg', GETDATE(), 15.0, 89.90);

-- 8. POPULAR PLANTIOS (5 registros)
INSERT INTO Plantios (UsuarioId, lavouraID, sementeID, descricao, dataHora, areaPlantada, qtde) VALUES
(1, 1, 1, 'Plantio de Soja - Safra 2024', DATEADD(day, -30, GETDATE()), 150.5, 500.0),
(1, 2, 2, 'Plantio de Milho - Safra 2024', DATEADD(day, -25, GETDATE()), 200.0, 300.0),
(1, 3, 3, 'Plantio de Feijão - Safra 2024', DATEADD(day, -20, GETDATE()), 180.3, 150.0),
(1, 4, 4, 'Plantio de Arroz - Safra 2024', DATEADD(day, -15, GETDATE()), 120.7, 400.0),
(1, 5, 5, 'Plantio de Trigo - Safra 2024', DATEADD(day, -10, GETDATE()), 250.1, 250.0);

-- 9. POPULAR APLICAÇÕES DE AGROTÓXICOS (5 registros)
INSERT INTO Aplicacoes (UsuarioId, lavouraID, agrotoxicoID, descricao, dataHora, qtde) VALUES
(1, 1, 1, 'Aplicação de Herbicida na Soja', DATEADD(day, -28, GETDATE()), 5.0),
(1, 2, 2, 'Aplicação de Fungicida no Milho', DATEADD(day, -23, GETDATE()), 8.0),
(1, 3, 3, 'Aplicação de Inseticida no Feijão', DATEADD(day, -18, GETDATE()), 3.0),
(1, 4, 4, 'Aplicação de Acaricida no Arroz', DATEADD(day, -13, GETDATE()), 6.0),
(1, 5, 5, 'Aplicação de Nematicida no Trigo', DATEADD(day, -8, GETDATE()), 4.0);

-- 10. POPULAR APLICAÇÕES DE INSUMOS (5 registros)
INSERT INTO Aplicacao_Insumos (UsuarioId, lavouraID, insumoID, descricao, dataHora, qtde) VALUES
(1, 1, 1, 'Adubação Nitrogenada na Soja', DATEADD(day, -27, GETDATE()), 200.0),
(1, 2, 2, 'Adubação Fosfatada no Milho', DATEADD(day, -22, GETDATE()), 300.0),
(1, 3, 3, 'Adubação Potássica no Feijão', DATEADD(day, -17, GETDATE()), 150.0),
(1, 4, 4, 'Aplicação de Micronutrientes no Arroz', DATEADD(day, -12, GETDATE()), 50.0),
(1, 5, 5, 'Calagem no Trigo', DATEADD(day, -7, GETDATE()), 1000.0);

-- 11. POPULAR COLHEITAS (5 registros)
INSERT INTO Colheitas (UsuarioId, lavouraID, tipo, dataHora, descricao, quantidadeSacas, areaHectares, cooperativaDestino, precoPorSaca) VALUES
(1, 1, 'Colheita Mecanizada', DATEADD(day, -5, GETDATE()), 'Colheita de Soja - Safra 2024', 4500.0, 150.5, 'Cooperativa Central', 85.50),
(1, 2, 'Colheita Mecanizada', DATEADD(day, -3, GETDATE()), 'Colheita de Milho - Safra 2024', 12000.0, 200.0, 'Cooperativa Sul', 45.80),
(1, 3, 'Colheita Manual', DATEADD(day, -1, GETDATE()), 'Colheita de Feijão - Safra 2024', 900.0, 180.3, 'Cooperativa Norte', 120.00),
(1, 4, 'Colheita Mecanizada', GETDATE(), 'Colheita de Arroz - Safra 2024', 2400.0, 120.7, 'Cooperativa Leste', 65.30),
(1, 5, 'Colheita Mecanizada', DATEADD(day, 2, GETDATE()), 'Colheita de Trigo - Safra 2024', 3750.0, 250.1, 'Cooperativa Oeste', 78.90);

-- 12. POPULAR CUSTOS (5 registros)
INSERT INTO Custos (UsuarioId, lavouraID, aplicacaoAgrotoxicoID, aplicacaoInsumoID, plantioID, colheitaID, custoTotal, ganhoTotal) VALUES
(1, 1, 1, 1, 1, 1, 12500.00, 384750.00),
(1, 2, 2, 2, 2, 2, 18900.00, 549600.00),
(1, 3, 3, 3, 3, 3, 8500.00, 108000.00),
(1, 4, 4, 4, 4, 4, 11200.00, 156720.00),
(1, 5, 5, 5, 5, 5, 15800.00, 295875.00);

-- 13. POPULAR MOVIMENTAÇÕES DE ESTOQUE (5 registros)
INSERT INTO MovimentacoesEstoque (UsuarioId, lavouraID, movimentacao, agrotoxicoID, sementeID, insumoID, qtde, dataHora, descricao, origemAplicacaoID, origemAplicacaoInsumoID, origemPlantioID) VALUES
(1, 1, 1, NULL, NULL, 1, 1000.0, DATEADD(day, -35, GETDATE()), 'Entrada de Fertilizante Nitrogenado', NULL, NULL, NULL),
(1, 2, 2, 1, NULL, NULL, 5.0, DATEADD(day, -28, GETDATE()), 'Saída de Herbicida para Aplicação', 1, NULL, NULL),
(1, 3, 1, NULL, 1, NULL, 500.0, DATEADD(day, -32, GETDATE()), 'Entrada de Sementes de Soja', NULL, NULL, NULL),
(1, 4, 2, NULL, NULL, 2, 300.0, DATEADD(day, -22, GETDATE()), 'Saída de Fertilizante Fosfatado', NULL, 2, NULL),
(1, 5, 1, NULL, NULL, 5, 5000.0, DATEADD(day, -40, GETDATE()), 'Entrada de Calcário Dolomítico', NULL, NULL, NULL);

PRINT 'Banco de dados populado com sucesso!';
PRINT 'Total de registros inseridos: 65';
PRINT 'Todos os registros associados ao UsuarioId = 1';
PRINT 'Nomes seguem o padrão: nomes descritivos e realistas';

