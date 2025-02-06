using Anade.Domain.Core;
using Anade.Khadamat.Business;
using Anade.Khadamat.Domain;
using Anade.Khadamat.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Anade.Khadamat.Web.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Admin,DG,AdminAgence,Formateur")]
    public class GenericReferentielController<T, TKey> : Controller where T : Referentiel, IEntity<TKey>
    {
        protected readonly ReferentielBusinessService<T, TKey> _referentielBusiessService;

        public GenericReferentielController(ReferentielBusinessService<T, TKey> refrentielBusiessService)
        {
            _referentielBusiessService = refrentielBusiessService;
        }
        // GET: Generic
        public virtual IActionResult Index()
        {
            var model = _referentielBusiessService.GetAll();

            return View(model);
        }

        public virtual IActionResult Paged(string search, int page = 1, int pageSize = 10)
        {
            PagedResult<T> model;
            if (page < 1)
                page = 1;

            if (pageSize > 50)
                pageSize = 50;

            if (!string.IsNullOrEmpty(search))
                model = _referentielBusiessService.GetAllFilteredPaged(x => x.Code.StartsWith(search) || x.DesignationFr.Contains(search),
                    "Code", (page - 1) * pageSize, pageSize, _referentielBusiessService.GetDefaultLoadProperties());
            else
                model = _referentielBusiessService.GetAllPaged("Code", (page - 1) * pageSize, pageSize, _referentielBusiessService.GetDefaultLoadProperties());

            ViewBag.search = search;

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult DataTable(DataTableAjaxModel model)
        {

            GetDataTableParameters(model, out string search, out string orderBy, out int startRowIndex, out int maxRows);

            if (!string.IsNullOrEmpty(search))
            {
                var result = _referentielBusiessService.GetAllFilteredPaged(x => x.Code.StartsWith(search) || x.DesignationFr.Contains(search),
                    orderBy, startRowIndex, maxRows, _referentielBusiessService.GetDefaultLoadProperties());
                return Json(new JQueryDataTableRetunedData<T> { draw = model.draw, recordsFiltered = result.TotalCount, recordsTotal = result.TotalCount, data = result.Items });
            }
            else
            {
                var result = _referentielBusiessService.GetAllPaged(orderBy, startRowIndex, maxRows, _referentielBusiessService.GetDefaultLoadProperties());

                return Json(new JQueryDataTableRetunedData<T> { draw = model.draw, recordsFiltered = result.TotalCount, recordsTotal = result.TotalCount, data = result.Items });
            }
        }
        [Authorize(Roles = "Admin,DG")]
        // GET: Generic/Details/5
        public virtual IActionResult Details(TKey id)
        {
            var model = _referentielBusiessService.GetById(id, _referentielBusiessService.GetDefaultLoadProperties());
            if (model == null)
                return NotFound();
            return View(model);
        }
        [Authorize(Roles = "Admin,DG")]
        // GET: Generic/Create
        public virtual IActionResult Create()
        {
            return View();
        }

        // POST: Generic/Create
        [Authorize(Roles = "Admin,DG")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Create(T model)
        {
            if (ModelState.IsValid)
            {
                var businessResult = _referentielBusiessService.Add(model);

                if (businessResult.Succeeded)
                {
                    TempData["Message"] = businessResult.ToBootstrapAlerts();

                    return RedirectToAction(nameof(Index));
                }

                ViewData["Message"] = businessResult.ToBootstrapAlerts();
            }


            return View(model);
        }
        [Authorize(Roles = "Admin,DG")]
        [Route("{id}")]
        // GET: Generic/Edit/5

        public virtual IActionResult Edit(TKey id)
        {
            var model = _referentielBusiessService.GetById(id, _referentielBusiessService.GetDefaultLoadProperties());
            if (model == null)
                return NotFound();
            return View(model);
        }

        // POST: Generic/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{id}")]
        public virtual IActionResult Edit(T model)
        {
            if (ModelState.IsValid)
            {
                var businessResult = _referentielBusiessService.Update(model);

                ViewData["Message"] = businessResult.ToBootstrapAlerts();
            }

            return View(model);
        }
        [Authorize(Roles = "Admin,DG")]
        [Route("{id}")]
        // GET: Generic/Delete/5
        public virtual IActionResult Delete(TKey id)
        {
            var model = _referentielBusiessService.GetById(id);
            if (model == null)
                return NotFound();
            return View(model);
        }

        // POST: Generic/Delete/5
        [Authorize(Roles = "Admin,DG")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{id}")]
        public virtual IActionResult Delete(TKey id, T model)
        {
            var businessResult = _referentielBusiessService.Delete(model);

            if (businessResult.Succeeded)
            {
                TempData["Message"] = businessResult.ToBootstrapAlerts();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Message"] = businessResult.ToBootstrapAlerts();

            return View(model);
        }

        protected static void GetDataTableParameters(DataTableAjaxModel model, out string search, out string orderBy, out int startRowIndex, out int maxRows)
        {
            maxRows = model.length;
            startRowIndex = model.start;
            string sortBy = "", sortDir = "";

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower();
                orderBy = sortBy + " " + sortDir;
            }
            else
            {
                orderBy = "";
            }

            if (model.search.value != null)
            {
                search = model.search.value;
            }
            else
            {
                search = "";
            }

        }
    }

}