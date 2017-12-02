using System.Collections.Generic;

namespace Prevea.WebPreveaUI.Security
{
    #region Using

    using System.Security.Principal;
    using System.Linq;

    #endregion

    public class AppPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (Roles.Any(role.Contains))
            {
                return true;
            }

            return false;
        }

        public AppPrincipal(string userName)
        {
            this.Identity = new GenericIdentity(userName);
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; }
    }

    public class AppPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; }
    }
}