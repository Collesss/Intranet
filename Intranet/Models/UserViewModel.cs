namespace Intranet.Models
{
    public class UserViewModel
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public PhoneUserViewModel[] Phones { get; set; } = new PhoneUserViewModel[0];

        public Dictionary<string, string[]> PhonesNumer { get; set; } = new Dictionary<string, string[]>();
    }
}
