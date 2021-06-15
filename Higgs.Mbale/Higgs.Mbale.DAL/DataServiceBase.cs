using Higgs.Mbale.EF.Models;
using Higgs.Mbale.EF.UnitOfWork;

namespace Higgs.Mbale.DAL
{
 public   class DataServiceBase
    {

        private IUnitOfWork<MbaleEntities> _unitOfwork;

        protected IUnitOfWork<MbaleEntities> UnitOfWork { get { return this._unitOfwork; } }

        public DataServiceBase(IUnitOfWork<MbaleEntities> unitOfWork)
        {
            this._unitOfwork = unitOfWork;
        }
    }
}
