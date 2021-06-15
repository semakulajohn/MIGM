using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.DTO;
using Higgs.Mbale.EF.Models;

namespace Higgs.Mbale.DAL.Interface
{
    public interface IOrderDataService
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrder(long orderId);
        long SaveOrder(OrderDTO order, string userId);
        void MarkAsDeleted(long orderId, string userId);  
        IEnumerable<Order> GetAllOrdersForAParticularBranch(long branchId);
        IEnumerable<Order> GetAllOrdersForAParticularCustomer(string customerId);
       // void SaveOrderGrade(OrderGradeDTO orderGradeDTO);
        void SaveOrderGradeSize(OrderGradeSizeDTO orderGradeSizeDTO);
        void PurgeOrderGradeSize(long orderId);
        void UpdateOrderWithCompletedStatus(long orderId, long statusId,double balance, string userId);
        IEnumerable<Order> GetAllCompletedOrdersForAParticularCustomer(string customerId, long statusId);
        IEnumerable<Order> GetAllOpenOrdersForAParticularCustomer(string customerId, long statusId);
        void UpdateOrderWithInProgressStatus(long orderId, long statusId,double balance, string userId);
        void UpdateOrderWithBalance(long orderId, double balance, string userId);
        void UpdateOrderGradeSizes(long orderId, long gradeId, long sizeId, double quantity, double balance);

    }
}
