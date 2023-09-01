using CamundaInstance.Domain.Hasura.Contracts;
using CamundaInstance.Domain.Hasura.GraphQL.Queries;
using CamundaInstance.Domain.Hasura.GraphQL.Variables;
using Newtonsoft.Json.Linq;

namespace CamundaInstance.Domain.Hasura
{
    public class HasuraService : IHasuraService
    {
        private readonly IGraphQLRepository _graphQLRepository;

        public HasuraService(IGraphQLRepository graphQLRepository)
        {
            _graphQLRepository = graphQLRepository;
        }
        public async Task<bool> InsertAdminUser(AdminUser adminUser)
        {
            var variable = JObject.FromObject(new
            {
                user = new
                {
                    Id = adminUser.Id,
                    Email = adminUser.Email
                }
            });

            return await _graphQLRepository.Mutation<dynamic>(Mutation.Insert_AdminUser, variable);            
        }
    }
}
