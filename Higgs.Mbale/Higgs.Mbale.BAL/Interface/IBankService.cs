using System.Collections.Generic;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
public    interface IBankService
    {
        IEnumerable<Bank> GetAllBanks();
        Bank GetBank(long bankId);
        long SaveBank(Bank bank, string userId);
        void MarkAsDeleted(long bankId, string userId);
    }
}
