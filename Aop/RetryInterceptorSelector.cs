using Castle.DynamicProxy;
using DataETLViaHttp.BackgroundService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataETLViaHttp.Aop
{
    public class RetryInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (method.Name != "Exeute")
            {
                return interceptors.Where(w => !(w is DataETLRetryInterceptor)).ToArray();
            }

            return interceptors;
        }
    }
}
