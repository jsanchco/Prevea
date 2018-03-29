namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    #endregion

    [NotMapped]
    public class InputTemplate
    {
        #region Constructor

        public InputTemplate()
        {
            DataSource = new List<string>();
        }

        #endregion

        public string Name { get; set; }
        public int Type { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }               
        public int DefaultValue { get; set; }
        public string DefaultText { get; set; }
        public List<string> DataSource { get; set; }
    }

    public enum EnInputTemplateType { Input = 1, Single, Multiple, DateTime, TextArea }
}
