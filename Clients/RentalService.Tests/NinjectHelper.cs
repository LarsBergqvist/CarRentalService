using System;
using System.IO;
using System.Reflection;
using System.Text;
using Ninject;
using RentalService.Contract;
using RentalsRepository.Contract;
using Ninject.Extensions.Conventions;
using VehicleTypes.Contract;

namespace RentalService.Tests
{
    public class NinjectHelper
    {
        public IKernel Kernel { get; private set; }
        public IRentalsRepository RentalsRepo { get; private set; }
        public IRentalService RentalService { get; private set; }
        public IVehicleTypesRepository VehicleTypesRepo { get; private set; }

        public void BindImplementations()
        {
            Kernel = new StandardKernel();
            Kernel.Load(Assembly.GetExecutingAssembly());

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            try
            {
                Kernel.Bind(x =>
                {
                    x.FromAssembliesInPath(path)
                        .SelectAllClasses()
                        .InheritedFrom<IRentalsRepository>()
                        .BindDefaultInterface().Configure(b => b.InSingletonScope());
                });

                Kernel.Bind(x =>
                {
                    x.FromAssembliesInPath(path)
                        .SelectAllClasses()
                        .InheritedFrom<IFactory>()
                        .BindDefaultInterface().Configure(b => b.InSingletonScope());
                });

                Kernel.Bind(x =>
                {
                    x.FromAssembliesInPath(path)
                        .SelectAllClasses()
                        .InheritedFrom<IRentalService>()
                        .BindAllInterfaces().Configure(b => b.InSingletonScope());
                });

                RentalsRepo = Kernel.Get<IRentalsRepository>();
                RentalService = Kernel.Get<IRentalService>();
                VehicleTypesRepo = Kernel.Get<IVehicleTypesRepository>();
            }
            catch (Ninject.ActivationException exc)
            {
                var message = new StringBuilder();
                message.AppendLine("Exception when binding with Ninject.");
                message.AppendFormat(
                    "Check that all implementation assemblies exists in the executing assemblies path {0}{1}", path,
                    Environment.NewLine);
                message.AppendLine("If using Resharper, turn off shadow-copying of assemblies in unit test settings.");
                message.AppendLine(exc.Message);
                var wrappingExc = new Exception(message.ToString());
                throw wrappingExc;
            }            
        }
    }
}
