namespace Prevea.Model.ViewModel
{
    #region Using

    using System;

    #endregion

    public class IncidenceViewModel
    {
        public int Id { get; set; }

        public string Screen { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        public int UserId { get; set; }
        public string UserInitials { get; set; }

        public int IncidenceStateId { get; set; }
        public string IncidenceStateDescription { get; set; }
    }
}
