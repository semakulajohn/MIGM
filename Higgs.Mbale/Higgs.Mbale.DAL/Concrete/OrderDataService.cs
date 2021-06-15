using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Higgs.Mbale.EF.Models;
using Higgs.Mbale.DAL.Concrete;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.EF.UnitOfWork;
using Higgs.Mbale.DTO;
using log4net;
using EntityFramework.Extensions;

namespace Higgs.Mbale.DAL.Concrete
{
    public class OrderDataService : DataServiceBase, IOrderDataService
    {
        ILog logger = log4net.LogManager.GetLogger(typeof(OrderDataService));

        public OrderDataService(IUnitOfWork<MbaleEntities> unitOfWork)
            : base(unitOfWork)
        {

        }

        public IEnumerable<Order> GetAllOrders()
        {
            return this.UnitOfWork.Get<Order>().AsQueryable()
                .Where(e => e.Deleted == false);
        }

        public IEnumerable<Order> GetAllOrdersForAParticularBranch(long branchId)
        {
            return this.UnitOfWork.Get<Order>().AsQueryable().Where(e => e.Deleted == false && e.BranchId == branchId);
        }


        public IEnumerable<Order> GetAllOrdersForAParticularCustomer(string customerId)
        {
            return this.UnitOfWork.Get<Order>().AsQueryable().Where(e => e.Deleted == false && e.CustomerId == customerId).OrderByDescending(e => e.CreatedOn).Take(20);
        }

        public IEnumerable<Order> GetAllCompletedOrdersForAParticularCustomer(string customerId,long statusId)
        {
            return this.UnitOfWork.Get<Order>().AsQueryable().Where(e => e.Deleted == false && e.CustomerId == customerId && e.StatusId == statusId).OrderByDescending(e => e.CreatedOn).Take(20);
        }
        public IEnumerable<Order> GetAllOpenOrdersForAParticularCustomer(string customerId, long statusId)
        {
            return this.UnitOfWork.Get<Order>().AsQueryable().Where(e => e.Deleted == false && e.CustomerId == customerId && e.StatusId == statusId).OrderByDescending(e => e.CreatedOn).Take(20);
        }
        public Order GetOrder(long orderId)
        {
            return this.UnitOfWork.Get<Order>().AsQueryable()
                 .FirstOrDefault(c =>
                    c.OrderId == orderId &&
                    c.Deleted == false
                );
        }

        /// <summary>
        /// Saves a new Order or updates an already existing Order.
        /// </summary>
        /// <param name="orderDTO">Order to be saved or updated.</param>
        /// <param name="userId">userId of the Order creating or updating</param>
        /// <returns>orderId</returns>
        public long SaveOrder(OrderDTO orderDTO, string userId)
        {
            long orderId = 0;

            if (orderDTO.OrderId == 0)
            {
                var order = new Order()
                {
                    Amount = orderDTO.Amount,
                    ProductId = orderDTO.ProductId,
                    StatusId = orderDTO.StatusId,
                    BranchId = orderDTO.BranchId,                   
                    CustomerId = orderDTO.CustomerId,                  
                    CreatedOn = DateTime.Now,
                    Balance = orderDTO.Amount,
                    TimeStamp = DateTime.Now,
                    Price = orderDTO.Price,
                    CreatedBy = userId,
                    Deleted = false,
                };

                this.UnitOfWork.Get<Order>().AddNew(order);
                this.UnitOfWork.SaveChanges();
                orderId = order.OrderId;
                

                
                return orderId;
            }

            else
            {
                var result = this.UnitOfWork.Get<Order>().AsQueryable()
                    .FirstOrDefault(e => e.OrderId == orderDTO.OrderId);
                if (result != null)
                {
                    result.OrderId = orderDTO.OrderId;
                    result.CustomerId = orderDTO.CustomerId;
                    result.Amount = orderDTO.Amount;
                    result.ProductId = orderDTO.ProductId;                    
                    result.BranchId = orderDTO.BranchId;
                    result.Balance = orderDTO.Amount;
                    result.StatusId = orderDTO.StatusId;
                    result.Price = orderDTO.Price;
                    result.UpdatedBy = userId;
                    result.TimeStamp = DateTime.Now;
                    this.UnitOfWork.Get<Order>().Update(result);
                    this.UnitOfWork.SaveChanges();
                }
                return orderDTO.OrderId;
            }
        }

        public void MarkAsDeleted(long orderId, string userId)
        {
           
            PurgeOrderGradeSize(orderId);
            using (var dbContext = new MbaleEntities())
            {
                //TODO: THROW NOT IMPLEMENTED EXCEPTION
                dbContext.Mark_Order_AsDeleted(orderId, userId);
            }

        }

        public void SaveOrderGradeSize(OrderGradeSizeDTO orderGradeSizeDTO)
        {
            var orderGradeSize = new OrderGradeSize()
            {
                OrderId = orderGradeSizeDTO.OrderId,
                GradeId = orderGradeSizeDTO.GradeId,
                SizeId = orderGradeSizeDTO.SizeId,
                Quantity = orderGradeSizeDTO.Quantity,
                Balance = orderGradeSizeDTO.Quantity,
                TimeStamp = DateTime.Now,
                Price = orderGradeSizeDTO.Price
                
            };
            this.UnitOfWork.Get<OrderGradeSize>().AddNew(orderGradeSize);
            this.UnitOfWork.SaveChanges();
        }

        public void PurgeOrderGradeSize(long orderId)
        {
            this.UnitOfWork.Get<OrderGradeSize>().AsQueryable()
                .Where(m => m.OrderId == orderId)
                .Delete();
        }

        public void UpdateOrderWithCompletedStatus(long orderId, long statusId,double balance, string userId)
        {
            using (var dbContext = new MbaleEntities())
            {
                dbContext.UpdateOrderWithCompletedStatus(orderId, statusId,balance, userId);
            }

        }

        public void UpdateOrderGradeSizes(long orderId, long gradeId, long sizeId,double quantity,double balance)
        {
            using (var dbContext = new MbaleEntities())
            {
                dbContext.UpdateOrderGradeSizes(orderId, gradeId,sizeId,quantity, balance);
            }

        }

        public void UpdateOrderWithInProgressStatus(long orderId, long statusId,double balance, string userId)
        {
            using (var dbContext = new MbaleEntities())
            {
                dbContext.UpdateOrderWithInProgressStatus(orderId, statusId,balance, userId);
            }

        }
        public void UpdateOrderWithBalance(long orderId, double balance, string userId)
        {
            using (var dbContext = new MbaleEntities())
            {
                dbContext.UpdateOrderWithBalanceQuantity(orderId, balance, userId);
            }

        }

        //public GetOrderToDeliver_Result GetOrderToDeliver(long orderId)
        //{

        //    using (var dbContext = new MbaleEntities())
        //    {
        //        var result = dbContext.GetOrderToDeliver(orderId);
        //        return result.FirstOrDefault();
        //    }

        //}
    }
}
