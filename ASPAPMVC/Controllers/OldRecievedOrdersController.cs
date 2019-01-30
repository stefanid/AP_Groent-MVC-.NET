using ASPAPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPAPMVC.Controllers
{
    public class OldRecievedOrdersController : Controller
    {
        private OrderExposer _exposer;
        public OldRecievedOrdersController()
        {
            _exposer = new OrderExposer();
        }
        // GET: OldRecievedOrders
        public ActionResult Overview()
        {

            List<Order> _recievedOrders = _exposer.getRecievedOrderLineWithOrderLines();
            return View(_recievedOrders);
        }

        // GET: OldRecievedOrders/Details/5
        public ActionResult Details(string sOrder_UID)
        {
            return RedirectToAction("OverviewOld", "ReceivedOrderLine", new { Order_UID = sOrder_UID });
        }

        // GET: OldRecievedOrders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OldRecievedOrders/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OldRecievedOrders/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OldRecievedOrders/Edit/5
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

        // GET: OldRecievedOrders/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OldRecievedOrders/Delete/5
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
