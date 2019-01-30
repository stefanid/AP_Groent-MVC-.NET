using ASPAPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPAPMVC.Controllers
{
    public class SuppliersController : Controller
    {
        private SupplierExposer _exposer;
        public SuppliersController()
        {
            _exposer = new SupplierExposer();
        }
        // GET: Suppliers
        public ActionResult Overview()
        {
            List<Supplier> _suppliers = _exposer.GetSuppliers();
            return View(_suppliers);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            List<Country> _countries = _exposer.getCountries();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var country in _countries)
            {
                items.Add(new SelectListItem
                {
                    Text = country.Country_Name,
                    Value = country.Country_UID
                });
            }
            ViewBag.Country = items;
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                Supplier newSupplier = new Supplier();
                newSupplier.Country_UID = Guid.Parse(collection["Country"].ToString());
                newSupplier.Supplier_Name = collection["Supplier_Name"];
                var _createdSupplier = _exposer.AddUpdateSupplier(newSupplier);
                return RedirectToAction("Overview");
            }
            catch
            {
                return View();
            }
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(string Supplier_UID)
        {
            List<Country> _countries = _exposer.getCountries();
            List<SelectListItem> items = new List<SelectListItem>();
          
            Supplier _supplier = new Supplier();

            _supplier = _exposer.getSupplier(Supplier_UID);
            foreach (var country in _countries)
            {
                if (Guid.Parse(country.Country_UID) == _supplier.Country_UID)
                {
                    items.Add(new SelectListItem
                    {
                        Text = country.Country_Name,
                        Value = country.Country_UID,
                        Selected = true
                    });
                }
                else
                {
                    items.Add(new SelectListItem
                    {
                        Text = country.Country_Name,
                        Value = country.Country_UID
                    });

                }

            }
            ViewBag.Country = items;
            ViewData["SupplierName"] = _supplier;
            return View();
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        public ActionResult Edit(string Supplier_UID, FormCollection collection)
        {
            try
            {
                Supplier newSupplier = new Supplier();
                newSupplier.Supplier_UID = Guid.Parse(Supplier_UID);
                newSupplier.Country_UID = Guid.Parse(collection["Country"].ToString());
                newSupplier.Supplier_Name = collection["data.Supplier_Name"];
                _exposer.AddUpdateSupplier(newSupplier);
                return RedirectToAction("Overview");
            }
            catch
            {

                return View();
            }


        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(string Supplier_UID)
        {
            try
            {
                Supplier _deleteSupplier = new Supplier();
                _deleteSupplier.Supplier_UID = Guid.Parse(Supplier_UID);
                _exposer.deleteSupplier(_deleteSupplier);          
                return RedirectToAction("Overview");
            }
            catch
            {
                return View();
            }
        }
    }
}
