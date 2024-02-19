using System.ComponentModel.DataAnnotations;

namespace AirStatusesDomain 
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        public string Code { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
