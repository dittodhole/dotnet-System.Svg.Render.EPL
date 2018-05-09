using System;
using System.ComponentModel;
using System.Globalization;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  // taken from http://stackoverflow.com/a/2172576/57508

  [TypeConverter(typeof(PercentageConverter))]
  public struct Percentage
  {
    public float Value { get; }

    public Percentage(float value)
    {
      this.Value = value;
    }

    public Percentage(string value)
    {
      var percentage = (Percentage) TypeDescriptor.GetConverter(typeof(Percentage))
                                                  .ConvertFromString(value);
      this.Value = percentage.Value;
    }

    public override string ToString()
    {
      return this.ToString(CultureInfo.InvariantCulture);
    }

    public string ToString(CultureInfo Culture)
    {
      return TypeDescriptor.GetConverter(this.GetType())
                           .ConvertToString(null,
                                            Culture,
                                            this);
    }
  }

  public class PercentageConverter : TypeConverter
  {
    private static readonly TypeConverter Instance = TypeDescriptor.GetConverter(typeof(float));

    public override bool CanConvertFrom([NotNull] ITypeDescriptorContext context,
                                        Type sourceType)
    {
      return PercentageConverter.Instance.CanConvertFrom(context,
                                                         sourceType);
    }

    public override bool CanConvertTo([NotNull] ITypeDescriptorContext context,
                                      Type destinationType)
    {
      if (destinationType == typeof(Percentage))
      {
        return true;
      }

      return PercentageConverter.Instance.CanConvertTo(context,
                                                       destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context,
                                       [CanBeNull] CultureInfo culture,
                                       object value)
    {
      if (value == null)
      {
        return new Percentage();
      }
      if (culture == null)
      {
        culture = CultureInfo.InvariantCulture;
      }

      if (value is string)
      {
        var s = value as string;
        s = s.TrimEnd(' ',
                      '\t',
                      '\r',
                      '\n');

        var percentage = s.EndsWith(culture.NumberFormat.PercentSymbol);
        if (percentage)
        {
          s = s.Substring(0,
                          s.Length - culture.NumberFormat.PercentSymbol.Length);
        }

        var result = (float) PercentageConverter.Instance.ConvertFromString(s);
        if (percentage)
        {
          result /= 100;
        }

        return new Percentage(result);
      }

      return new Percentage((float) PercentageConverter.Instance.ConvertFrom(context,
                                                                             culture,
                                                                             value));
    }

    public override object ConvertTo([NotNull] ITypeDescriptorContext context,
                                     [CanBeNull] CultureInfo culture,
                                     [CanBeNull] object value,
                                     Type destinationType)
    {
      if (!(value is Percentage))
      {
        throw new ArgumentNullException(nameof(value));
      }
      if (culture == null)
      {
        culture = CultureInfo.InvariantCulture;
      }

      var percentage = (Percentage) value;

      if (destinationType == typeof(string))
      {
        return PercentageConverter.Instance.ConvertTo(context,
                                                      culture,
                                                      percentage.Value * 100,
                                                      destinationType) + culture.NumberFormat.PercentSymbol;
      }

      return PercentageConverter.Instance.ConvertTo(context,
                                                    culture,
                                                    percentage.Value,
                                                    destinationType);
    }
  }
}
