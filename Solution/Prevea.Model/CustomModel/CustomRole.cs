namespace Prevea.Model.CustomModel
{
    #region Using

    using System;
    using Model;

    #endregion

    public class CustomRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public string RoleDescription => Helpers.HelperClass.GetDescription(Enum.GetName(typeof(EnRole), RoleId));
    }
}
