namespace Intranet.Repository.Entities
{
    public class PhoneEntity : BaseEntity<int>
    {
        public int UserId { get; set; }

        public UserEntity User { get; set; }

        public string Type { get; set; }

        public string PhoneNumbers { get; set; }
    }
}
