using DAL.EntityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : Controller
    {
        [HttpGet]
        public async Task<string> Index()
        {
            QueryProContext db = new QueryProContext();
            var user = db.Users.Where(x => x.Username == "").FirstOrDefault();
            return "";
        }
    }
}
