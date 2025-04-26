using System.ComponentModel.DataAnnotations;

public class CreateEventDto
{
    [Required(ErrorMessage = "Event Name is required.")]
    [StringLength(20, ErrorMessage = "Event Name cannot be longer than 20 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Event Date is required.")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(200, ErrorMessage = "Location cannot be longer than 200 characters.")]
    public string Location { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Number of guests is required.")]
    [Range(1, 10000, ErrorMessage = "Number of guests must be between 1 and 10,000.")]
    public int NumberOfGuests { get; set; }

    [Required(ErrorMessage = "Manager ID is required.")]
    public int ManagerId { get; set; }

    public List<int> SupplierIds { get; set; } = new List<int>();
}
