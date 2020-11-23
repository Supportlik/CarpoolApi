using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

#nullable disable

namespace CarPoolApi.DB
{
    public partial class FahrgemeinschaftMitglied
    {
        public FahrgemeinschaftMitglied()
        {
            Fahrts = new HashSet<Fahrt>();
        }

        public int Id { get; set; }
        public int FahrgemeinschaftId { get; set; }
        public int UserId { get; set; }

        public int FahrtCount
        {
            get { return Fahrgemeinschaft.Fahrts.Count(i => i.FahrerId == Id); }
        }
        
        [JsonIgnore]
        public virtual Fahrgemeinschaft Fahrgemeinschaft { get; set; }
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ICollection<Fahrt> Fahrts { get; set; }
    }
}
