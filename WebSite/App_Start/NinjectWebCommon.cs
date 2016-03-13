using System;
using System.Web;
using Driver.WebSite.Models;
using Driver.WebSite;
using Driver.WebSite.DAL;
using Driver.WebSite.DAL.Drivers;
using Driver.WebSite.DAL.Items;
using Driver.WebSite.DAL.Votes;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace Driver.WebSite
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = 
                    new Ninject.WebApi.DependencyResolver.NinjectDependencyResolver(kernel);

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();
            kernel.Bind<IItemsRepository>().To<ItemsRepository>().InRequestScope();
            kernel.Bind<IDriversRepository>().To<DriversRepository>().InRequestScope();
            kernel.Bind<IVotesRepository<Item>>().To<ItemVotesRepository>().InRequestScope();
            kernel.Bind<IVotesRepository<Comment>>().To<CommentVotesRepository>().InRequestScope();
            kernel.Bind<IVotesRepository<DriverOccurrence>>().To<DriverOccurenceVotesRepository>().InRequestScope();
            kernel.Bind<HomePageItemsQuery>().ToSelf().InRequestScope();
            kernel.Bind<TopItemsQuery>().ToSelf().InRequestScope();
            kernel.Bind<WaitingItemsQuery>().ToSelf().InRequestScope();
        }        
    }
}
