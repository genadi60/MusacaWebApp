namespace MusacaWebApp.Controllers
{
    using SIS.HTTP.Responses;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (User.IsLoggedIn)
            {
                return Redirect("/users/index");
            }
            return View();
        }
    }
}
