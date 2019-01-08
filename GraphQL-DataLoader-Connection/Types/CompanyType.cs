using System;
using System.Linq;
using GraphQL.DataLoader;
using GraphQL.Relay.Types;
using GraphQL_DataLoader_Connection.Models;

namespace GraphQL_DataLoader_Connection.Types
{
    public class CompanyType : NodeGraphType<Company>
    {
        public CompanyType(IDataLoaderContextAccessor dataLoader)
        {
            Field(q => q.Name);

            Connection<PersonType>()
                .Name("persons")
                .Unidirectional()
                .PageSize(10)
                .ResolveAsync(async context =>
                {
                    var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<Guid, Person>("personLoader", PersonRepo.PersonsByCompanyId);

                    // IMPORTANT: In order to avoid deadlocking on the loader we use the following construct (next 2 lines):
                    var loadHandle = loader.LoadAsync(context.Source.Id);
                    var result = await loadHandle;

                    return await result.ToConnection(context);
                });
        }

        public override Company GetById(string id)
        {
            return CompanyRepo.Companies.FirstOrDefault(q => q.Id == id.FromCursor());
        }
    }
}
