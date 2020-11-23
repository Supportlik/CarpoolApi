using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CarPoolApi.DB
{
    public partial class Fahrgemeinschaft
    {
        public Fahrgemeinschaft()
        {
            FahrgemeinschaftMitglieds = new HashSet<FahrgemeinschaftMitglied>();
            Fahrts = new HashSet<Fahrt>();
        }

        public int Id { get; set; }
        
        public int CreatorId { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual User Creator { get; set; }
        public virtual ICollection<FahrgemeinschaftMitglied> FahrgemeinschaftMitglieds { get; set; }
        public virtual ICollection<Fahrt> Fahrts { get; set; }
    }
}
