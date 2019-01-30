using ASPAPMVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPAPMVC.Controllers
{
    public class OrderLineController : Controller
    {
        private OrderLineExposer _exposer;
        private OrderExposer _orderExposer;
        public OrderLineController()
        {
            _exposer = new OrderLineExposer();
            _orderExposer = new OrderExposer();
        }
        // GET: OrderLine
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

        // GET: OrderLine/Create
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

        // POST: OrderLine/Create
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

        // GET: OrderLine/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderLine/Edit/5
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

        // GET: OrderLine/Delete/5
        public ActionResult Delete(string iOrderLine_UID)
        {
            var delete = _exposer.deleteOrderLine(iOrderLine_UID);
            if (delete)
            {
                var sOrder_UID = TempData["Order_UID"].ToString();
                return RedirectToAction("Overview", new { Order_UID = sOrder_UID });
            }
            else
            {
                return View();
            }

        }





    }
}
