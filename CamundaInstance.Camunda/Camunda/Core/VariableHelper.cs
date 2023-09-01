using Camunda.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamundaInstance.Domain.Camunda.Core
{
    public static class VariableHelper
    {
        public static Dictionary<string, object> ToObjectDictionary(this IDictionary<string, VariableValue> source)
        {
            var result = new Dictionary<string, object>();

            foreach (var (key, value) in source)
            {
                result.Add(key, value);
            }

            return result;
        }
    }
}
