﻿namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class ContactPerson
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int ContactPersonTypeId { get; set; }
        public virtual ContactPersonType ContactPersonType { get; set; }
    }
}
