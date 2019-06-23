using CsvHelper;
using CsvHelper.Configuration;
using GraphQL_DataLoader_Connection.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQL_DataLoader_Connection
{
    /// <summary>
    /// Just ignore this class. It's only some mock data.
    /// </summary>
    public static class PersonRepo
    {
        static PersonRepo()
        {
            using (var reader = new StreamReader("Resources\\FakeNameGenerator.com_1eeb3d1a.csv"))
            using (var csv = new CsvReader(reader, new Configuration(CultureInfo.InvariantCulture)))
            {
                var randomizer = new Random();

                var records = csv.GetRecords<dynamic>();
                Persons = records.Select(q => new Person
                {
                    Id = new Guid(q.GUID),
                    CompanyId = CompanyRepo.Companies[randomizer.Next(0, 12)].Id,
                    FirstName = q.GivenName,
                    LastName = q.Surname,
                    Gender = q.Gender,
                    City = q.City
                }).ToList();
            }
        }

        public static List<Person> Persons { get; set; }

        public static async Task<ILookup<Guid, Person>> PersonsByCompanyId(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            await Task.Delay(8);    // Just simulate some network response time
            return Persons
                .Where(q => ids.Contains(q.CompanyId))
                .ToLookup(q => q.CompanyId);
        }
    }
}
