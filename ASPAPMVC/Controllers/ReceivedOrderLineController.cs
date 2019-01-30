using ASPAPMVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPAPMVC.Controllers
{
    public class ReceivedOrderLineController : Controller
    {
        private OrderLineExposer _exposer;
        private OrderExposer _orderExposer;
        public ReceivedOrderLineController()
        {
            _exposer = new OrderLineExposer();
            _orderExposer = new OrderExposer();
        }
        // GET: ReceivedOrderLine
        public ActionResult Overview(string Order_UID)
        {
            List<OrderLine> _orderLine;

            if (Order_UID == null)
            {
                var sOrder_UID = TempData["Order_UID"].ToString();
                _orderLine = _exposer.getOrderLine(sOrder_UID);

            }
            else
            {
                _orderLine = _exposer.getOrderLine(Order_UID);
                TempData["Order_UID"] = Order_UID;


            }

            return View(_orderLine);
        }

        public ActionResult OverviewOld(string Order_UID)
        {
            List<OrderLine> _orderLine;

            if (Order_UID == null)
            {
                var sOrder_UID = TempData["Order_UID"].ToString();
                _orderLine = _exposer.getOrderLineOld(sOrder_UID);

            }
            else
            {
                _orderLine = _exposer.getOrderLineOld(Order_UID);
                TempData["Order_UID"] = Order_UID;


            }

            return View(_orderLine);
        }


        public ActionResult ReceivedOrderLine(string iOrderLine_UID)
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReceivedOrderLine(string iOrderLine_UID, FormCollection formCollection)
        {
            var sOrder_UID = TempData["Order_UID"].ToString();
            RecievedOrderLine recievedOrderLine = new RecievedOrderLine();
            OrderLine orderLine = _exposer.getOrderLineSingle(iOrderLine_UID);
            recievedOrderLine.iOrderLine_UID = Guid.Parse(iOrderLine_UID);
            recievedOrderLine.iOrder_UID = Guid.Parse(sOrder_UID);
            recievedOrderLine.iOrderLine_OrderBrutto = orderLine.iOrderLine_OrderBrutto;
            recievedOrderLine.iOrderLine_OrderNetto = orderLine.iOrderLine_OrderNetto;          
            recievedOrderLine.ProductVariation_UID = orderLine.ProductVariation_UID;
            recievedOrderLine.BoxType_UID = orderLine.BoxType_UID;
            recievedOrderLine.iOrderLine_DeliveryLotNr = formCollection["iOrderLine_DeliveryLotNr"].ToString();
            recievedOrderLine.iOrderLine_RecivedNetto = Convert.ToDecimal(formCollection["iOrderLine_RecivedNetto"].ToString(), CultureInfo.InvariantCulture);
            recievedOrderLine.iOrderLine_RecivedBrutto = Convert.ToDecimal(formCollection["iOrderLine_RecivedBrutto"].ToString(), CultureInfo.InvariantCulture);
            recievedOrderLine.iOrderLine_Pallets = int.Parse(formCollection["iOrderLine_Pallets"].ToString());
            recievedOrderLine.iOrderLine_Colli = int.Parse(formCollection["iOrderLine_Colli"].ToString());
            recievedOrderLine.iOrderLine_Temperature = Convert.ToDecimal(formCollection["iOrderLine_Temperature"].ToString(), CultureInfo.InvariantCulture);
            _exposer.updateReceiveLine(recievedOrderLine);
            return RedirectToAction("ReceivedOrderLineControl", new { iOrderLine_UID = iOrderLine_UID });
        }

        public ActionResult ReceivedOrderLineControl(string iOrderLine_UID)
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReceivedOrderLineControl(string iOrderLine_UID, FormCollection formCollection)
        {
            var sOrder_UID = TempData["Order_UID"].ToString();
            RecievedOrderLine recievedOrderLine = new RecievedOrderLine();
            OrderLine orderLine = _exposer.getOrderLineSingle(iOrderLine_UID);
            recievedOrderLine.iOrderLine_UID = Guid.Parse(iOrderLine_UID);
            recievedOrderLine.iOrder_UID = Guid.Parse(sOrder_UID);
            recievedOrderLine.iOrderLine_OrderBrutto = orderLine.iOrderLine_OrderBrutto;
            recievedOrderLine.iOrderLine_OrderNetto = orderLine.iOrderLine_OrderNetto;
            recievedOrderLine.ProductVariation_UID = orderLine.ProductVariation_UID;
            recievedOrderLine.BoxType_UID = orderLine.BoxType_UID;
            recievedOrderLine.iOrderLine_DeliveryLotNr = formCollection["iOrderLine_DeliveryLotNr"].ToString();
            recievedOrderLine.iOrderLine_RecivedNetto = orderLine.iOrderLine_RecivedNetto;
            recievedOrderLine.iOrderLine_RecivedBrutto = orderLine.iOrderLine_RecivedBrutto;
            recievedOrderLine.iOrderLine_Pallets = orderLine.iOrderLine_Pallets;
            recievedOrderLine.iOrderLine_Colli = orderLine.iOrderLine_Colli;
            recievedOrderLine.iOrderLine_Temperature = orderLine.iOrderLine_Temperature;
            recievedOrderLine.iOrderLine_ErrSize = formCollection["iOrderLine_ErrSize"].ToString();
            recievedOrderLine.iOrderLine_ErrForm = formCollection["iOrderLine_ErrForm"].ToString();
            recievedOrderLine.iOrderLine_ErrColor = formCollection["iOrderLine_ErrColor"].ToString();
            recievedOrderLine.iOrderLine_InsectDamage = formCollection["iOrderLine_InsectDamage"].ToString();
            recievedOrderLine.iOrderLine_FungiDamage = formCollection["iOrderLine_FungiDamage"].ToString();
            recievedOrderLine.iOrderLine_PhysicalDamage = formCollection["iOrderLine_PhysicalDamage"].ToString();
            recievedOrderLine.iOrderLine_Note = formCollection["iOrderLine_Note"].ToString();
            var MyBoolValue = Convert.ToBoolean(formCollection["Organic"].ToString().Split(',')[0]);
            if (MyBoolValue)
            {
                recievedOrderLine.iOrderLine_Organic = 1;
            }
            else
            {
                recievedOrderLine.iOrderLine_Organic = 0;
            }
            _exposer.updateControlReceiveLine(recievedOrderLine);
            return RedirectToAction("Overview", new { Order_UID = iOrderLine_UID });
        }
        // GET: ReceivedOrderLine/Create
        public ActionResult Create()
        {
            var Order_UID = TempData["Order_UID"].ToString();
            Order _order = _orderExposer.getOrder(Order_UID);
            OrderLine orderLine = new OrderLine();
            orderLine.iOrder_ID = _order.iOrder_ID;
            List<BoxType> _boxTypes = _exposer.getBoxTypes();
            List<Product_ProductVariation> _productProductVars = _exposer.getProductProductVar();
            List<SelectListItem> boxList = new List<SelectListItem>();
            List<SelectListItem> ppvList = new List<SelectListItem>();
            foreach (var box in _boxTypes)
            {
                boxList.Add(new SelectListItem
                {
                    Text = box.BoxType_Name,
                    Value = box.BoxType_UID.ToString()
                });
            }
            foreach (var ppv in _productProductVars)
            {
                ppvList.Add(new SelectListItem
                {
                    Text = ppv.Product_Name + " " + ppv.ProductVariation_Name,
                    Value = ppv.ProductVariation_UID.ToString()
                });
            }
            ViewBag.Boxes = boxList;
            ViewBag.PpvList = ppvList;
            ViewData["specificOrder"] = orderLine;
            TempData["Order_UID"] = Order_UID;
            return View();
        }

        // POST: ReceivedOrderLine/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            OrderLine _orderLine = new OrderLine();
            var Order_UID = TempData["Order_UID"].ToString();
            _orderLine.iOrder_UID = Guid.Parse(Order_UID);
            _orderLine.iOrderLine_DeliveryLotNr = collection["iOrderLine_DeliveryLotNr"].ToString();
            _orderLine.ProductVariation_UID = Guid.Parse(collection["PpvList"].ToString());
            _orderLine.BoxType_UID = Guid.Parse(collection["Boxtype"].ToString());
            _orderLine.iOrderLine_OrderNetto = Convert.ToDecimal(collection["iOrderLine_OrderNetto"].ToString(), CultureInfo.InvariantCulture);
            _orderLine.iOrderLine_OrderBrutto = Convert.ToDecimal(collection["iOrderLine_OrderBrutto"].ToString(), CultureInfo.InvariantCulture);
            _orderLine.iOrderLine_Pallets = int.Parse(collection["iOrderLine_Pallets"].ToString());
            var MyBoolValue = Convert.ToBoolean(collection["Organic"].ToString().Split(',')[0]);
            if (MyBoolValue)
            {
                _orderLine.iOrderLine_Organic = 1;
            }
            else
            {
                _orderLine.iOrderLine_Organic = 0;
            }
            _exposer.addUpdateOrderLine(_orderLine);
            TempData["Order_UID"] = Order_UID;
            return RedirectToAction("Overview", new { Order_UID = _orderLine.iOrder_UID.ToString() });
        }

        // GET: ReceivedOrderLine/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReceivedOrderLine/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ReceivedOrderLine/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReceivedOrderLine/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
