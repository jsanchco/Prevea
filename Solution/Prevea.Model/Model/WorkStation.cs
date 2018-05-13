namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    #endregion

    public class WorkStation
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string ProfessionalCategory { get; set; }

        public string Description { get; set; }

        public int CnaeId { get; set; }
        public virtual Cnae Cnae { get; set; }

        public virtual ICollection<RiskEvaluation> RiskEvaluations { get; set; }
    }
}
