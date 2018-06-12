using System.ComponentModel.DataAnnotations;

namespace Prevea.WebTestSignaturePad.Models
{
    public class MyDummyModel
    {
        [UIHint("SignaturePad")]
        public byte[] MySignature { get; set; }
    }
}