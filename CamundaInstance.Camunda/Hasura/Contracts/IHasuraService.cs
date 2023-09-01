using CamundaInstance.Domain.Hasura.GraphQL.Variables;

namespace CamundaInstance.Domain.Hasura.Contracts
{
    public interface IHasuraService
    {
        Task<bool> InsertAdminUser(AdminUser adminUser);
    }
}
