﻿namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    #endregion

    public class Area
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public string Url { get; set; }

        public bool StoreInServer { get; set; }

        [Required]
        public int EntityId { get; set; }
        public virtual Entity Entity { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
