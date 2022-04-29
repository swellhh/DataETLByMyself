using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DataETLViaHttp.BackgroundService;
using DataETLViaHttp.Strategy;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Aop
{
    public class DependencyResolver
    {
        private static IWindsorContainer _container;

        //Initialize the container
        public static IWindsorContainer Initialize()
        {
            _container = new WindsorContainer();

            return _container;
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        //Resolve types
        public static T For<T>(IInterceptor interceptor)
        {
            ProxyGenerator generator = new ProxyGenerator();
            var target = _container.Resolve<T>();
            var proxyInstance = generator.CreateClassProxyWithTarget(typeof(T), target, interceptor);
            return (T)proxyInstance;
        }

        public static void Register(params IRegistration[] registrations)
        {
            _container.Register(registrations);
        }
    }
}
