using System.ComponentModel.DataAnnotations;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Configuration;

public class MySqlSettings
{
    [Required]
    public string Server { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public uint Port { get; set; }

    [Required]
    public string Database { get; set; }
}