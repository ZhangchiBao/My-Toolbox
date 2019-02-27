using Autofac;
using System;

namespace BookApp.Ndro.Common
{
    public class IOC
    {
        private static ContainerBuilder builder;
        private static IContainer container;

        static IOC()
        {
            builder = new ContainerBuilder();
        }

        public static void Registe<T>()
        {
            builder.RegisterType<T>().SingleInstance();
        }

        public static void Registe(Type type)
        {
            builder.RegisterType(type).SingleInstance();
        }

        public static void Build()
        {
            container = builder.Build();
        }

        public static T Get<T>()
        {
            return container.Resolve<T>();
        }

        public static object Get(Type type)
        {
            return container.Resolve(type);
        }
    }
}
