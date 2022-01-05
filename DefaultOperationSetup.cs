using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace kdyf.operations
{
    public static class DefaultOperationSetup
    {
        public static IServiceCollection AddDefaultOperations(this IServiceCollection services, List<Assembly> assemblies)
        {
            services.AddScoped<IOperationExecutor, OperationExecutor>();

            var allOperationsAndTypes = 
                assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(s=>!s.IsAbstract)
                .Select(s => new
                {
                    Operation = s,
                    Interface = s.GetInterfaces().FirstOrDefault(y => y.IsGenericType && y.GetInterfaces().FirstOrDefault() == typeof(IOperation))
                })
                .Where(s=>s.Interface != null)
                .ToList();


            foreach (var opType in allOperationsAndTypes)
                services.AddTransient(opType.Operation);

            return services;

        }
    }
}
