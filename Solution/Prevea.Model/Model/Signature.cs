namespace Prevea.Model.Model
{
    #region Using

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    [NotMapped]
    public class Signature
    {
        public int? DocumentId { get; set; }
        public int? UserId { get; set; }

        [UIHint("SignaturePad")]
        public byte[] MySignature { get; set; }
    }
}
