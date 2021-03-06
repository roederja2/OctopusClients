using System;
using System.Linq;
using Octopus.Client.Model;
using Octopus.Client.Model.Endpoints;
using Octopus.Client.Repositories;

namespace Octopus.Client.Editors
{
    public class MachineEditor : IResourceEditor<MachineResource, MachineEditor>
    {
        private readonly IMachineRepository repository;

        public MachineEditor(IMachineRepository repository)
        {
            this.repository = repository;
        }

        public MachineResource Instance { get; private set; }

        public MachineEditor CreateOrModify(
            string name,
            EndpointResource endpoint,
            EnvironmentResource[] environments,
            string[] roles)
        {
            var existing = repository.FindByName(name);
            if (existing == null)
            {
                Instance = repository.Create(new MachineResource
                {
                    Name = name,
                    Endpoint = endpoint,
                    EnvironmentIds = new ReferenceCollection(environments.Select(e => e.Id)),
                    Roles = new ReferenceCollection(roles)
                });
            }
            else
            {
                existing.Name = name;
                existing.Endpoint = endpoint;
                existing.EnvironmentIds.ReplaceAll(environments.Select(e => e.Id));
                existing.Roles.ReplaceAll(roles);

                Instance = repository.Modify(existing);
            }

            return this;
        }

        public MachineEditor CreateOrModify(
            string name,
            EndpointResource endpoint,
            EnvironmentResource[] environments,
            string[] roles,
            TenantResource[] tenants,
            TagResource[] tenantTags)
        {
            var existing = repository.FindByName(name);
            if (existing == null)
            {
                Instance = repository.Create(new MachineResource
                {
                    Name = name,
                    Endpoint = endpoint,
                    EnvironmentIds = new ReferenceCollection(environments.Select(e => e.Id)),
                    Roles = new ReferenceCollection(roles),
                    TenantIds = new ReferenceCollection(tenants.Select(t => t.Id)),
                    TenantTags = new ReferenceCollection(tenantTags.Select(t => t.CanonicalTagName))
                });
            }
            else
            {
                existing.Name = name;
                existing.Endpoint = endpoint;
                existing.EnvironmentIds.ReplaceAll(environments.Select(e => e.Id));
                existing.Roles.ReplaceAll(roles);
                existing.TenantIds.ReplaceAll(tenants.Select(t => t.Id));
                existing.TenantTags.ReplaceAll(tenantTags.Select(t => t.CanonicalTagName));

                Instance = repository.Modify(existing);
            }

            return this;
        }

        //public IEnumerable<MachineResource> BuildSamples<TEndpoint>(
        //    this IMachineRepository repo,
        //    int numberOfDeploymentTargets,
        //    EnvironmentResource[] environments,
        //    string[] roles,
        //    Func<int, string> nameBuilder = null,
        //    Action<TEndpoint> customizeEndpoint = null,
        //    Action<MachineResource> customizeDeploymentTarget = null)
        //    where TEndpoint : EndpointResource, new()
        //{
        //    Log.Information("Building {Count} sample {Endpoint}...", numberOfDeploymentTargets, typeof(TEndpoint).Name);

        //    var namer = nameBuilder ?? (i => RandomStringGenerator.Generate(16));
        //    return Enumerable.Range(1, numberOfDeploymentTargets).Select(i => repository.Build(namer(i), environments, roles, customizeEndpoint, customizeDeploymentTarget));
        //}

        //public MachineResource ForTenant(this MachineResource machine, TenantResource tenant)
        //{
        //    machine.TenantIds.Add(tenant.Id);

        //    return machine;
        //}

        //public MachineResource ForTenantsWithTag(this MachineResource machine, TagResource tag)
        //{
        //    machine.TenantTags.Add(tag.CanonicalTagName);

        //    return machine;
        //}

        public MachineEditor Customize(Action<MachineResource> customize)
        {
            customize?.Invoke(Instance);
            return this;
        }

        public MachineEditor Save()
        {
            Instance = repository.Modify(Instance);
            return this;
        }
    }
}