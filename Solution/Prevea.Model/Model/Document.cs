﻿namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Collections.Generic;
    using Helpers;
    using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime? DateModification { get; set; }

        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        public int DocumentNumber { get; set; }

        [Required]
        public int Edition { get; set; }

        public int AreaId { get; set; }
        public virtual Area Area { get; set; }        

        public int DocumentStateId { get; set; }
        public virtual DocumentState DocumentState { get; set; }

        public bool HasFirm { get; set; }
        public bool IsFirmedDocument { get; set; }

        public byte[] Signature { get; set; }

        public string InputTemplatesJSON { get; set; }

        [NotMapped]
        public List<InputTemplate> InputTemplates { get; set; }

        public int? DocumentParentId { get; set; }
        public virtual Document DocumentParent { get; set; }

        public int? DocumentFirmedId { get; set; }
        public virtual Document DocumentFirmed { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int? SimulationId { get; set; }
        public virtual Simulation Simulation { get; set; }

        public virtual ICollection<HistoricDownloadDocument> HistoricDownloadDocuments { get; set; }

        public virtual ICollection<DocumentUserCreator> DocumentUserCreators { get; set; }

        public virtual ICollection<DocumentUserOwner> DocumentUserOwners { get; set; }

        public virtual ICollection<MedicalExaminationDocuments> MedicalExaminationDocuments { get; set; }

        public virtual ICollection<PreventivePlan> PreventivePlans { get; set; }

        #region Constructor

        public Document()
        {
            Date = DateTime.Now;
        }

        #endregion
    }
}
