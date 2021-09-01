using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Graphs.Documents
{
    internal static class DocumentKeys<T>
        where T : class
    {
        public static PropertyInfo[] KeyProperties { get; } =
            typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null)
                .ToArray();

        public static string TypeName { get; } = typeof(T).Name;
    }
}
