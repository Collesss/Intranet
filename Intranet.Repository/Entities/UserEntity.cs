﻿namespace Intranet.Repository.Entities
{
    public class UserEntity : BaseEntity<int>
    {
        public string SID { get; set; }

        public string UserPrincipalName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public PhoneUserEntity[] Phones { get; set; }
    }
}
