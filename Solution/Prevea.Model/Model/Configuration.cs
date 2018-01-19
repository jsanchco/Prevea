namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class Configuration
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Tag { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
