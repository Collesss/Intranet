namespace Intranet.Repository.Entities
{
    public class PhoneUserEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UserEntity User { get; set; }

        public string Type { get; set; }

        public string PhoneNumbers { get; set; }
    }
}
