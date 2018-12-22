namespace MusacaWebApp.Controllers
{
    using System;
    using System.Linq;

    using InputModels;
    using Models;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Services;
    using ViewModels;

    public class UsersController : BaseController
    {
        private readonly IHashService _hashService;

        public UsersController(IHashService hashService)
        {
            _hashService = hashService;
        }

        [Authorize]
        public IHttpResponse Index()
        {
            var orders = Db.Orders.Where(o => o.Status == Status.Active && o.Cashier.Username.Equals(User.Username))
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    Product = o.Product.Name,
                    Quantity = o.Quantity,
                    Price = o.Product.Price
                })
                .ToList();

            var loggedInUser = new LoggedInUserViewModel
            {
                OrderViewModels = orders
            };

            return View("/home/loggedInUser", loggedInUser);
        }

        [Authorize]
        public IHttpResponse Profile()
        {
           var receipts = Db.Receipts.Where(r => r.Cashier.Username.Equals(User.Username))
                .Select(r => new ReceiptViewModel
                {
                    Id = r.Id,
                    IssuedOn = r.IssuedOn.ToString("dd/MM/yyyy"),
                    Total = r.ReceiptOrders.Select(ro => ro.Orders.Sum(o => o.Product.Price * o.Quantity)).Sum(),
                    Cashier = r.Cashier.Username
                })
                .ToList();

           var profile = new UserProfileViewModel
            {
                Receipts = receipts
            };

            return View("/users/profile", profile);
        }

        [Authorize]
        public IHttpResponse Logout()
        {
            if (!Request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return Redirect("/");
            }

            var cookie = Request.Cookies.GetCookie(".auth-cakes");
            cookie.Delete();
            Response.Cookies.Add(cookie);
            return Redirect("/");
        }

        public IHttpResponse Login()
        {
            if (User.IsLoggedIn)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost]
        public IHttpResponse Login(DoLoginInputModel model)
        {
            if (User.IsLoggedIn)
            {
                return Redirect("/");
            }

            var hashedPassword = _hashService.Hash(model.Password);

            var user = Db.Users.FirstOrDefault(x =>
                x.Username == model.Username.Trim() &&
                x.Password == hashedPassword);

            if (user == null)
            {
                return BadRequestErrorWithView("Invalid username or password.");
            }

            var mvcUser = new MvcUserInfo
            {
                Username = user.Username,
                Role = user.Role.ToString(),
                Info = user.Email,
            };
            var cookieContent = UserCookieService.GetUserCookie(mvcUser);

            var cookie = new HttpCookie(".auth-cakes", cookieContent, 7) { HttpOnly = true };
            Response.Cookies.Add(cookie);

            return Redirect("/");
        }

        public IHttpResponse Register()
        {
            if (User.IsLoggedIn)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost]
        public IHttpResponse Register(DoRegisterInputModel model)
        {
            if (User.IsLoggedIn)
            {
                return Redirect("/");
            }
            
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Trim().Length < 4)
            {
                return BadRequestErrorWithView("Please provide valid username with length of 4 or more characters.", model);
            }

            if (string.IsNullOrWhiteSpace(model.Email) || model.Email.Trim().Length < 4)
            {
                return BadRequestErrorWithView("Please provide valid email with length of 4 or more characters.", model);
            }

            if (Db.Users.Any(x => x.Username == model.Username.Trim()))
            {
                return BadRequestErrorWithView("User with the same name already exists.", model);
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
            {
                return BadRequestErrorWithView("Please provide password of length 6 or more.", model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                return BadRequestErrorWithView("Passwords do not match.", model);
            }

            var hashedPassword = _hashService.Hash(model.Password);

            var role = Role.User;
            if (!Db.Users.Any())
            {
                role = Role.Admin;
            }

            var user = new User
            {
                Username = model.Username.Trim(),
                Email = model.Email.Trim(),
                Password = hashedPassword,
                Role = role
            };

            Db.Users.Add(user);

            try
            {
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequestErrorWithView(e.Message);
            }

            return Redirect("/users/login");
        }
    }
}