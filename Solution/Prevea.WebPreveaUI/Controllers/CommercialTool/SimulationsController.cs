namespace Prevea.WebPreveaUI.Controllers.CommercialTool
{
    #region Using

    using System.Web.Mvc;

    #endregion

    public class SimulationsController : Controller
    {
        // GET: Simulations
        public ActionResult Simulations()
        {
            return PartialView();
        }
    }
}