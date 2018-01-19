namespace Prevea.Service.Service
{
    public partial class Service
    {
        public string GetTagValue(string tag)
        {
            return Repository.GetTagValue(tag);
        }
    }
}
