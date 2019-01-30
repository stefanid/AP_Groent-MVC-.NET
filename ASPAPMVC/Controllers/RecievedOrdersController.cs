using ASPAPMVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPAPMVC.Controllers
{
    public class RecievedOrdersController : Controller
    {
        private OrderExposer _exposer;
        private SupplierExposer _supplierExposer;
        public RecievedOrdersController()
        {
            _supplierExposer = new SupplierExposer();
            _exposer = new OrderExposer();
        }
        // GET: RecievedOrders
        public ActionResult Overview()
        {
            List<Order> _recievedOrders = _exposer.getRecievedOrderLineWithoutOrderLines();
            return View(_recievedOrders);
        }


        // GET: RecievedOrders/Details/5
        public ActionResult Details(string sOrder_UID)
        {
            return RedirectToAction("Overview", "ReceivedOrderLine", new { Order_UID = sOrder_UID });
        }

        // GET: RecievedOrders/Create
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
            Order order = _exposer.AddUpdateRecievedOrder(_newOrder);

            return RedirectToAction("Overview", "ReceivedOrderLine", new { Order_UID = order.iOrder_UID.ToString() });

        }


        // GET: RecievedOrders/Edit/5
        public ActionResult Edit(string sOrder_UID)
        {
            List<Supplier> _suppliers = _supplierExposer.GetSuppliers();
            Order _sepcificOrder = _exposer.getOrder(sOrder_UID);
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
        [HttpPost]
        public ActionResult Edit(string sOrder_UID, FormCollection collection)
        {

            Order _editOrder = new Order();
            _editOrder.iOrder_UID = Guid.Parse(sOrder_UID);
            _editOrder.iOrder_ID = Convert.ToInt32(collection["data.iOrder_ID"].ToString());
            _editOrder.Supplier_UID = Guid.Parse(collection["Supplier"].ToString());
            _editOrder.iOrder_SupplierRef = collection["data.iOrder_SupplierRef"].ToString();
            var deliveryDate = DateTime.ParseExact(collection["data.StringDeliveryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
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
            _exposer.AddUpdateRecievedOrder(_editOrder);
           
            return RedirectToAction("Overview");
        }

        // GET: RecievedOrders/Delete/5
        public ActionResult Delete(string sOrder_UID)
        {
            var delete = _exposer.deleteOrder(sOrder_UID);
            if (delete)
            {
                return RedirectToAction("Overview");
            }
            else
            {
                return View();
            }

        }



    }
}
