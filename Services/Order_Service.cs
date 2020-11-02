
using BackEnd.Models;
using BackEnd.Models.OnlineShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.Services
{
    public class Order_Service
    {
        private ApplicationDbContext ModelsContext;
        private Address_Service address_Service;
        private Payment_Service payment_Service;
        public Order_Service()
        {
            this.ModelsContext = new ApplicationDbContext();
            this.address_Service = new Address_Service();
            this.payment_Service = new Payment_Service();
        }
        public List<Order> GetOrders()
        {
            return ModelsContext.Orders.ToList();
        }
        public List<Order> GetOrders(string status)
        {
            return ModelsContext.Orders.Where(p => p.status.ToLower() == status.ToLower()).ToList();
        }
        public Order GetOrder(string order_id)
        {
            return ModelsContext.Orders.Find(order_id);
        }
        public List<Order_Item> GetOrderItems(string order_id)
        {
            return GetOrder(order_id).Order_Items.ToList();
        }
        public OrderDetailModel GetOrderDetail(string order_id)
        {
            try
            {
                string shipping_method = "Collect at warehouse", payment_method = "Awaiting Payment";
                if (address_Service.GetShippingAddress(order_id) != null)
                    shipping_method = "Standard delivery";
                if (payment_Service.GetOrderPayment(order_id) != null)
                    payment_method = payment_Service.GetOrderPayment(order_id).PaymentMethod;

                return new OrderDetailModel()
                {
                    customer = GetOrder(order_id).Customer,
                    order = GetOrder(order_id),
                    shipping_method = shipping_method,
                    delivery = null,
                    address = address_Service.GetShippingAddress(order_id),
                    payment_Method = payment_method,
                    payment = payment_Service.GetOrderPayment(order_id),
                    order_items = GetOrderItems(order_id),
                    order_total = (decimal)GetOrderTotal(order_id)
                };
            }
            catch (Exception ex)
            {
                return new OrderDetailModel();
            }
        }
        public List<Order_Tracking> GetOrderTrackingReport(string order_id)
        {
            return ModelsContext.Order_Trackings.Where(x => x.order_ID == order_id).ToList();
        }
        public void MarkOrderAsPacked(string order_id)
        {
            var order = GetOrder(order_id);
            order.packed = true;
            if (ModelsContext.Shipping_Addresses.Where(p => p.Order_ID == order_id) != null)
            {
                order.status = "With courier";
                //order tracking
                ModelsContext.Order_Trackings.Add(new Order_Tracking()
                {
                    order_ID = order.Order_ID,
                    date = DateTime.Now,
                    status = "Order Packed, Now with our courier",
                    Recipient = ""
                });
            }

            ModelsContext.SaveChanges();
        }
        public void AddOrder(Customer customer)
        {
            try
            {
                ModelsContext.Orders.Add(new Order()
                {
                    Order_ID = GenerateOrderNumber(10),
                    Email = customer.Email,
                    date_created = DateTime.Now,
                    shipped = false,
                    status = "Awaiting Payment"
                });
                ModelsContext.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public void AddOrderItems(Order order, List<Cart_Item> items)
        {
            foreach (var item in items)
            {
                var x = new Order_Item()
                {
                    Order_id = order.Order_ID,
                    item_id = item.item_id,
                    quantity = item.quantity,
                    price = item.price
                };
                ModelsContext.Order_Items.Add(x);
                ModelsContext.SaveChanges();
            }
        }
        public void AddOrderTrackingReport(Order_Tracking tracking)
        {
            try
            {
                ModelsContext.Order_Trackings.Add(tracking);
                ModelsContext.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public void EstimateDeliveryDateReport(Order order)
        {
            try
            {
                var expected_Date = DateTime.Now.AddDays(2);
                do
                {
                    expected_Date = expected_Date.AddDays(1);
                } while (expected_Date.DayOfWeek.ToString().ToLower() == "sunday" ||
                    expected_Date.DayOfWeek.ToString().ToLower() == "saturday");

                if (IsDeliveryRequested(order.Order_ID))
                {
                    AddOrderTrackingReport(new Order_Tracking() 
                    {
                        order_ID = order.Order_ID,
                        date = DateTime.Now,
                        status = "Expected delivery on " + expected_Date.ToLongDateString() + " before 5pm",
                        Recipient = ""
                    });
                }
                else
                {
                    expected_Date = DateTime.Now.AddHours(1);

                    AddOrderTrackingReport(new Order_Tracking()
                    {
                        order_ID = order.Order_ID,
                        date = DateTime.Now,
                        status = "Can be collected during business hours as from " + expected_Date.ToLongDateString() + " " + expected_Date.ToLongTimeString(),
                        Recipient = ""
                    });
                }


            }
            catch (Exception ex) { }
        }
        public bool IsDeliveryRequested(string order_id)
        {
            return ModelsContext.Shipping_Addresses.FirstOrDefault(p => p.Order_ID == order_id) != null;
        }
        public void AddPayment(string order_id)
        {
            var order = ModelsContext.Orders.Find(order_id);
            var email = order.Customer.Email;
            try
            {
                //if (IsPaymentComplete(order_id))
                {
                    ModelsContext.Payments.Add(new Payment()
                    {
                        Date = DateTime.Now,
                        Email = email,
                        AmountPaid = GetOrderTotal(order.Order_ID),
                        PaymentFor = "Order " + order_id + " Payment",
                        PaymentMethod = "EFT via PayFast Online",
                        Order_ID = order_id
                    });
                    ModelsContext.SaveChanges();
                    UpdateOrderReport(order_id);
                }
            }
            catch (Exception) { }
        }
        public void UpdateOrderReport(string order_id)
        {
            var order = ModelsContext.Orders.Find(order_id);
            try
            {
                if (IsPaymentComplete(order_id))
                {
                    order.status = "At warehouse";
                    ModelsContext.SaveChanges();
                    //order tracking
                    ModelsContext.Order_Trackings.Add(new Order_Tracking()
                    {
                        order_ID = order.Order_ID,
                        date = DateTime.Now,
                        status = "Payment Recieved | Order still at warehouse",
                        Recipient = ""
                    });
                    ModelsContext.SaveChanges();
                }
            }
            catch (Exception) { }
        }
        public bool IsPaymentComplete(string order_id)
        {
            return ModelsContext.Payments.ToList()
                      .FindAll(x => x.Order_ID == order_id)
                      .Sum(x => x.AmountPaid) >= GetOrderTotal(order_id);
        }
        public double GetOrderTotal(string order_id)
        {
            double amount = 0;
            foreach (var item in ModelsContext.Order_Items.ToList().FindAll(match: x => x.Order_id == order_id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }
        public void Update_Stock(string order_id)
        {
            var order = ModelsContext.Orders.Find(order_id);
            List<Order_Item> items = ModelsContext.Order_Items.ToList().FindAll(x => x.Order_id == order_id);
            foreach (var item in items)
            {
                var product = ModelsContext.Items.Find(item.item_id);
                if (product != null)
                {
                    if ((product.QuantityInStock - item.quantity) >= 0)
                    {
                        product.QuantityInStock -= item.quantity;
                    }
                    else
                    {
                        item.quantity = product.QuantityInStock;
                        product.QuantityInStock = 0;
                    }
                    try
                    {
                        ModelsContext.SaveChanges();
                    }
                    catch (Exception ex) { }
                }
            }
        }
        public string GenerateOrderNumber(int length)
        {
            var random = new Random();
            string number = string.Empty;
            for (int i = 0; i < length; i++)
                number = String.Concat(number, random.Next(10).ToString());
            while (GetOrder(number) != null)
                number = GenerateOrderNumber(length);
            return number;
        }

        public void schedule_OrderDelivery(string order_id, DateTime date)
        {
            var order = GetOrder(order_id);
            order.status = "Scheduled for delivery";
            //order tracking
            ModelsContext.Order_Trackings.Add(new Order_Tracking()
            {
                order_ID = order.Order_ID,
                date = DateTime.Now,
                status = "Scheduled for delivery on " + date.ToLongDateString(),
                Recipient = ""
            });
            ModelsContext.SaveChanges();
        }
    }
}