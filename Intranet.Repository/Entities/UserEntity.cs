namespace Intranet.Repository.Entities
{
    public class UserEntity : BaseEntity<string>
    {
        public string UserPrincipalName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string WorkPhone { get; set; }
    }
}
