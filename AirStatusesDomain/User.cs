using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirStatusesDomain
{
    /// <summary>
    /// Класс User представляет пользователя в домене AirStatusesDomain.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя, который генерируется базой данных.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя, которое не может превышать 256 символов.
        /// </summary>
        [MaxLength(256)]
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя, который не может превышать 256 символов.
        /// </summary>
        [MaxLength(256)]
        public string Password { get; set; }

        /// <summary>
        /// Токен пользователя, который может быть null.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Идентификатор роли пользователя.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Роль пользователя.
        /// </summary>
        public virtual Role Role { get; set; }
    }
}
