using Alpha.Models.Role;

namespace Alpha.Models.User
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string  Patronymic { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public RoleModel Role { get; set; }
    }
}