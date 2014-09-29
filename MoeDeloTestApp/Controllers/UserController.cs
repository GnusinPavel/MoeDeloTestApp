using System.Web.Mvc;
using MoeDeloTestApp.Dapper;
using MoeDeloTestApp.Models;

namespace MoeDeloTestApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager _userManager;
        private readonly PostManager _postManager;

        public UserController()
        {
            _userManager = new UserManager();
            _postManager = new PostManager();
        }

        // GET: /User/
        public ActionResult Index()
        {
            ViewBag.Posts = _postManager.GetAll();
            return View();
        }

        // POST: /User/Filter
        [HttpPost]
        public JsonResult Filter(int current, int rowCount, string searchPhrase)
        {
            int total;
            var users = _userManager.Filter(current, rowCount, searchPhrase, out total);
            return Json(
                new
                {
                    current,
                    rowCount,
                    rows = users,
                    total
                });
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (user.Id == 0)
            {
                _userManager.Add(user);
            }
            else
            {
                _userManager.Update(user);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            _userManager.Remove(id);
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            _userManager.Dispose();
            _postManager.Dispose();

            base.Dispose(disposing);
        }
    }
}
