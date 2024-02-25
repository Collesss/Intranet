namespace Intranet.Api.Dto.User.Response
{
    public class UserResponseDto
    {
        public int Id { get; set; }

        public string SID { get; set; }

        public string UserPrincipalName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        //public IEnumerable<> Phones { get; set; }
    }
}
