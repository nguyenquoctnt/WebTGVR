using VDMMutiline.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace VDMMutiline.Core.ShoppingCart
{

    public class ShoppingCart
    {

        private DateTime _dateCreated;
        private DateTime _lastUpdate;
        private List<CartItem> _items;
        public ShoppingCart()
        {
            if (this._items == null)
            {
                this._items = new List<CartItem>();
                this._dateCreated = DateTime.Now;
            }
        }

        public List<CartItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }
        public List<CartItem> Getcart()
        {
            return _items;
        }
        public void Insert(CartItem iteam)
        {
            var objcart = _items.FirstOrDefault(z => z.IDProduct == iteam.IDProduct);
            if (objcart == null)
            {
                var newItem = new CartItem
                {
                    IDProduct = iteam.IDProduct,
                    Mau = iteam.Mau,
                    Productname = iteam.Productname,
                    ProductNumber = iteam.ProductNumber,
                    Price = iteam.Price,
                    Total = iteam.Price * iteam.ProductNumber,
                    ProductImage = iteam.ProductImage
                };
                _items.Add(newItem);
            }
            else
            {
                if (iteam.ProductNumber == 1)
                {
                    objcart.ProductNumber = objcart.ProductNumber + 1;
                }
                else
                    objcart.ProductNumber = iteam.ProductNumber;
                objcart.Total = iteam.Price * objcart.ProductNumber;
            }
            _lastUpdate = DateTime.Now;
        }

        public void Update(CartItem iteam)
        {
            var objcart = _items.FirstOrDefault(z => z.IDProduct == iteam.IDProduct);
            if (objcart != null)
            {
                objcart.ProductNumber = iteam.ProductNumber;
                objcart.Total = objcart.Price * iteam.ProductNumber;
            }
            _lastUpdate = DateTime.Now;
        }

        public void DeleteItem(int productid)
        {
            var obj = _items.FirstOrDefault(z => z.IDProduct == productid);
            if (obj != null)
            {
                _items.Remove(obj);
                _lastUpdate = DateTime.Now;
            }
        }
        public double TotalOrder
        {
            get
            {
                if (_items == null)
                {
                    return 0;
                }
                return _items.Sum(item => item.Total);
            }
        }
    }
}
