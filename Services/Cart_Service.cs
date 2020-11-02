
using BackEnd.Models;
using BackEnd.Models.OnlineShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.Services
{
    public class Cart_Service
    {
        private ApplicationDbContext ModelsContext;
        public static string shoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";
        public Cart_Service()
        {
            this.ModelsContext = new ApplicationDbContext();
        }

        public void AddItemToCart(int id)
        {
            shoppingCartID = GetCartID();

            var item = ModelsContext.Items.Find(id);
            if (item != null)
            {
                var cartItem =
                    ModelsContext.Cart_Items.FirstOrDefault(x => x.cart_id == shoppingCartID && x.item_id == item.ItemCode);
                if (cartItem == null)
                {
                    var cart = ModelsContext.Carts.Find(shoppingCartID);
                    if (cart == null)
                    {
                        ModelsContext.Carts.Add(entity: new Cart()
                        {
                            cart_id = shoppingCartID,
                            date_created = DateTime.Now
                        });
                        ModelsContext.SaveChanges();
                    }

                    ModelsContext.Cart_Items.Add(entity: new Cart_Item()
                    {
                        cart_item_id = Guid.NewGuid().ToString(),
                        cart_id = shoppingCartID,
                        item_id = item.ItemCode,
                        quantity = 1,
                        price = item.Price
                    }
                        );
                }
                else
                {
                    cartItem.quantity++;
                }
                ModelsContext.SaveChanges();
            }
        }
        public void RemoveItemFromCart(string id)
        {
            shoppingCartID = GetCartID();

            var item = ModelsContext.Cart_Items.Find(id);
            if (item != null)
            {
                var cartItem =
                    ModelsContext.Cart_Items.FirstOrDefault(predicate: x => x.cart_id == shoppingCartID && x.item_id == item.item_id);
                if (cartItem != null)
                {
                    ModelsContext.Cart_Items.Remove(entity: cartItem);
                }
                ModelsContext.SaveChanges();
            }
        }
        public List<Cart_Item> GetCartItems()
        {
            shoppingCartID = GetCartID();
            return ModelsContext.Cart_Items.ToList().FindAll(match: x => x.cart_id == shoppingCartID);
        }
        public void UpdateCart(string id, int qty)
        {
            var item = ModelsContext.Cart_Items.Find(id);
            if (qty < 0)
                item.quantity = qty / -1;
            else if (qty == 0)
                RemoveItemFromCart(item.cart_item_id);
            else if (item.Item.QuantityInStock < qty)
                item.quantity = item.Item.QuantityInStock;
            else
                item.quantity = qty;
            ModelsContext.SaveChanges();
        }
        public double GetCartTotal(string id)
        {
            double amount = 0;
            foreach (var item in ModelsContext.Cart_Items.ToList().FindAll(match: x => x.cart_id == id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }
        public void EmptyCart()
        {
            shoppingCartID = GetCartID();
            foreach (var item in ModelsContext.Cart_Items.ToList().FindAll(match: x => x.cart_id == shoppingCartID))
            {
                ModelsContext.Cart_Items.Remove(item);
            }
            try
            {
                ModelsContext.Carts.Remove(ModelsContext.Carts.Find(shoppingCartID));
                ModelsContext.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public string GetCartID()
        {
            if (System.Web.HttpContext.Current.Session[name: CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(value: System.Web.HttpContext.Current.User.Identity.Name))
                {
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid temp = Guid.NewGuid();
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = temp.ToString();
                }
            }
            return System.Web.HttpContext.Current.Session[name: CartSessionKey].ToString();
        }
    }
}