using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirStatusesDomain
{
    /// <summary>
    /// Класс Role представляет роль пользователя в домене AirStatusesDomain.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Идентификатор роли, который генерируется базой данных.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Код роли, который не может превышать 256 символов.
        /// </summary>
        [MaxLength(256)]
        public string Code { get; set; }

        /// <summary>
        /// Коллекция пользователей, связанных с данной ролью.
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
    }
}
