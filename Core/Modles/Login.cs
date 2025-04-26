using System.ComponentModel.DataAnnotations;

namespace EventMangerServerApi.Core.Modles
{
    public class Login
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        // תעודת זהות - אם מדובר במנהל אירוע
        public string? IdNumber { get; set; }

        // אם יש צורך לאמת שזהו מנהל אירוע
        public bool IsEventManager { get; set; }

        // סוג המשתמש, אם זה אדמין או לקוח רגיל
        public string Role { get; set; }
    }
}
