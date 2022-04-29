using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using DataETLViaHttp.BackgroundService;
using DataETLViaHttp.Strategy;
using DataETLViaHttp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataETLViaHttp.Aop
{
    public class ComponentRegistration : IRegistration
    {
        public void Register(IKernelInternal kernel)
        {
            kernel.Register(
                Component.For<DataETLRetryInterceptor>().LifeStyle.Transient);

            kernel.Register(
                Component.For<LoggerInterceptor>().LifeStyle.Transient);

            var types = typeof(BaseStrategy).GetTypeInfo().Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(BaseStrategy)));

            foreach (var item in types)
            {
                kernel.Register(
               Component.For(item)
               .Interceptors(InterceptorReference.ForType<DataETLRetryInterceptor>()).Anywhere
               .Interceptors(InterceptorReference.ForType<LoggerInterceptor>()).Anywhere);
            }

        }
    }
}
