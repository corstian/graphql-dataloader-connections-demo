using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL_DataLoader_Connection.Models
{
    public class Person : IId
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
    }
}
