using BackEnd.Contracts;
using BackEnd.DataManager;
using BackEnd.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace BackEnd
{
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>

        #endregion
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<ApplicationDbContext>();
            container.RegisterType<ApplicationSignInManager>();
            container.RegisterType<ApplicationUserManager>();

            container.RegisterType<IAuthenticationManager>(
               new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
                new InjectionConstructor(typeof(ApplicationDbContext)));

            container.RegisterType<IPackageType, PackageTypDataManager>();


        }

    }
}