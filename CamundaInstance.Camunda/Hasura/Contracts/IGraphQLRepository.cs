namespace CamundaInstance.Domain.Hasura.Contracts
{
    public interface IGraphQLRepository
    {
        Task<bool> Mutation<T>(string query, object variables);
    }
}
