using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.Models;

namespace Higgs.Mbale.BAL.Interface
{
 public   interface IAccountTransactionActivityService
    {
        IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivities();
        AccountTransactionActivity GetAccountTransactionActivity(long accountTransactionActivityId);
        long SaveAccountTransactionActivity(AccountTransactionActivity accountTransactionActivity, string userId);
        void MarkAsDeleted(long accountTransactionActivityId, string userId);

        IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivitiesForAParticularAccount(string accountId);
        IEnumerable<PaymentMode> GetAllPaymentModes();
        IEnumerable<AccountTransactionActivity> MapEFToModel(IEnumerable<EF.Models.AccountTransactionActivity> data);
        PaymentMode GetPaymentMode(long paymentModeId);
        IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivitiesForAParticularSupply(long supplyId);
        bool checkIfSupplyRelatesToAnyAccountTransaction(long supplyId);
        double GetBalanceForLastAccountAccountTransactionActivityForSupplier(string accountId);
        IEnumerable<AccountTransactionActivity> GetAllAdvancedPaymentsForAParticularAspNetUser(string accountId, long transactionSubTypeId);
        double GetBalanceForLastAccountAccountTransactionActivityForSupplierForAParticularDate(string accountId, DateTime dateTime);

        IEnumerable<AccountTransactionActivity> GetLatestFortyAccountTransactionActivitiesForAParticularAccount(string accountId);

       
    }
}
