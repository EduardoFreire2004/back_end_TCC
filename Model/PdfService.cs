using API_TCC.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace API_TCC.Model
{
    public class PdfService : IPdfService
    {
        public byte[] GerarRelatorioCustos(RelatorioCustosDto relatorio)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeCustosContent(x, relatorio));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GerarRelatorioInsumos(RelatorioInsumosDto relatorio)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeInsumosContent(x, relatorio));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GerarRelatorioAgrotoxicos(RelatorioAgrotoxicosDto relatorio)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeAgrotoxicosContent(x, relatorio));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GerarRelatorioSementes(RelatorioSementesDto relatorio)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeSementesContent(x, relatorio));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GerarRelatorioPlantios(RelatorioPlantiosDto relatorio)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposePlantiosContent(x, relatorio));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GerarRelatorioAplicacoes(RelatorioAplicacoesDto relatorio)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeAplicacoesContent(x, relatorio));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GerarRelatorioColheitas(RelatorioColheitasDto relatorio)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeColheitasContent(x, relatorio));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GerarRelatorioEstoque(RelatorioEstoqueDto relatorio)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeEstoqueContent(x, relatorio));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GerarRelatorioGeral(RelatorioGeralDto relatorio)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeGeralContent(x, relatorio));
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Sistema de Gestão Agrícola").FontSize(16).Bold();
                    column.Item().Text("Relatório Gerencial").FontSize(12).Italic();
                });

                row.RelativeItem().AlignRight().Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(10);
            });
        }

        private void ComposeCustosContent(IContainer container, RelatorioCustosDto relatorio)
        {
            container.Column(column =>
            {
                column.Item().Text("RELATÓRIO DE CUSTOS").FontSize(18).Bold();
                column.Item().Height(20);

                // Informações da Lavoura
                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Column(col =>
                {
                    col.Item().Text($"Lavoura: {relatorio.Lavoura.Nome}").Bold();
                    col.Item().Text($"Área: {relatorio.Lavoura.Area:F2} hectares");
                    col.Item().Text($"Status: {relatorio.Lavoura.Status}");
                });

                column.Item().Height(20);

                // Estatísticas
                column.Item().Text("RESUMO FINANCEIRO").FontSize(14).Bold();
                column.Item().Grid(grid =>
                {
                    grid.Columns(2);
                    grid.Item().Text("Total de Custos:").Bold();
                    grid.Item().Text($"R$ {relatorio.Estatisticas.TotalCustos:F2}");
                    grid.Item().Text("Custo por Hectare:").Bold();
                    grid.Item().Text($"R$ {relatorio.Estatisticas.CustoPorHectare:F2}");
                });

                column.Item().Height(20);

                // Lista de Custos
                column.Item().Text("DETALHAMENTO DE CUSTOS").FontSize(14).Bold();
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Data").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Categoria").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Valor").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Descrição").Bold();
                    });

                    foreach (var custo in relatorio.Custos)
                    {
                        table.Cell().Text(custo.Data.ToString("dd/MM/yyyy"));
                        table.Cell().Text(custo.Categoria);
                        table.Cell().Text($"R$ {custo.Valor:F2}");
                        table.Cell().Text(custo.Descricao);
                    }
                });
            });
        }

        private void ComposeInsumosContent(IContainer container, RelatorioInsumosDto relatorio)
        {
            container.Column(column =>
            {
                column.Item().Text("RELATÓRIO DE INSUMOS").FontSize(18).Bold();
                column.Item().Height(20);

                // Informações da Lavoura
                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Column(col =>
                {
                    col.Item().Text($"Lavoura: {relatorio.Lavoura.Nome}").Bold();
                    col.Item().Text($"Área: {relatorio.Lavoura.Area:F2} hectares");
                    col.Item().Text($"Status: {relatorio.Lavoura.Status}");
                });

                column.Item().Height(20);

                // Estatísticas
                column.Item().Text("RESUMO DE INSUMOS").FontSize(14).Bold();
                column.Item().Grid(grid =>
                {
                    grid.Columns(2);
                    grid.Item().Text("Total de Insumos:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.TotalInsumos.ToString());
                    grid.Item().Text("Valor Total:").Bold();
                    grid.Item().Text($"R$ {relatorio.Estatisticas.ValorTotalEstoque:F2}");
                    grid.Item().Text("Quantidade Total:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.QuantidadeTotal.ToString());
                    grid.Item().Text("Total de Aplicações:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.TotalAplicacoes.ToString());
                });

                column.Item().Height(20);

                // Lista de Insumos
                column.Item().Text("DETALHAMENTO DE INSUMOS").FontSize(14).Bold();
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Nome").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Categoria").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Fornecedor").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Quantidade").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Valor").Bold();
                    });

                    foreach (var insumo in relatorio.Insumos)
                    {
                        table.Cell().Text(insumo.Nome);
                        table.Cell().Text(insumo.Categoria);
                        table.Cell().Text(insumo.Fornecedor);
                        table.Cell().Text($"{insumo.Quantidade} {insumo.UnidadeMedida}");
                        table.Cell().Text($"R$ {insumo.ValorTotal:F2}");
                    }
                });
            });
        }

        private void ComposeAgrotoxicosContent(IContainer container, RelatorioAgrotoxicosDto relatorio)
        {
            container.Column(column =>
            {
                column.Item().Text("RELATÓRIO DE AGROTÓXICOS").FontSize(18).Bold();
                column.Item().Height(20);

                // Informações da Lavoura
                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Column(col =>
                {
                    col.Item().Text($"Lavoura: {relatorio.Lavoura.Nome}").Bold();
                    col.Item().Text($"Área: {relatorio.Lavoura.Area:F2} hectares");
                    col.Item().Text($"Status: {relatorio.Lavoura.Status}");
                });

                column.Item().Height(20);

                // Estatísticas
                column.Item().Text("RESUMO DE AGROTÓXICOS").FontSize(14).Bold();
                column.Item().Grid(grid =>
                {
                    grid.Columns(2);
                    grid.Item().Text("Total de Agrotóxicos:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.TotalAgrotoxicos.ToString());
                    grid.Item().Text("Valor Total:").Bold();
                    grid.Item().Text($"R$ {relatorio.Estatisticas.ValorTotalEstoque:F2}");
                    grid.Item().Text("Total de Aplicações:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.TotalAplicacoes.ToString());
                    grid.Item().Text("Área Total Aplicada:").Bold();
                    grid.Item().Text($"{relatorio.Estatisticas.AreaTotalAplicada:F2} hectares");
                });

                column.Item().Height(20);

                // Lista de Agrotóxicos
                column.Item().Text("DETALHAMENTO DE AGROTÓXICOS").FontSize(14).Bold();
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Nome").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Tipo").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Fornecedor").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Quantidade").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Valor").Bold();
                    });

                    foreach (var agrotoxico in relatorio.Agrotoxicos)
                    {
                        table.Cell().Text(agrotoxico.Nome);
                        table.Cell().Text(agrotoxico.Tipo);
                        table.Cell().Text(agrotoxico.Fornecedor);
                        table.Cell().Text($"{agrotoxico.Quantidade} {agrotoxico.UnidadeMedida}");
                        table.Cell().Text($"R$ {agrotoxico.ValorTotal:F2}");
                    }
                });
            });
        }

        private void ComposeSementesContent(IContainer container, RelatorioSementesDto relatorio)
        {
            container.Column(column =>
            {
                column.Item().Text("RELATÓRIO DE SEMENTES").FontSize(18).Bold();
                column.Item().Height(20);

                // Informações da Lavoura
                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Column(col =>
                {
                    col.Item().Text($"Lavoura: {relatorio.Lavoura.Nome}").Bold();
                    col.Item().Text($"Área: {relatorio.Lavoura.Area:F2} hectares");
                    col.Item().Text($"Status: {relatorio.Lavoura.Status}");
                });

                column.Item().Height(20);

                // Estatísticas
                column.Item().Text("RESUMO DE SEMENTES").FontSize(14).Bold();
                column.Item().Grid(grid =>
                {
                    grid.Columns(2);
                    grid.Item().Text("Total de Sementes:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.TotalSementes.ToString());
                    grid.Item().Text("Valor Total:").Bold();
                    grid.Item().Text($"R$ {relatorio.Estatisticas.ValorTotalEstoque:F2}");
                    grid.Item().Text("Total de Plantios:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.TotalPlantios.ToString());
                    grid.Item().Text("Área Total Plantada:").Bold();
                    grid.Item().Text($"{relatorio.Estatisticas.AreaTotalPlantada:F2} hectares");
                });

                column.Item().Height(20);

                // Lista de Sementes
                column.Item().Text("DETALHAMENTO DE SEMENTES").FontSize(14).Bold();
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Nome").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Cultura").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Fornecedor").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Quantidade").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Valor").Bold();
                    });

                    foreach (var semente in relatorio.Sementes)
                    {
                        table.Cell().Text(semente.Nome);
                        table.Cell().Text(semente.Cultura);
                        table.Cell().Text(semente.Fornecedor);
                        table.Cell().Text($"{semente.Quantidade} {semente.UnidadeMedida}");
                        table.Cell().Text($"R$ {semente.ValorTotal:F2}");
                    }
                });
            });
        }

        private void ComposePlantiosContent(IContainer container, RelatorioPlantiosDto relatorio)
        {
            container.Column(column =>
            {
                column.Item().Text("RELATÓRIO DE PLANTIOS").FontSize(18).Bold();
                column.Item().Height(20);

                // Informações da Lavoura
                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Column(col =>
                {
                    col.Item().Text($"Lavoura: {relatorio.Lavoura.Nome}").Bold();
                    col.Item().Text($"Área: {relatorio.Lavoura.Area:F2} hectares");
                    col.Item().Text($"Status: {relatorio.Lavoura.Status}");
                });

                column.Item().Height(20);

                // Estatísticas
                column.Item().Text("RESUMO DE PLANTIOS").FontSize(14).Bold();
                column.Item().Grid(grid =>
                {
                    grid.Columns(2);
                    grid.Item().Text("Total de Plantios:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.TotalPlantios.ToString());
                    grid.Item().Text("Área Total Plantada:").Bold();
                    grid.Item().Text($"{relatorio.Estatisticas.AreaTotalPlantada:F2} hectares");
                    grid.Item().Text("Culturas:").Bold();
                    grid.Item().Text(string.Join(", ", relatorio.Estatisticas.Culturas));
                });

                column.Item().Height(20);

                // Lista de Plantios
                column.Item().Text("DETALHAMENTO DE PLANTIOS").FontSize(14).Bold();
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Data").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Cultura").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Área").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Status").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Observações").Bold();
                    });

                    foreach (var plantio in relatorio.Plantios)
                    {
                        table.Cell().Text(plantio.DataPlantio.ToString("dd/MM/yyyy"));
                        table.Cell().Text(plantio.Cultura);
                        table.Cell().Text($"{plantio.AreaPlantada:F2} ha");
                        table.Cell().Text(plantio.Status);
                        table.Cell().Text(plantio.Observacoes ?? "-");
                    }
                });
            });
        }

        private void ComposeAplicacoesContent(IContainer container, RelatorioAplicacoesDto relatorio)
        {
            container.Column(column =>
            {
                column.Item().Text("RELATÓRIO DE APLICAÇÕES").FontSize(18).Bold();
                column.Item().Height(20);

                // Informações da Lavoura
                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Column(col =>
                {
                    col.Item().Text($"Lavoura: {relatorio.Lavoura.Nome}").Bold();
                    col.Item().Text($"Área: {relatorio.Lavoura.Area:F2} hectares");
                    col.Item().Text($"Status: {relatorio.Lavoura.Status}");
                });

                column.Item().Height(20);

                // Estatísticas
                column.Item().Text("RESUMO DE APLICAÇÕES").FontSize(14).Bold();
                column.Item().Grid(grid =>
                {
                    grid.Columns(2);
                    grid.Item().Text("Total de Aplicações:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.TotalAplicacoes.ToString());
                    grid.Item().Text("Área Total Aplicada:").Bold();
                    grid.Item().Text($"{relatorio.Estatisticas.AreaTotalAplicada:F2} hectares");
                    grid.Item().Text("Produtos Utilizados:").Bold();
                    grid.Item().Text(string.Join(", ", relatorio.Estatisticas.ProdutosUtilizados));
                });

                column.Item().Height(20);

                // Lista de Aplicações
                column.Item().Text("DETALHAMENTO DE APLICAÇÕES").FontSize(14).Bold();
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Data").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Produto").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Área").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Dosagem").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Observações").Bold();
                    });

                    foreach (var aplicacao in relatorio.Aplicacoes)
                    {
                        table.Cell().Text(aplicacao.DataAplicacao.ToString("dd/MM/yyyy"));
                        table.Cell().Text(aplicacao.Produto);
                        table.Cell().Text($"{aplicacao.AreaAplicada:F2} ha");
                        table.Cell().Text(aplicacao.Dosagem);
                        table.Cell().Text(aplicacao.Observacoes ?? "-");
                    }
                });
            });
        }

        private void ComposeColheitasContent(IContainer container, RelatorioColheitasDto relatorio)
        {
            container.Column(column =>
            {
                column.Item().Text("RELATÓRIO DE COLHEITAS").FontSize(18).Bold();
                column.Item().Height(20);

                // Informações da Lavoura
                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Column(col =>
                {
                    col.Item().Text($"Lavoura: {relatorio.Lavoura.Nome}").Bold();
                    col.Item().Text($"Área: {relatorio.Lavoura.Area:F2} hectares");
                    col.Item().Text($"Status: {relatorio.Lavoura.Status}");
                });

                column.Item().Height(20);

                // Estatísticas
                column.Item().Text("RESUMO DE COLHEITAS").FontSize(14).Bold();
                column.Item().Grid(grid =>
                {
                    grid.Columns(2);
                    grid.Item().Text("Total de Colheitas:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.TotalColheitas.ToString());
                    grid.Item().Text("Área Total Colhida:").Bold();
                    grid.Item().Text($"{relatorio.Estatisticas.AreaTotalColhida:F2} hectares");
                    grid.Item().Text("Produtividade Média:").Bold();
                    grid.Item().Text($"{relatorio.Estatisticas.ProdutividadeMedia:F2} kg/ha");
                    grid.Item().Text("Quantidade Total:").Bold();
                    grid.Item().Text($"{relatorio.Estatisticas.QuantidadeTotal:F2} kg");
                });

                column.Item().Height(20);

                // Lista de Colheitas
                column.Item().Text("DETALHAMENTO DE COLHEITAS").FontSize(14).Bold();
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Data").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Cultura").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Área").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Produtividade").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Quantidade").Bold();
                    });

                    foreach (var colheita in relatorio.Colheitas)
                    {
                        table.Cell().Text(colheita.DataColheita.ToString("dd/MM/yyyy"));
                        table.Cell().Text(colheita.Cultura);
                        table.Cell().Text($"{colheita.AreaColhida:F2} ha");
                        table.Cell().Text($"{colheita.Produtividade:F2} kg/ha");
                        table.Cell().Text($"{colheita.QuantidadeColhida:F2} kg");
                    }
                });
            });
        }

        private void ComposeEstoqueContent(IContainer container, RelatorioEstoqueDto relatorio)
        {
            container.Column(column =>
            {
                column.Item().Text("RELATÓRIO DE ESTOQUE").FontSize(18).Bold();
                column.Item().Height(20);

                // Informações da Lavoura
                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Column(col =>
                {
                    col.Item().Text($"Lavoura: {relatorio.Lavoura.Nome}").Bold();
                    col.Item().Text($"Área: {relatorio.Lavoura.Area:F2} hectares");
                    col.Item().Text($"Status: {relatorio.Lavoura.Status}");
                });

                column.Item().Height(20);

                // Estatísticas
                column.Item().Text("RESUMO DE ESTOQUE").FontSize(14).Bold();
                column.Item().Grid(grid =>
                {
                    grid.Columns(2);
                    grid.Item().Text("Total de Entradas:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.TotalEntradas.ToString());
                    grid.Item().Text("Total de Saídas:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.TotalSaidas.ToString());
                    grid.Item().Text("Produtos em Estoque:").Bold();
                    grid.Item().Text(relatorio.Estatisticas.ProdutosEmEstoque.ToString());
                    grid.Item().Text("Valor do Estoque:").Bold();
                    grid.Item().Text($"R$ {relatorio.Estatisticas.ValorEstoque:F2}");
                });

                column.Item().Height(20);

                // Saldo Atual
                column.Item().Text("SALDO ATUAL DO ESTOQUE").FontSize(14).Bold();
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Produto").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Saldo").Bold();
                    });

                    foreach (var saldo in relatorio.SaldoAtual)
                    {
                        table.Cell().Text(saldo.Key);
                        table.Cell().Text(saldo.Value.ToString());
                    }
                });

                column.Item().Height(20);

                // Movimentações
                column.Item().Text("MOVIMENTAÇÕES RECENTES").FontSize(14).Bold();
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Data").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Tipo").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Produto").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Quantidade").Bold();
                        header.Cell().Background(Colors.Grey.Lighten2).Text("Observações").Bold();
                    });

                    foreach (var movimentacao in relatorio.Movimentacoes)
                    {
                        table.Cell().Text(movimentacao.Data.ToString("dd/MM/yyyy"));
                        table.Cell().Text(movimentacao.Tipo);
                        table.Cell().Text(movimentacao.Produto);
                        table.Cell().Text($"{movimentacao.Quantidade} {movimentacao.Unidade}");
                        table.Cell().Text(movimentacao.Observacoes ?? "-");
                    }
                });
            });
        }

        private void ComposeGeralContent(IContainer container, RelatorioGeralDto relatorio)
        {
            container.Column(column =>
            {
                column.Item().Text("RELATÓRIO GERAL").FontSize(18).Bold();
                column.Item().Height(20);

                // Informações da Lavoura
                column.Item().Background(Colors.Grey.Lighten3).Padding(10).Column(col =>
                {
                    col.Item().Text($"Lavoura: {relatorio.Lavoura.Nome}").Bold();
                    col.Item().Text($"Área: {relatorio.Lavoura.Area:F2} hectares");
                    col.Item().Text($"Status: {relatorio.Lavoura.Status}");
                });

                column.Item().Height(20);

                // Resumo Geral
                column.Item().Text("RESUMO GERAL").FontSize(14).Bold();
                column.Item().Grid(grid =>
                {
                    grid.Columns(2);
                    grid.Item().Text("Total de Plantios:").Bold();
                    grid.Item().Text(relatorio.Resumo.TotalPlantios.ToString());
                    grid.Item().Text("Total de Aplicações (Agrotóxicos):").Bold();
                    grid.Item().Text(relatorio.Resumo.TotalAplicacoesAgrotoxicos.ToString());
                    grid.Item().Text("Total de Aplicações (Insumos):").Bold();
                    grid.Item().Text(relatorio.Resumo.TotalAplicacoesInsumos.ToString());
                    grid.Item().Text("Total de Colheitas:").Bold();
                    grid.Item().Text(relatorio.Resumo.TotalColheitas.ToString());
                    grid.Item().Text("Custo Total:").Bold();
                    grid.Item().Text($"R$ {relatorio.Resumo.CustoTotal:F2}");
                    grid.Item().Text("Receita Total:").Bold();
                    grid.Item().Text($"R$ {relatorio.Resumo.ReceitaTotal:F2}");
                    grid.Item().Text("Lucro Estimado:").Bold();
                    grid.Item().Text($"R$ {relatorio.Resumo.LucroEstimado:F2}");
                });
            });
        }
    }
}











