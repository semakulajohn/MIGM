using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Higgs.Mbale.Branch._classes
{
    public sealed class SessionSingleton
    {
        #region Singleton

        private const string SESSION_SINGLETON_NAME = "Singleton_4B314E79-D665-4BA2-ADE3-A696798573DC";

        private SessionSingleton()
        {

        }

        public static SessionSingleton Current
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_SINGLETON_NAME] == null)
                {
                    HttpContext.Current.Session[SESSION_SINGLETON_NAME] = new SessionSingleton();
                }

                return HttpContext.Current.Session[SESSION_SINGLETON_NAME] as SessionSingleton;
            }
        }

        #endregion

        public string BranchId { get; set; }
    }
}