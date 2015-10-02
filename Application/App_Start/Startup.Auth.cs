using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Application.Models;

namespace Application
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Настройка контекста базы данных, диспетчера пользователей и диспетчера входа для использования одного экземпляра на запрос
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // регистрация менеджера ролей
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "66902402963-14v6dfmne1mg34jbb1htto9uasqe1hnu.apps.googleusercontent.com",
                ClientSecret = "QQsHpgVXq2BzRQvgM2T-_uag"
            });

        }
    }
}