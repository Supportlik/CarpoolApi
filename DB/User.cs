using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

#nullable disable

namespace CarPoolApi.DB
{
    public partial class User
    {
        public User()
        {
            FahrgemeinschaftMitglieds = new HashSet<FahrgemeinschaftMitglied>();
            Fahrgemeinschafts = new HashSet<Fahrgemeinschaft>();
        }

        public int Id { get; set; }
        public string OauthId { get; set; }
        [Required] public string Vorname { get; set; }
        [Required] public string Nachname { get; set; }
        
        [JsonIgnore] public virtual ICollection<FahrgemeinschaftMitglied> FahrgemeinschaftMitglieds { get; set; }
        
        [JsonIgnore] public virtual ICollection<Fahrgemeinschaft> Fahrgemeinschafts { get; set; }
    }
}