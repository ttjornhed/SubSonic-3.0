using System;

namespace SubSonic.Extensions
{
    public static class TypeExtensions
    {
        public static Type ToNotNullable(this Type type)
        {
            if (type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof (Nullable<>))
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }
    }
}