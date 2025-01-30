using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using R61M9C10_W01.Data;
using R61M9C10_W01.ViewModels;

namespace R61M9C10_W01.Controllers
{
    public class ManageRoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleStore<IdentityRole> _roleStore;
        public ManageRoleController(RoleManager<IdentityRole> _roleManager, IRoleStore<IdentityRole>rolestore)
        {
            this._roleManager = _roleManager;
            this._roleStore = rolestore;
            
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.OrderBy(r=>r.Name);
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string rolename)
        {
            var roles =await _roleManager.CreateAsync(new IdentityRole
            {
                Name = rolename
            });
            if (roles.Succeeded)
            {
                return RedirectToAction("Index");
            }
            if (roles.Errors.Count() > 0)
            {

                string msg = "";
                foreach (var item in roles.Errors)
                {
                    msg += $"Error code {item.Code}, Description:{item.Description}";
                }
             ViewBag.errorMsg=msg;
               
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var role= _roleManager.Roles.FirstOrDefault(r=>r.Id.Equals(id));
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(IdentityRole role)
        {
            
            var roles = await _roleManager.UpdateAsync(role);
            if (roles.Succeeded)
            {
                return RedirectToAction("Index");
            }
            if (roles.Errors.Count() > 0)
            {

                string msg = "";
                foreach (var item in roles.Errors)
                {
                    msg += $"Error code {item.Code}, Description:{item.Description}";
                }
                ViewBag.errorMsg = msg;

            }
            return View();
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = _roleManager.Roles.FirstOrDefault(r => r.Id.Equals(id));
          var result= await  _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            if (result.Errors.Count() > 0)
            {

                string msg = "";
                foreach (var item in result.Errors)
                {
                    msg += $"Error code {item.Code}, Description:{item.Description}";
                }
                ViewBag.errorMsg = msg;

            }
            return View(role);
        }
    }
}
