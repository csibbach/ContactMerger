using ContactMerger.DataProviders.contracts;
using ContactMerger.Factories.contracts;
using ContactMerger.Factories.implementations;
using ContactMerger.Utility;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ContactMerger.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(ContactMerger.App_Start.NinjectWebCommon), "Stop")]

namespace ContactMerger.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

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
        /// This is a good place in production to put in some reflection and use a module system
        /// For this simple project I'll just bind everything directly and leave it be.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IFlowMetadataFactory>().To<FlowMetadataFactory>();
            kernel.Bind<IGoogleCredentialProvider>().To<IGoogleCredentialProvider>().InSingletonScope();
            kernel.Bind<IContactProvider>().To<DataProviders.implementations.ContactProvider>();
            kernel.Bind<IContactFactory>().To<ContactFactory>().InSingletonScope();
            kernel.Bind<IGoogleServiceFactory>().To<GoogleServiceFactory>().InSingletonScope();
        }        
    }
}