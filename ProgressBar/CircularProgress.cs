using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProgressBar
{
    public class CircularProgress : Shape
    {
        static CircularProgress()
        {
            var myGreenBrush = new SolidColorBrush(Color.FromArgb(255, 6, 176, 37));
            myGreenBrush.Freeze();

            StrokeProperty.OverrideMetadata(
                typeof(CircularProgress),
                new FrameworkPropertyMetadata(myGreenBrush));
            FillProperty.OverrideMetadata(
                typeof(CircularProgress),
                new FrameworkPropertyMetadata(Brushes.Transparent));

            StrokeThicknessProperty.OverrideMetadata(
                typeof(CircularProgress),
                new FrameworkPropertyMetadata(10.0));
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty); 
            set => SetValue(ValueProperty, value);
        }

        private static FrameworkPropertyMetadata valueMetadata =
                new FrameworkPropertyMetadata(
                    0.0,     
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    null,    
                    new CoerceValueCallback(CoerceValue)); 

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(CircularProgress), valueMetadata);

        private static object CoerceValue(DependencyObject depObj, object baseValue)
        {
            var value = (double)baseValue;
            value = Math.Min(value, 99.999);
            value = Math.Max(value, 0.0);
            return value;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                var maxWidth = Math.Max(0.0, RenderSize.Width - StrokeThickness);
                var maxHeight = Math.Max(0.0, RenderSize.Height - StrokeThickness);
                
                var startAngle = 90.0;
                var endAngle = 90.0 - ((Value / 100.0) * 360.0);
                var xStart = maxWidth / 2.0 * Math.Cos(startAngle * Math.PI / 180.0);
                var yStart = maxHeight / 2.0 * Math.Sin(startAngle * Math.PI / 180.0);
                
                var xEnd = maxWidth / 2.0 * Math.Cos(endAngle * Math.PI / 180.0);
                var yEnd = maxHeight / 2.0 * Math.Sin(endAngle * Math.PI / 180.0);

                var geometry = new StreamGeometry();
                using (StreamGeometryContext context = geometry.Open())
                {
                    context.BeginFigure(
                        new Point((RenderSize.Width / 2.0) + xStart,
                                  (RenderSize.Height / 2.0) - yStart), 
                                  true,  
                                  false);
                    context.ArcTo(
                        new Point((RenderSize.Width / 2.0) + xEnd,
                                  (RenderSize.Height / 2.0) - yEnd),
                        new Size(maxWidth / 2.0, maxHeight / 2), 
                        0.0,     
                        (startAngle - endAngle) > 180,  
                        SweepDirection.Clockwise,
                        true,    
                        false);
                }

                return geometry;
            }
        }
    }
}
