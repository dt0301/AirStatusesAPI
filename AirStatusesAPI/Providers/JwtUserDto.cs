namespace AirStatusesAPI.Providers
{
    public class JwtUserDto
    {
        public JwtUserDto(int userId, string userName, string role)
        {
            UserId = userId;
            UserName = userName;
            Role = role;
        }

        public JwtUserDto(){}

        public string UserName { get; private set; }
        public int UserId { get; private set; }
        public string Role{ get; private set; }
    }
}