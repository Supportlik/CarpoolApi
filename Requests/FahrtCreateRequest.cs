using System;
using System.ComponentModel.DataAnnotations;

namespace CarPoolApi.Requests
{
    public class FahrtCreateRequest
    {
        [Required] public int FahrerId { get; set; }
        [Required] public DateTime Date { get; set; }
    }
}