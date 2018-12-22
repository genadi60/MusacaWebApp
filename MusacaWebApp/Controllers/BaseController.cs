namespace MusacaWebApp.Controllers
{
    using Data;
    using SIS.MvcFramework;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            Db = new ApplicationDbContext();
        }

        public ApplicationDbContext Db { get;  }
    }
}
