namespace Prevea.Model.ViewModel
{
    #region Using

    using System;

    #endregion

    public class TemplatePreventivePlanViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Template { get; set; }
    }
}
