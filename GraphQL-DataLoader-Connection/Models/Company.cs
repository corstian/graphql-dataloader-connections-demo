using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL_DataLoader_Connection.Models
{
    public class Company : IId
    {
        public Company()
        {

        }

        public Company(Guid guid, string name)
        {
            Id = guid;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
