using System.ComponentModel.DataAnnotations;

namespace EventMangerServerApi.Core.Dtos
{
    public class UpdateSupplierDto
    {
        [Required(ErrorMessage = "Supplier ID is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Supplier Name is required.")]
        [StringLength(100, ErrorMessage = "Supplier Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [EnumDataType(typeof(SupplierCategory), ErrorMessage = "Invalid supplier category.")]
        public SupplierCategory Category { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }
    }
}
