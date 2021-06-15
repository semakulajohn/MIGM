using System;
using System.Collections.Generic;
using System.Linq;
using Higgs.Mbale.DTO;
using Higgs.Mbale.BAL.Interface;
using Higgs.Mbale.DAL.Interface;
using Higgs.Mbale.Models;
using Higgs.Mbale.Helpers;
using log4net;

namespace Higgs.Mbale.BAL.Concrete
{
    public class OrderService: IOrderService
    {
        
        ILog logger = log4net.LogManager.GetLogger(typeof(OrderService));
        private IOrderDataService _dataService;
        private IUserService _userService;
        private IGradeService _gradeService;
        

        public OrderService(IOrderDataService dataService,IUserService userService, IGradeService gradeService)
        {
            this._dataService = dataService;
            this._userService = userService;
            this._gradeService = gradeService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order GetOrder(long orderId)
        {
            var result = this._dataService.GetOrder(orderId);
            return MapEFToModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Order> GetAllOrders()
        {
            var results = this._dataService.GetAllOrders();
            return MapEFToModel(results);
        }

        public IEnumerable<Order> GetAllOrdersForAParticularBranch(long branchId)
        {
            var results = this._dataService.GetAllOrdersForAParticularBranch(branchId);
            return MapEFToModel(results);
        }

        public IEnumerable<Order> GetAllOrdersForAParticularCustomer(string customerId)
        {
            var results = this._dataService.GetAllOrdersForAParticularCustomer(customerId);
            return MapEFToModel(results);
        }

        public IEnumerable<Order> GetAllCompletedOrdersForAParticularCustomer(string customerId, long statusId)
        {
            var results = this._dataService.GetAllCompletedOrdersForAParticularCustomer(customerId, statusId);
            return MapEFToModel(results);
        }
        public IEnumerable<Order> GetAllOpenOrdersForAParticularCustomer(string customerId, long statusId)
        {
            var results = this._dataService.GetAllOpenOrdersForAParticularCustomer(customerId, statusId);
            return MapEFToModel(results);
        }

        public long SaveOrder(Order order, string userId)
        {
            var orderDTO = new DTO.OrderDTO()
            {
                Amount = order.Amount,
                OrderId = order.OrderId,
                ProductId = order.ProductId,
                StatusId = order.StatusId,
                BranchId = order.BranchId,
                CustomerId = order.CustomerId,  
                Deleted = order.Deleted,
                CreatedBy = order.CreatedBy,
                CreatedOn = order.CreatedOn,
                Price = order.Price,
            };

           var orderId = this._dataService.SaveOrder(orderDTO, userId);
           if (order.Grades != null)
           {
               if (order.Grades.Any())
               {
                   List<OrderGradeSize> orderGradeSizeList = new List<OrderGradeSize>();

                   foreach (var grade in order.Grades)
                   {
                       var gradeId = grade.GradeId;
                       if (grade.Denominations != null)
                       {
                           if(grade.Denominations.Any())
                           {
                               foreach (var denomination in grade.Denominations)
                               {
                                   var orderGradeSize = new OrderGradeSize()
                                   {
                                       GradeId = gradeId,
                                       SizeId = denomination.DenominationId,
                                       OrderId = orderId,
                                       Quantity = denomination.Quantity,
                                       TimeStamp = DateTime.Now,
                                       Price = denomination.Price,
                                   };
                                   orderGradeSizeList.Add(orderGradeSize);
                               }
                           }
                       }
                   }
                   this._dataService.PurgeOrderGradeSize(orderId);                  
                   this.SaveOrderGradeSizeList(orderGradeSizeList);
               }
           }
           return orderId;
                      
        }


        public void UpdateOrderWithCompletedStatus(long orderId, long statusId,double balance, string userId)
        {
            _dataService.UpdateOrderWithCompletedStatus(orderId, statusId,balance, userId);
        }

        public void UpdateOrderWithInProgressStatus(long orderId, long statusId,double balance, string userId)
        {
            _dataService.UpdateOrderWithInProgressStatus(orderId, statusId,balance, userId);
        }
        public void UpdateOrderWithBalance(long orderId, double balance, string userId)
        {
            _dataService.UpdateOrderWithBalance(orderId, balance, userId);
        }

       

        public void UpdateOrderGradeSizes(long orderId, long gradeId,long sizeId,double quantity, double balance)
        {
            _dataService.UpdateOrderGradeSizes(orderId, gradeId,sizeId,quantity, balance);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        public void MarkAsDeleted(long orderId, string userId)
        {
            _dataService.MarkAsDeleted(orderId, userId);
        }

        public void PurgeOrderGradeSize(long orderId)
        {
            this._dataService.PurgeOrderGradeSize(orderId);
        }
     public   void SaveOrderGradeSizeList(List<OrderGradeSize> orderGradeSizeList)
        {
            if (orderGradeSizeList != null)
            {
                if (orderGradeSizeList.Any())
                {
                    foreach (var orderGradeSize in orderGradeSizeList)
                    {
                        var orderGradeSizeDTO = new DTO.OrderGradeSizeDTO()
                        {
                            OrderId = orderGradeSize.OrderId,
                            GradeId = orderGradeSize.GradeId,
                            Quantity = orderGradeSize.Quantity,
                            SizeId = orderGradeSize.SizeId,
                            TimeStamp = orderGradeSize.TimeStamp,
                            Price = orderGradeSize.Price,
                        };
                        this.SaveOrderGradeSize(orderGradeSizeDTO);
                    }
                }
            }
        }
        void SaveOrderGradeSize(OrderGradeSizeDTO orderGradeSizeDTO)
        {
            _dataService.SaveOrderGradeSize(orderGradeSizeDTO);
        }
      
        #region Mapping Methods

        public IEnumerable<Order> MapEFToModel(IEnumerable<EF.Models.Order> data)
        {
            var list = new List<Order>();
            foreach (var result in data)
            {
                list.Add(MapEFToModel(result));
            }
            return list;
        }

        /// <summary>
        /// Maps Order EF object to Order Model Object and
        /// returns the Order model object.
        /// </summary>
        /// <param name="result">EF Order object to be mapped.</param>
        /// <returns>Order Model Object.</returns>
        public Order MapEFToModel(EF.Models.Order data)
        {
            if (data != null)
            {

                var order = new Order()
                {
                    CustomerName = _userService.GetUserFullName(data.AspNetUser1),
                    Amount = data.Amount,
                    ProductId = data.ProductId,
                    ProductName = data.Product != null ? data.Product.Name : "",
                    BranchName = data.Branch != null ? data.Branch.Name : "",
                    BranchId = data.BranchId,
                    StatusId = data.StatusId,
                    OrderId = data.OrderId,
                    StatusName = data.Status != null ? data.Status.Name : "",
                    CreatedOn = data.CreatedOn,
                    TimeStamp = data.TimeStamp,
                    Balance = data.Balance,
                    Price = data.Price,
                    Deleted = data.Deleted,
                    CreatedBy = _userService.GetUserFullName(data.AspNetUser),
                    UpdatedBy = _userService.GetUserFullName(data.AspNetUser2)
                };

                if (data.OrderGradeSizes != null)
                {

                    if (data.OrderGradeSizes.Any())
                    {
                        List<Grade> grades = new List<Grade>();
                        var distinctGrades = data.OrderGradeSizes.Where(a=>a.OrderId == data.OrderId).GroupBy(g => g.GradeId).Select(o => o.First()).ToList();
                        foreach (var orderGradeSize in distinctGrades)
                        {
                            var grade = new Grade()
                            {
                                GradeId = orderGradeSize.Grade.GradeId,
                                Value = orderGradeSize.Grade.Value,
                                CreatedOn = orderGradeSize.Grade.CreatedOn,
                                TimeStamp = orderGradeSize.Grade.TimeStamp,
                                Deleted = orderGradeSize.Grade.Deleted,
                                
                                CreatedBy = _userService.GetUserFullName(orderGradeSize.Grade.AspNetUser),
                                UpdatedBy = _userService.GetUserFullName(orderGradeSize.Grade.AspNetUser1),
                            };
                            List<Denomination> denominations = new List<Denomination>();
                            if (orderGradeSize.Grade.OrderGradeSizes != null)
                            {
                                if (orderGradeSize.Grade.OrderGradeSizes.Any())
                                {
                                    var distinctSizes = orderGradeSize.Grade.OrderGradeSizes.Where(a=>a.OrderId == data.OrderId).GroupBy(s => s.SizeId).Select(o => o.First()).ToList();
                                    //var distinctSizes = orderGradeSize.Grade.OrderGradeSizes.GroupBy(s => s.OrderId ).Select(s => s.First()).ToList();
                                    foreach (var ogs in distinctSizes)
                                    {
                                        var denomination = new Denomination()
                                        {
                                            DenominationId = ogs.SizeId,
                                            Value = ogs.Size != null ? ogs.Size.Value : 0,
                                            Quantity = ogs.Quantity,
                                            Balance =Convert.ToDouble(ogs.Balance),
                                            Price = Convert.ToDouble(ogs.Price),
                                        };
                                        order.TotalQuantity += (ogs.Quantity * ogs.Size.Value);
                                        denominations.Add(denomination);
                                    }
                                }
                                grade.Denominations = denominations;
                            }
                            grades.Add(grade);
                        }
                        order.Grades = grades;
                    }
                }

                return order;
            }
            return null;
        }



       #endregion
    }
}
