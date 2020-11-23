using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#nullable disable

namespace CarPoolApi.DB
{
    public partial class Fahrt
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int FahrgemeinschaftId { get; set; }
        public int FahrerId { get; set; }
        
        public virtual FahrgemeinschaftMitglied Fahrer { get; set; }
        
        [JsonIgnore]
        public virtual Fahrgemeinschaft Fahrgemeinschaft { get; set; }
    }
}
