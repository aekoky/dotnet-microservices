using Formuler.Core.Repository;
using System;

namespace UserService.Data.Models
{
    public class AccountEntity : MongoEntity
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Salt { get; set; }
    }
}
