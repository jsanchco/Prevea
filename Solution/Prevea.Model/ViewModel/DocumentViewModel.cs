﻿namespace Prevea.Model.ViewModel
{
    #region Using    

    using System;

    #endregion

    public class DocumentViewModel
    {
        public int Id { get; set; }

        public string UrlRelative { get; set; }
        public string Description { get; set; }
        public string Observations { get; set; }
        public string Name { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string AreaUrl { get; set; }
        public string Icon { get; set; }
        public bool UpdateFile { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DateModification { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Edition { get; set; }
        public int DocumentStateId { get; set; }
        public int DocumentUserCreatorId { get; set; }
        public string DocumentUserCreatorName { get; set; }
        public bool HasFirm { get; set; }
        public bool IsFirmedDocument { get; set; }
    }
}
