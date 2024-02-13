namespace Intranet.Models
{
    public class UserViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public PhoneUserViewModel[] Phones { get; set; }
    }
}
