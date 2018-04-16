namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Collections.Generic;
    using Helpers;

    #endregion

    public class Document
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string UrlRelative { get; set; }
        public string FileName => Path.GetFileName(UrlRelative);
        public string Extension => Path.GetExtension(UrlRelative);

        public string Name => $"{Area.Name}_{DocumentNumber:00000}_{Edition}";

        public string Icon => HelperClass.GetExension(UrlRelative);

        [Required]
        public string Description { get; set; }
        public string Observations { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public DateTime DateModification { get; set; }

        [Required]
        public int DocumentNumber { get; set; }

        [Required]
        public int Edition { get; set; }

        public int AreaId { get; set; }
        public virtual Area Area { get; set; }        

        public int DocumentStateId { get; set; }
        public virtual DocumentState DocumentState { get; set; }

        public bool HasFirm { get; set; }

        public int? DocumentParentId { get; set; }
        public virtual Document DocumentParent { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
                       
        public virtual ICollection<HistoricDownloadDocument> HistoricDownloadDocuments { get; set; }

        public virtual ICollection<DocumentUserCreator> DocumentUserCreators { get; set; }

        public virtual ICollection<DocumentUserOwner> DocumentUserOwners { get; set; }

        #region Constructor

        public Document()
        {
            Date = DateTime.Now;
        }

        #endregion
    }
}
