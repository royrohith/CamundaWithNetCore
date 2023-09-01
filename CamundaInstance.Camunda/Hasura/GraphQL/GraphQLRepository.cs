using CamundaInstance.Domain.Hasura.Contracts;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Options;

namespace CamundaInstance.Domain.Hasura.GraphQL
{
    public class GraphQLRepository : IGraphQLRepository
    {
        #region Private fields
        private readonly GraphQLHttpClient _client;
        private readonly HasuraSettings _settings;
        #endregion

        #region Constructor
        public GraphQLRepository(IOptions<HasuraSettings> hasuraSettings)
        {
            _settings = hasuraSettings.Value;
            var httpClient = new HttpClient();

            _client = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_settings.Url),
            },
            new NewtonsoftJsonSerializer(), httpClient);
            _client.HttpClient.DefaultRequestHeaders.Add("x-hasura-admin-secret", _settings.AdminSecret);
        }
        #endregion

        #region Public methods
        public async Task<bool> Mutation<T>(string query, object variables)
        {
            var graphQLRequest = new GraphQLRequest(query, variables);
            var response = await _client.SendMutationAsync<dynamic>(graphQLRequest);
            if (response.Errors is not null)
            {
                Console.WriteLine($"Errors: {response.Errors[0].Message}");
            }
            return response?.Errors is null;
        }
        #endregion
    }
}
