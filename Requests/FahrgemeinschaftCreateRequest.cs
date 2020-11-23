using System.ComponentModel.DataAnnotations;

namespace CarPoolApi.Requests
{
    public class FahrgemeinschaftCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}