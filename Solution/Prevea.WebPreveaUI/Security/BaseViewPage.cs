namespace Prevea.WebPreveaUI.Security
{
    #region Using

    using System.Web.Mvc;

    #endregion

    public abstract class BaseViewPage : WebViewPage
    {
        public new virtual AppPrincipal User => base.User as AppPrincipal;
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public new virtual AppPrincipal User => base.User as AppPrincipal;
    }
}