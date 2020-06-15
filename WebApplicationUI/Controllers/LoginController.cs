using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuevoSoftware.ApplicationMonitoring.Data;
using NuevoSoftware.ApplicationMonitoring.Models;
using NuevoSoftware.ApplicationMonitoring.Common;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using NuevoSoftware.ApplicationMonitoring.ClientBusiness;

namespace WebApplicationUI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly NSAppMonDBContext _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public LoginController(NSAppMonDBContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            string dialogMessage = TempData[Constants.StateKey.DialogMessage] as string;
            if (!string.IsNullOrEmpty(dialogMessage))
            {
                ViewBag.DialogTitle = TempData[Constants.StateKey.DialogTitle] as string;
                ViewBag.DialogMessage = dialogMessage;
            }
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel tempModel)
        {
            if (ModelState.IsValid)
            {
                NsusersT userRecord = await _context.NsusersT.SingleOrDefaultAsync(entity => entity.UserName == tempModel.Username && entity.Password == tempModel.Password && entity.IsActive == true);
                if (userRecord == null)
                {
                    ViewBag.DialogTitle = ResourceManager.Instance.GetResource(Constants.ResourceKey.Warning);
                    ViewBag.DialogMessage = ResourceManager.Instance.GetResource(Constants.ResourceKey.LoginDataError);
                    return View(tempModel);
                }

                if (tempModel.Password.Equals(userRecord.Password))
                {
                    NsusersT userContext = await _context.NsusersT.Include(entity => entity.NsapplicationsT).SingleAsync(entity => entity.Id == userRecord.Id);
                    UserModel userModel = ClientBusiness.MapUserModel(userContext);
                    TempData[Constants.StateKey.UserModel] = JsonConvert.SerializeObject(userModel);
                    return RedirectToAction("Index", "AppMon");
                }
            }
            return View(tempModel);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Signup()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(UserModel tempModel)
        {
            if (ModelState.IsValid)
            {
                if (!tempModel.Password.Equals(tempModel.ReEnterPassword))
                {
                    ViewBag.DialogTitle = ResourceManager.Instance.GetResource(Constants.ResourceKey.Warning);
                    ViewBag.DialogMessage = ResourceManager.Instance.GetResource(Constants.ResourceKey.PasswordsIncorrect);
                    return View(tempModel);
                }
                NsusersT oldUserRecord = await _context.NsusersT.SingleOrDefaultAsync(entity => entity.UserName == tempModel.Username && entity.IsActive == true);
                if (oldUserRecord != null)
                {
                    ViewBag.DialogTitle = ResourceManager.Instance.GetResource(Constants.ResourceKey.Warning);
                    ViewBag.DialogMessage = ResourceManager.Instance.GetResource(Constants.ResourceKey.UserExist);
                    return View(tempModel);
                }
                NsusersT newUserRecord = new NsusersT
                {
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = tempModel.Password,
                    UserName = tempModel.Username,
                    Mail = tempModel.Mail
                };
                _context.Add(newUserRecord);
                await _context.SaveChangesAsync();
                TempData[Constants.StateKey.DialogTitle] = ResourceManager.Instance.GetResource(Constants.ResourceKey.Success);
                TempData[Constants.StateKey.DialogMessage] = ResourceManager.Instance.GetResource(Constants.ResourceKey.Success);
                return RedirectToAction(nameof(Login));
            }

            return View(tempModel);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            return RedirectToAction("Login", "Login");
        }
    }
}
