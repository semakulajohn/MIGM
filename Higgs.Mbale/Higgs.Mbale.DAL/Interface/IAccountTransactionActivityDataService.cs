using System;
using System.Collections.Generic;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
    public interface IAccountTransactionActivityDataService
    {
        AccountTransactionActivity GetAccountTransactionActivity(long accountTransactionActivityId);
        IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivities();

        IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivitiesForAParticularAspNetUser(string accountId);

        IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivitiesForAParticularCasualWorker(long accountId);
        
        long SaveAccountTransactionActivity(AccountTransactionActivityDTO accountTransactionActivityDTO, string userId);

        void MarkAsDeleted(long accountTransactionActivityId, string userId);
        AccountTransactionActivity GetLatestAccountTransactionActivityForAParticularAspNetUser(string accountId);
        AccountTransactionActivity GetLatestAccountTransactionActivitiesForAParticularCasualWorker(long casualWorkerId);
        IEnumerable<PaymentMode> GetAllPaymentModes();
        PaymentMode GetPaymentMode(long paymentModeId);
        IEnumerable<AccountTransactionActivity> GetAllAccountTransactionActivitiesForAParticularSupply(long supplyId);

        IEnumerable<AccountTransactionActivity> GetAllAdvancedPaymentsForAParticularAspNetUser(string accountId, long transactionSubTypeId);
        IEnumerable<AccountTransactionActivity> GetLatestFortyAccountTransactionActivitiesForAParticularAspNetUser(string accountId);

        IEnumerable<AccountTransactionActivity> GetLatestFortyAccountTransactionActivitiesForAParticularCasualWorker(long accountId);
        AccountTransactionActivity GetLatestAccountTransactionActivityForAParticularAspNetUserForAParticularDate(string accountId, DateTime dateTime);
    }
}
