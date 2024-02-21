namespace Intranet.Repository.Entities
{
    public class PhoneUserEntity : BaseEntity<int>
    {
        public int UserId { get; set; }

        public UserEntity User { get; set; }

        public string Type { get; set; }

        public string PhoneNumbers { get; set; }
    }
}
