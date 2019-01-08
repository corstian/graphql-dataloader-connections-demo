using GraphQL.DataLoader;
using GraphQL.Types;
using GraphQL_DataLoader_Connection.Types;

namespace GraphQL_DataLoader_Connection
{
    public class RootQuery : ObjectGraphType<object>
    {
        public RootQuery(IDataLoaderContextAccessor dataLoader)
        {
            /*
             * There are a few things to notice here:
             * - The connections here only return a valid connection object. (So no dataloader business here)
             * - Actual pagination of the final resultset is done in the `ToConnection` extension method
             * - Personally I prefer to handle arguments here, and build a resultset accordingly, and hand this off to the extension method for pagination.
             */

            Connection<CompanyType>()
                .Name("companies")
                .Unidirectional()
                .PageSize(10)
                .ResolveAsync(async context =>
                {
                    return await CompanyRepo.Companies.ToConnection(context);
                });

            Connection<PersonType>()
                .Name("persons")
                .Unidirectional()
                .PageSize(10)
                .ResolveAsync(async context =>
                {
                    return await PersonRepo.Persons.ToConnection(context);
                });
        }
    }
}
