using System;

namespace Octopus.Client.Model
{
    public class ActionTemplateUsageResource : Resource
    {
        public string ActionTemplateId { get; set; }
        public string DeploymentProcessId { get; set; }
        public string ActionId { get; set; }
        public string ActionName { get; set; }
        public string StepId { get; set; }
        public string StepName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectSlug { get; set; }
        public string Version { get; set; }
    }
}