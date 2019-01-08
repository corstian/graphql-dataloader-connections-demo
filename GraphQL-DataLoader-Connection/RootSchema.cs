using GraphQL;
using GraphQL.Types;

namespace GraphQL_DataLoader_Connection
{
    public class RootSchema : Schema
    {
        public RootSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<RootQuery>();
        }
    }
}
