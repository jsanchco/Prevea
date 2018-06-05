namespace Prevea.Model.Model
{
    #region Using

    using System;
    using System.ComponentModel.DataAnnotations;

    #endregion

    public class Incidence
    {
        [Key]
        public int Id { get; set; }

        public string Screen { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int IncidenceStateId { get; set; }
        public virtual IncidenceState IncidenceState { get; set; }
        public int CriticalNivelId { get; set; }
        public virtual CriticalNivel CriticalNivel { get; set; }
    }
}
