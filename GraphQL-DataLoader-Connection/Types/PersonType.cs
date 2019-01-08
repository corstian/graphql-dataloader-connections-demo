using GraphQL.Relay.Types;
using GraphQL_DataLoader_Connection.Models;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL_DataLoader_Connection.Types
{
    public class PersonType : NodeGraphType<Person>
    {
        public PersonType()
        {
            Field(q => q.FirstName);
            Field(q => q.LastName);
            Field(q => q.Gender);
            Field(q => q.City);

            Field<CompanyType>()
                .Name("company")
                .ResolveAsync(async context =>
                {
                    await Task.Delay(8);    // Network latency
                    return CompanyRepo.Companies.FirstOrDefault(q => q.Id == context.Source.CompanyId);
                });
        }

        public override Person GetById(string id)
        {
            return PersonRepo.Persons.FirstOrDefault(q => q.Id == id.FromCursor());
        }
    }
}
