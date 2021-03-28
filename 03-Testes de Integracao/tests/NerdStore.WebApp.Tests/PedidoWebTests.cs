using AngleSharp.Html.Parser;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
namespace NerdStore.WebApp.Tests
{
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class PedidoWebTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;

        public PedidoWebTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Adicionar item em novo pedido")]
        [Trait("Categoria", "Integração Web - Pedido")]
        public async Task AdicionarItem_NovoPedido_DeveAtualizarValorTotal()
        {
            // Arrange
            var produtoId = new Guid("03cc1b7f-9f4c-4c74-92d4-b954bfc8d069");
            var quantidade = 2;

            await _testsFixture.RealizarLoginWeb();

            var initialResponse = await _testsFixture.Client.GetAsync($"/produto-detalhe/{produtoId}");
            initialResponse.EnsureSuccessStatusCode();

            var formData = new Dictionary<string, string>
            {
                {"Id", produtoId.ToString() },
                {"quantidade", quantidade.ToString() }
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/meu-carrinho")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var postResponse = await _testsFixture.Client.SendAsync(postRequest);

            // Assert  
            postResponse.EnsureSuccessStatusCode();

            var html = new HtmlParser()   //Lib: AngleSharp
                .ParseDocumentAsync(await postResponse.Content.ReadAsStringAsync())
                .Result
                .All;

            var formQuantidade = html?.FirstOrDefault(c => c.Id == "quantidade")?.GetAttribute("value").ApenasNumeros();
            var formValorUnitario = html?.FirstOrDefault(c => c.Id == "valorUnitario")?.TextContent.Split(",")[0]?.ApenasNumeros();
            var formValorTotal = html?.FirstOrDefault(c => c.Id == "valorTotal")?.TextContent.Split(",")[0]?.ApenasNumeros();

            Assert.Equal(formValorTotal, formValorUnitario * formQuantidade);
        }
    }
}
