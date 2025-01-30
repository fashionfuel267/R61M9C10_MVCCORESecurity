using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using R61M9C10_W01.Data;
using R61M9C10_W01.ViewModels;

namespace R61M9C10_W01.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        IWebHostEnvironment _webHostEnvironment;
        public ProfileController(UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,  IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _userStore = userStore;
            _webHostEnvironment = hostEnvironment;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
           var uname= User.Identity.Name;
           //  var user =await _userManager.GetUserAsync(User);
            var user = _userManager.Users.FirstOrDefault(u => u.UserName.Equals(uname));
            return View(user);
        }
        [Authorize]
        public IActionResult EditProfile( string userid)
        {
            var user= _userManager.Users.FirstOrDefault(u=>u.Id.Equals(userid));
         var model=   new PROFILEVM
            {
              Fullname=user.Fullname,
               PhoneNumber=user.PhoneNumber,
               Email=user.Email,
               Userid=user.Id
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(PROFILEVM pROFILEVM)
        {
            if (pROFILEVM.ProfilePic != null)
            {
                string ext = Path.GetExtension(pROFILEVM.ProfilePic.FileName).ToLower();
                if(ext ==".jpg")
                {
                    string folderToSave = Path.Combine(_webHostEnvironment.WebRootPath,"Pictures");
                    if (!Directory.Exists(folderToSave)) { Directory.CreateDirectory(folderToSave); }
                   

                    string fileToSave = Path.Combine(folderToSave, pROFILEVM.Fullname + ext);
                using(var fs=new FileStream(fileToSave,FileMode.Create))
                {
                        pROFILEVM.ProfilePic.CopyTo(fs);
                }
                var user = _userManager.Users.FirstOrDefault(u => u.Id.Equals(pROFILEVM.Userid));
                user.Fullname = pROFILEVM.Fullname;
                user.PhoneNumber = pROFILEVM.PhoneNumber;
                    user.ProfilePicPath = "Pictures/" + pROFILEVM.Fullname + ext;
               IdentityResult result=await _userManager.UpdateAsync(user);
                if ((result.Succeeded))
                {
                    return RedirectToAction("Index");
                }
               else if (result.Errors.Count()>0)
                {
                    string msg = "";
                    foreach (var item in result.Errors)
                    {
                        msg += $"Error code {item.Code}, Description:{item.Description}";
                    }
                    ModelState.AddModelError("", msg);
                    return View(pROFILEVM);
                }
                } 
            }
            else
            {
                ModelState.AddModelError("", "Provie rpofile pic");
                return View(pROFILEVM);
            }
            ModelState.AddModelError("", "Save failed");
            return View(pROFILEVM);
        }
    }
}
