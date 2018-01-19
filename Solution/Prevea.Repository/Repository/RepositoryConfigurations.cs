namespace Prevea.Repository.Repository
{
    #region Using

    using System.Linq;

    #endregion

    public partial class Repository
    {
        public string GetTagValue(string tag)
        {
            tag = tag.Trim().ToLower();

            return Context.Configurations.FirstOrDefault(x => x.Tag == tag)?.Value;
        }
    }
}
