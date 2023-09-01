namespace CamundaInstance.Domain.Hasura.GraphQL.Queries
{
    public static class Mutation
    {
        public const string Insert_AdminUser = @"
                mutation ($user: AdminUsers_insert_input!) {
                    insert_AdminUsers(objects: [$user]) {
                        affected_rows
                    }
                }
            ";
    }
}
