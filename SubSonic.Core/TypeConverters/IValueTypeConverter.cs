using System;
using SubSonic.Configuration;
using SubSonic.Extensions;

namespace SubSonic.TypeConverters
{
    public interface IValueTypeConverter<in TSourceType, out TDestinationType>
    {
        TDestinationType Convert(TSourceType value);
    }

    public class StringToGuidValueTypeConverter : IValueTypeConverter<String, Guid>
    {
        public Guid Convert(string value)
        {
            return new Guid(value);
        }
    }

    public class DoubleToDecimalValueTypeConverter:IValueTypeConverter<Double, Decimal>
    {
        public decimal Convert(double value)
        {
            return new decimal(value);
        }
    }

    public class ValueTypeConverterService
    {
        public static object ChangeType(Object value, Type destinationType)
        {
            try
            {
                
                var valueTypeConverter = LocateValueTypeConverter(value.GetType(), destinationType);
                return valueTypeConverter.GetType().GetMethod("Convert").Invoke(valueTypeConverter, new[] {value});
            }
            catch (Castle.MicroKernel.ComponentNotFoundException)
            {
                return Convert.ChangeType(value, destinationType);   
            }
        }

        private static object LocateValueTypeConverter(Type sourceType, Type destinationType)
        {
            try
            {
                return TryLocateValueTypeConverter(sourceType, destinationType);
                
            }
            catch (Castle.MicroKernel.ComponentNotFoundException)
            {
                return TryLocateValueTypeConverter(sourceType, destinationType.ToNotNullable());
            }  
        }

        private static object TryLocateValueTypeConverter(Type sourceType, Type destinationType)
        {
            var valueTypeConverterInterface = typeof(IValueTypeConverter<,>).MakeGenericType(sourceType, destinationType);
                return Configurator.Container.Resolve(valueTypeConverterInterface);
        }

        public static Func<object, object> ChangeTypeFunction(Type sourceType, Type destinationType, bool returnNullIfValueTypeConverterIsNotFound = false)
        {
            try
            {
                var valueTypeConverter = LocateValueTypeConverter(sourceType, destinationType);
                return src => valueTypeConverter.GetType().GetMethod("Convert").Invoke(valueTypeConverter, new[] { src });
            }
            catch (Castle.MicroKernel.ComponentNotFoundException)
            {
                if (returnNullIfValueTypeConverterIsNotFound)
                    return null;
                return src => Convert.ChangeType(src, destinationType);
            }
        }
    }
}