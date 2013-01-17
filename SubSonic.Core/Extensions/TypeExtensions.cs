using System;
<<<<<<< HEAD
using System.Linq;
=======
>>>>>>> 3bb947a811ef025821d4116fb33261404047507d

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
<<<<<<< HEAD

        public static bool Implements(this Type type, Type interfaceType)
        {
            if (!interfaceType.IsInterface)
                throw new ArgumentException("Argument must be an interface type.", "interfaceType");
            return type.GetInterfaces().ToList().Contains(interfaceType);
        }
=======
>>>>>>> 3bb947a811ef025821d4116fb33261404047507d
    }
}