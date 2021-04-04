using Alpha.Models.Role;

namespace Alpha.Models.Login
{
    public class LoginAuthorize
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public RoleModel Role { get; set; }
    }
}