using System;
using SubSonic.Configuration;

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

    public class ValueTypeConverterService
    {
        public static object ChangeType(Object value, Type conversionType)
        {
            try
            {
                var valueTypeConverterInterface = typeof(IValueTypeConverter<,>).MakeGenericType(value.GetType(), conversionType);
                var valueTypeConverter = Configurator.Container.Resolve(valueTypeConverterInterface);
                return valueTypeConverter.GetType().GetMethod("Convert").Invoke(valueTypeConverter, new[] {value});
            }
            catch (Castle.MicroKernel.ComponentNotFoundException)
            {
                return Convert.ChangeType(value, conversionType);   
            }
        }
    }
}