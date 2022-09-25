using System;

namespace Formuler.Shared.DTO.UserService
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
