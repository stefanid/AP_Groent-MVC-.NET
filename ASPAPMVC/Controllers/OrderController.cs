using ASPAPMVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPAPMVC.Controllers
{
    public class OrderController : Controller
    {
        private OrderExposer _exposer;
        private SupplierExposer _supplierExposer;
        public OrderController()
        {
            _exposer = new OrderExposer();
            _supplierExposer = new SupplierExposer();
        }
        // GET: Order
        public ActionResult Overview()
        {
            List<Order> _orders = _exposer.getTodaysOrder();

            return View(_orders);
        }

        // GET: Order/Details/5
        public ActionResult Details(string Order_UID)
        {
            return RedirectToAction("Overview", "OrderLine", new { Order_UID = Order_UID });
        }

        // GET: Order/Create
        public ActionResult Create()
        {

            List<Supplier> _suppliers = _supplierExposer.GetSuppliers();
            List<SelectListItem> itemsSupplier = new List<SelectListItem>();
            List<SelectListItem> orderStatus = new List<SelectListItem>();
            IEnumerable<ShippingStatus> values =

                     Enum.GetValues(typeof(ShippingStatus))

                     .Cast<ShippingStatus>();

            foreach (var supplier in _suppliers)
            {
                itemsSupplier.Add(new SelectListItem
                {
                    Text = supplier.Supplier_Name,
                    Value = supplier.Supplier_UID.ToString()
                });
            }

            foreach (var status in values)
            {
                orderStatus.Add(new SelectListItem
                {
                    Text = status.ToString(),
                    Value = (Convert.ToInt32(status)).ToString()
                });
            }


            ViewBag.Supplier = itemsSupplier;
            ViewBag.ShippingStatus = orderStatus;
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {

            Order _newOrder = new Order();
            _newOrder.Supplier_UID = Guid.Parse(collection["Supplier"].ToString());
            _newOrder.iOrder_ID = Convert.ToInt32(collection["iOrder_ID"].ToString());
            _newOrder.iOrder_SupplierRef = collection["iOrder_SupplierRef"].ToString();
            var deliveryDate = DateTime.ParseExact(collection["StringDeliveryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"); 
            _newOrder.iOrder_DeliveryDate = int.Parse(deliveryDate.ToString());
            var MyBoolValue = Convert.ToBoolean(collection["Organic"].ToString().Split(',')[0]);
            if (MyBoolValue)
            {
                _newOrder.iOrder_Organic = 1;
            }
            else
            {
                _newOrder.iOrder_Organic = 0;
            }

            _newOrder.iOrder_ConfirmationShipped = Convert.ToInt32(collection["ShippingStatus"].ToString());
            Order order = _exposer.AddUpdateOrder(_newOrder);

            return RedirectToAction("Overview", "OrderLine", new { Order_UID = order.iOrder_UID.ToString() });


        }

        // GET: Order/Edit/5
        public ActionResult Edit(string Order_UID)
        {

            List<Supplier> _suppliers = _supplierExposer.GetSuppliers();
            Order _sepcificOrder = _exposer.getOrder(Order_UID);
            List<SelectListItem> itemsSupplier = new List<SelectListItem>();
            List<SelectListItem> orderStatus = new List<SelectListItem>();
            IEnumerable<ShippingStatus> values =

                     Enum.GetValues(typeof(ShippingStatus))

                     .Cast<ShippingStatus>();

            IEnumerable<OrderStatus> valuesOrderDelivery =

                     Enum.GetValues(typeof(OrderStatus))

                     .Cast<OrderStatus>();
            foreach (var supplier in _suppliers)
            {

                itemsSupplier.Add(new SelectListItem
                {
                    Text = supplier.Supplier_Name,
                    Value = supplier.Supplier_UID.ToString()
                });
            }

            foreach (var status in values)
            {
                if (status.ToString() == _sepcificOrder.ShipStatus.ToString())
                {
                    orderStatus.Add(new SelectListItem
                    {
                        Text = status.ToString(),
                        Value = (Convert.ToInt32(status)).ToString(),
                        Selected = true
                    });

                }
                else
                {
                    orderStatus.Add(new SelectListItem
                    {
                        Text = status.ToString(),
                        Value = (Convert.ToInt32(status)).ToString()
                    });
                }

            }

            ViewBag.Supplier = itemsSupplier;
            ViewBag.ShippingStatus = orderStatus;
            ViewData["specificOrder"] = _sepcificOrder;
            return View();
        }

        // POST: Order/Edit/5
        [HttpPost]
        public ActionResult Edit(string Order_UID, FormCollection collection)
        {

            Order _editOrder = new Order();
            _editOrder.iOrder_UID = Guid.Parse(Order_UID);
            _editOrder.iOrder_ID = Convert.ToInt32(collection["data.iOrder_ID"].ToString());
            _editOrder.Supplier_UID = Guid.Parse(collection["Supplier"].ToString());
            _editOrder.iOrder_SupplierRef = collection["data.iOrder_SupplierRef"].ToString();
            var deliveryDate = DateTime.Parse(collection["data.StringDeliveryDate"].ToString()).ToString("yyyyMMdd");
            _editOrder.iOrder_DeliveryDate = int.Parse(deliveryDate);
            bool MyBoolValue = Convert.ToBoolean(collection["data.Organic"].ToString().Split(',')[0]);
            if (MyBoolValue)
            {
                _editOrder.iOrder_Organic = 1;
            }
            else
            {
                _editOrder.iOrder_Organic = 0;
            }
            _editOrder.iOrder_ConfirmationShipped = Convert.ToInt32(collection["ShippingStatus"].ToString());
            _exposer.AddUpdateOrder(_editOrder);
            // TODO: Add update logic here

            if (_editOrder.iOrder_StatusID == 10)
            {
                return RedirectToAction("Overview");
            }
            else
            {
                return RedirectToAction("Overview", "RecievedOrders");
            }





        }

        // GET: Order/Delete/5
        public ActionResult Delete(string Order_UID)
        {


            var delete = _exposer.deleteOrder(Order_UID);
            if (delete)
            {
                return RedirectToAction("Overview");
            }
            else
            {
                return View();
            }


        }

        // POST: Order/Delete/5

    }
}
