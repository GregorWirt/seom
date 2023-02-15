using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Seom.Application.Model
{
    [Index(nameof(Name), IsUnique = true)]
    public class Store
    {
        public Store(string name, string street, int zip, string city)
        {
            Name = name;
            Street = street;
            Zip = zip;
            City = city;
        }

#pragma warning disable CS8618
        protected Store() { }
#pragma warning restore CS8618
        public int Id { get; private set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public int Zip { get; set; }
        public string City { get; set; }
        public List<Offer> Offers { get; } = new();
    }
}
