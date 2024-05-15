using BlackHoleGame.Persistence;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BHWPF.ViewModel
{
    public class CellStateToImageBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is CellState cellState && values[1] is bool selected)
            {
                return GetImageBrush(cellState, selected);
            }

            return DependencyProperty.UnsetValue; // Return DependencyProperty.UnsetValue for an invalid value
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(); // ConvertBack is not needed for a one-way binding
        }

        private ImageBrush GetImageBrush(CellState cellState, bool selected)
        {
            ImageBrush imageBrush = new ImageBrush();
            BitmapImage? image;

            switch (cellState)
            {
                case CellState.Red:
                    image = new BitmapImage(new Uri(selected ? "pack://application:,,,/Resources/RedSelected.bmp" : "pack://application:,,,/Resources/Red.bmp"));
                    break;
                case CellState.Blue:
                    image = new BitmapImage(new Uri(selected ? "pack://application:,,,/Resources/BlueSelected.bmp" : "pack://application:,,,/Resources/Blue.bmp"));
                    break;
                case CellState.Black:
                    image = new BitmapImage(new Uri("pack://application:,,,/Resources/Black.bmp"));
                    break;
                case CellState.None:
                default:
                    image = null;
                    break;
            }

            imageBrush.ImageSource = image;
            return imageBrush;
        }
    }
}

