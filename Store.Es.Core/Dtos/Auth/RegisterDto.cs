using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Dtos.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="Email is Required!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "DisplayName is Required!")]

        public string DisplayName { get; set; }
        [Required(ErrorMessage = "Password is Required!")]

        public string Password {  get; set; }
        [Required(ErrorMessage = "PhoneNumber is Required!")]

        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Addresss is Required!")]

        public AddressDto Address { get; set; }
    }
}
