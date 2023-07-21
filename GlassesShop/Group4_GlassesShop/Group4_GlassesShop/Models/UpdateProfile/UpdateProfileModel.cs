using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Group4_GlassesShop.Models.UpdateProfile
{
    public class UpdateProfileModel
    {
        [Required(ErrorMessage = "Please enter your name")]
        [MaxLength(50)]

        public string CustomerName { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required.")] 
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must contain exactly 10 digits.")]
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; }
        public UpdateProfileModel()
        {

        }

        public UpdateProfileModel(string name, string phonenumber, string avatar)
        {
            CustomerName = name;
            PhoneNumber = phonenumber;
            AvatarUrl = avatar;
        }

    }

}
