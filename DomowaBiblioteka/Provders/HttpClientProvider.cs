using DomowaBiblioteka.Adapters;
using DomowaBiblioteka.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DomowaBiblioteka.Provders
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly string _baseBookControllerUri;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HttpClientProvider(IConfiguration configuration)
        {
            var baseWebApiUri = configuration.GetValue<string>("BaseWebApiUri");
            _baseBookControllerUri = $"{baseWebApiUri}api/Book/";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<Book> Add(Book book)
        {
            return await PostResultBookFromWebApi(book);
        }

        public async Task Delete(int id)
        {
            await DeleteBookWebApi(id);
        }

        public async Task<Book> Get(int id)
        {
            return await GetResultBookFromWebApi(id);
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await GetResultBooksFromWebApi();
        }

        public async Task Update(Book bookChanges)
        {
            await PutBookWebApi(bookChanges);
        }

        private HttpClient CreateHttpClient()
        {
            return new HttpClient();
        }

        private async Task<IEnumerable<Book>> GetResultBooksFromWebApi()
        {
            using var httpclient = CreateHttpClient();

            var response = await httpclient.GetAsync(_baseBookControllerUri);

            var contentAsString = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<DomowaBiblioteka.Common.Books.Book>>(contentAsString, _jsonSerializerOptions)
                .Select(BooksAdapter.ConvertFromDto);

            return result;
        }

        private async Task<Book> GetResultBookFromWebApi(int id)
        {
            using var httpclient = CreateHttpClient();

            var response = await httpclient.GetAsync($"{_baseBookControllerUri}{id}");

            var contentAsString = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<DomowaBiblioteka.Common.Books.Book>(contentAsString, _jsonSerializerOptions);

            return BooksAdapter.ConvertFromDto(result);
        }

        private async Task<Book> PostResultBookFromWebApi(Book book)
        {
            var bookDto = BooksAdapter.ConvertToDto(book);
            var bookDtoAsString = JsonSerializer.Serialize(bookDto, _jsonSerializerOptions);

            using var httpclient = CreateHttpClient();

            var htttpContent = new StringContent(bookDtoAsString, Encoding.UTF8, "application/json");

            var response = await httpclient.PostAsync(_baseBookControllerUri, htttpContent);

            var contentAsString = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<DomowaBiblioteka.Common.Books.Book>(contentAsString, _jsonSerializerOptions);

            return BooksAdapter.ConvertFromDto(result);
        }

        private async Task PutBookWebApi(Book book)
        {
            var bookDto = BooksAdapter.ConvertToDto(book);
            var bookDtoAsString = JsonSerializer.Serialize(bookDto, _jsonSerializerOptions);

            using var httpclient = CreateHttpClient();

            var htttpContent = new StringContent(bookDtoAsString, Encoding.UTF8, "application/json");

            await httpclient.PutAsync(_baseBookControllerUri, htttpContent);
        }

        private async Task DeleteBookWebApi(int id)
        {
            using var httpclient = CreateHttpClient();

            await httpclient.DeleteAsync($"{_baseBookControllerUri}{id}");
        }
    }
}
