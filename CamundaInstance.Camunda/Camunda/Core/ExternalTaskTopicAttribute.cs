﻿namespace CamundaInstance.Camunda.Camunda.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExternalTaskTopicAttribute : Attribute
    {
        public string Topic { get; set; }
        public ExternalTaskTopicAttribute(string topic)
        {
            Topic = topic;
        }
    }
}
