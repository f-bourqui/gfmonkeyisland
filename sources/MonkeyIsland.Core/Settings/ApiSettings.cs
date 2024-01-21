using System.ComponentModel.DataAnnotations;

namespace MonkeyIsland.Core.Settings;

public class ApiSettings
{
    [Required]
    public string Url { get; set; } = "";
    [Required]
    public string Key { get; set; } = "";
    [Required]
    public string CallbackUrl { get; set; } = "";
}