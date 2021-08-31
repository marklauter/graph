using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Repositories
{
    internal static class KeyInfoCache<T>
        where T : class
    {
        public static readonly PropertyInfo[] KeyProperties =
            typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null)
                .ToArray();
    }
}
