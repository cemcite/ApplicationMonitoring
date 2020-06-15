using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuevoSoftware.ApplicationMonitoring.Common;
using NuevoSoftware.ApplicationMonitoring.Data;
using NuevoSoftware.ApplicationMonitoring.Models;
using NuevoSoftware.ApplicationMonitoring.ClientBusiness;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationUI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AppMonController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly NSAppMonDBContext _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public AppMonController(NSAppMonDBContext context)
        {
            _context = context;
        }
        // GET: AppMon
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            UserModel userModel = GetUserModel();
            return View(userModel.ApplicationList);
        }

        /// <summary>
        /// GET: AppMon/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id)
        {
            AppMonModel applicationModel = GetApplicationModel(id);
            return View(applicationModel);
        }

        // GET: AppMon/Create
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppMon/Create
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppMonModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel userModel = GetUserModel();
                NsapplicationsT nsapplicationsT = new NsapplicationsT
                {
                    CreatedDate = DateTime.Now,
                    Interval = model.Interval,
                    IsActive = true,
                    Name = model.Name,
                    Url = model.URL,
                    UserId = userModel.ID
                };
                _context.Add(nsapplicationsT);
                await _context.SaveChangesAsync();
                await UpdateTempData(userModel.ID);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AppMon/Edit/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id)
        {
            AppMonModel model = GetApplicationModel(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: AppMon/Edit/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="applicationModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppMonModel applicationModel)
        {
            if (id != applicationModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                DateTime dateTimeNow = DateTime.Now;
                UserModel userModel = GetUserModel();
                NsapplicationsT nsapplicationsTOld = await _context.NsapplicationsT.SingleAsync(entity => entity.Id == applicationModel.ID);
                if (nsapplicationsTOld == null)
                {
                    return NotFound();
                }
                nsapplicationsTOld.IsActive = false;
                nsapplicationsTOld.UpdatedDate = dateTimeNow;

                NsapplicationsT nsapplicationsTNew = new NsapplicationsT
                {
                    CreatedDate = dateTimeNow,
                    Interval = applicationModel.Interval,
                    IsActive = true,
                    Name = applicationModel.Name,
                    Url = applicationModel.URL,
                    UserId = userModel.ID
                };

                try
                {
                    _context.Update(nsapplicationsTOld);
                    _context.Add(nsapplicationsTNew);
                    await _context.SaveChangesAsync();
                    await UpdateTempData(userModel.ID);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NsapplicationsTExists(nsapplicationsTOld.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationModel);
        }

        // GET: AppMon/Delete/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id)
        {
            AppMonModel model = GetApplicationModel(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: AppMon/Delete/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            NsapplicationsT nsapplicationsT = await _context.NsapplicationsT.SingleOrDefaultAsync(entity => entity.Id == id);
            if (nsapplicationsT == null)
            {
                return NotFound();
            }
            nsapplicationsT.IsActive = false;
            nsapplicationsT.UpdatedDate = DateTime.Now;
            _context.Update(nsapplicationsT);
            await _context.SaveChangesAsync();
            await UpdateTempData(nsapplicationsT.UserId);
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool NsapplicationsTExists(int id)
        {
            return _context.NsapplicationsT.Any(e => e.Id == id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private UserModel GetUserModel()
        {
            UserModel userModel = JsonConvert.DeserializeObject<UserModel>(TempData[Constants.StateKey.UserModel] as string);
            TempData.Keep(Constants.StateKey.UserModel);
            return userModel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationID"></param>
        /// <returns></returns>
        private AppMonModel GetApplicationModel(int applicationID)
        {
            UserModel userModel = GetUserModel();
            return userModel.ApplicationList.GetApplication(applicationID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private async Task UpdateTempData(int userID)
        {
            NsusersT userContext = await _context.NsusersT.Include(entity => entity.NsapplicationsT).SingleAsync(entity => entity.Id == userID);
            UserModel userModel = ClientBusiness.MapUserModel(userContext);
            TempData[Constants.StateKey.UserModel] = JsonConvert.SerializeObject(userModel);
        }
    }
}
