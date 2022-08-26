using Core.Repository;

namespace UserService.Data.Models
{
    public class UserEntity : MongoEntity
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
