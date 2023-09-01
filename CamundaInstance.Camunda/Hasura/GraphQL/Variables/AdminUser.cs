using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamundaInstance.Domain.Hasura.GraphQL.Variables
{
    public class AdminUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool IsHasuraCallRequired { get; set; }

    }
}
