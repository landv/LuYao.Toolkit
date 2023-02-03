using System;
using System.Reflection;

namespace LuYao.Toolkit
{
    public static class AppHelper
    {
        public static Type ViewModelTypeResolver(Type viewType, params Assembly[] assemblies)
        {
            var name = $"{viewType.FullName}ViewModel";
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(name);
                if (type != null) return type;
            }
            return null;
        }
    }
}
