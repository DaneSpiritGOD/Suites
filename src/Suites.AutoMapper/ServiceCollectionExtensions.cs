using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        private static readonly Action<IMapperConfigurationExpression> DefaultConfig = delegate
        {
        };

        public static IServiceCollection AddHostedAutoMapper(this IServiceCollection services)
        {
            return services.AddHostedAutoMapper(null, AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IServiceCollection AddHostedAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> additionalInitAction)
        {
            return services.AddHostedAutoMapper(additionalInitAction, AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IServiceCollection AddHostedAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
        {
            return AddAutoMapperClasses(services, null, assemblies);
        }

        public static IServiceCollection AddHostedAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> additionalInitAction, params Assembly[] assemblies)
        {
            return AddAutoMapperClasses(services, additionalInitAction, assemblies);
        }

        public static IServiceCollection AddHostedAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> additionalInitAction, IEnumerable<Assembly> assemblies)
        {
            return AddAutoMapperClasses(services, additionalInitAction, assemblies);
        }

        public static IServiceCollection AddHostedAutoMapper(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            return AddAutoMapperClasses(services, null, assemblies);
        }

        public static IServiceCollection AddHostedAutoMapper(this IServiceCollection services, params Type[] profileAssemblyMarkerTypes)
        {
            return AddAutoMapperClasses(services, null, from t in profileAssemblyMarkerTypes
                                                        select t.GetTypeInfo().Assembly);
        }

        public static IServiceCollection AddHostedAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> additionalInitAction, params Type[] profileAssemblyMarkerTypes)
        {
            return AddAutoMapperClasses(services, additionalInitAction, from t in profileAssemblyMarkerTypes
                                                                        select t.GetTypeInfo().Assembly);
        }

        public static IServiceCollection AddHostedAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> additionalInitAction, IEnumerable<Type> profileAssemblyMarkerTypes)
        {
            return AddAutoMapperClasses(services, additionalInitAction, from t in profileAssemblyMarkerTypes
                                                                        select t.GetTypeInfo().Assembly);
        }

        private static IServiceCollection AddAutoMapperClasses(IServiceCollection services, Action<IMapperConfigurationExpression> additionalInitAction, IEnumerable<Assembly> assembliesToScan)
        {
            if (services.Any((ServiceDescriptor sd) => sd.ServiceType == typeof(IMapper)))
            {
                return services;
            }
            additionalInitAction = (additionalInitAction ?? DefaultConfig);
            assembliesToScan = ((assembliesToScan as Assembly[]) ?? assembliesToScan.ToArray());
            TypeInfo[] allTypes = (from a in assembliesToScan
                                   where a.GetName().Name != "AutoMapper"
                                   select a).SelectMany((Assembly a) => a.DefinedTypes).ToArray();
            TypeInfo[] profiles = allTypes.Where(delegate (TypeInfo t)
            {
                if (typeof(Profile).GetTypeInfo().IsAssignableFrom(t))
                {
                    return !t.IsAbstract;
                }
                return false;
            }).ToArray();
            IConfigurationProvider config = new MapperConfiguration(
                cfg =>
                {
                    additionalInitAction(cfg);
                    foreach (Type item in from t in profiles
                                          select t.AsType())
                    {
                        cfg.AddProfile(item);
                    }
                });
            foreach (TypeInfo item2 in new Type[4]
            {
            typeof(IValueResolver<, , >),
            typeof(IMemberValueResolver<, , , >),
            typeof(ITypeConverter<, >),
            typeof(IMappingAction<, >)
            }.SelectMany((Type openType) => allTypes.Where(delegate (TypeInfo t)
            {
                if (t.IsClass && !t.IsAbstract)
                {
                    return t.AsType().ImplementsGenericInterface(openType);
                }
                return false;
            })))
            {
                services.AddTransient(item2.AsType());
            }
            services.AddSingleton(config);
            return services.AddSingleton<IMapper>((IServiceProvider sp) => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
        }

        private static bool ImplementsGenericInterface(this Type type, Type interfaceType)
        {
            if (!type.IsGenericType(interfaceType))
            {
                return type.GetTypeInfo().ImplementedInterfaces.Any((Type @interface) => @interface.IsGenericType(interfaceType));
            }
            return true;
        }

        private static bool IsGenericType(this Type type, Type genericType)
        {
            if (type.GetTypeInfo().IsGenericType)
            {
                return type.GetGenericTypeDefinition() == genericType;
            }
            return false;
        }
    }

}
