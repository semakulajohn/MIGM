
using Higgs.Mbale.Interfaces;

namespace Higgs.Mbale.Branch._classes
{
    public class WebContext
    {

        #region Properties

        /// <summary>
        /// Caching implementation
        /// </summary>
        public ICache Cache { get; set; }

        #endregion

        /// <summary>
        /// Set all app context settigs for site
        /// </summary>
        /// <param name="configSettings"></param>
        /// <param name="cache"></param>
        public WebContext(ICache cache)
        {
            Cache = cache;
        }
    }
}