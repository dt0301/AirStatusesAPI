using System.ComponentModel.DataAnnotations;

namespace AirStatusesDomain 
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        public string UserName { get; set; }

        [MaxLength(256)]
        public string Password { get; set; }

        public string Token { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }        
    }
}
