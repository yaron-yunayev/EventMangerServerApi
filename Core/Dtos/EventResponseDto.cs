using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventMangerServerApi.Core.Dtos
{
    public class EventResponseDto
    {
        [Required]
        public int Id { get; set; }

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

        public ManagerDto   Manager { get; set; }

        [Required(ErrorMessage = "At least one supplier is required.")]
        public List<int> SupplierIds { get; set; } = new List<int>();
        public List<SupplierDto> Suppliers { get; set; } = new List<SupplierDto>();
    }

    public class ManagerDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        public string LastName { get; set; }
    }
    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }
}
