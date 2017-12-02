namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class UserRole
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]        
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
