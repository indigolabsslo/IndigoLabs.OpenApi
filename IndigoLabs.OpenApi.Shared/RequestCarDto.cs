using System.ComponentModel.DataAnnotations;

namespace IndigoLabs.OpenApi.Shared
{
  /// <summary>
  /// A vehicle with 4 wheels.
  /// </summary>
  public class RequestCarDto
  {
    /// <summary>
    /// How much?
    /// </summary>
    [Required]
    [Range(100, 200000)]
    public int Price { get; set; }

    /// <summary>
    /// AKA name.
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Choose a better model name.")]
    public string Model { get; set; }

    /// <summary>
    /// Rainbows everywhere.
    /// </summary>
    public string Color { get; set; }
  }
}
