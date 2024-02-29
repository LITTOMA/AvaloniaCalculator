using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaCalculator.Views
{
    public static class ValueConverters
    {
        public static FuncValueConverter<double, double> FontSizeConverter { get; } = new FuncValueConverter<double, double>(x => x == 0f ? 18 : x / 3);
        public static FuncValueConverter<Rect, double> BoundsToFontSizeConverter { get; } = new FuncValueConverter<Rect, double>(x =>
        {
            var min = Math.Min(x.Width, x.Height);
            return min == 0f ? 18 : min / 3;
        });
    }
}
