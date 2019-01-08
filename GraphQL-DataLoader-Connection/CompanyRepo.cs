using GraphQL_DataLoader_Connection.Models;
using System;
using System.Collections.Generic;

namespace GraphQL_DataLoader_Connection
{
    /// <summary>
    /// Just ignore this class. It's only some mock data.
    /// </summary>
    public static class CompanyRepo
    {
        static CompanyRepo()
        {
            Companies = new List<Company>()
            {
                new Company(new Guid("d2a9f9ba-6ea6-439c-9d53-80ae28da5a0e"), "CHOAM"),
                new Company(new Guid("2163549a-5b13-4b64-a083-c9e1ff46eb80"), "Acme Corporation"),
                new Company(new Guid("6aa03c51-4ef2-458a-92c0-5f54e36eb437"), "Sirius Cybernetics Corporation"),
                new Company(new Guid("45639e53-272c-4cec-9971-61e4abc3d142"), "MomCorp"),
                new Company(new Guid("444eb0ef-374b-4c57-80ab-2121881d10fe"), "Rich Industries"),
                new Company(new Guid("e8b4b6fa-0d1c-4fff-9cdb-05b5ae2edf5f"), "Soylent Corporation"),
                new Company(new Guid("79bd6786-d8b4-4ac4-977a-54570926ff53"), "Very Big Corporation of America"),
                new Company(new Guid("09080058-bbcc-4caf-94b2-dd1e8e81b31c"), "Frobozz Magic Corporation"),
                new Company(new Guid("835c1ee3-2307-4cb7-9bd5-4fb1d690925b"), "Warbucks Industries"),
                new Company(new Guid("b33a1cff-d7b3-4c87-924e-4f63d051fe6a"), "Tyrell Corporation"),
                new Company(new Guid("527da740-a7c4-4a34-a9b5-fc132771ba89"), "Wayne Enterprises"),
                new Company(new Guid("32cb04ef-2e84-4764-8d76-0347b6aca836"), "Virtucon")
            };

            Companies.ForEach(q => q.Id = Guid.NewGuid());
        }

        public static List<Company> Companies { get; set; }
        
    }
}
