using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfUserControl = System.Windows.Controls.UserControl;


namespace WindowsIntegrityGuard.UI.Controls
{
    public partial class CircularProgressBar : WpfUserControl
    {
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(double), typeof(CircularProgressBar), new PropertyMetadata(0d, OnProgressChanged));

        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        public CircularProgressBar()
        {
            InitializeComponent();

            Loaded += (_, _) => UpdateProgress();
            SizeChanged += (_, _) => UpdateProgress();
        }

        private static void OnProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CircularProgressBar control)
            {
                control.UpdateProgress();
            }
        }

        private void UpdateProgress()
        {
            double progress = Math.Clamp(Progress, 0, 100);
            double angle = progress / 100d * 360d;
            double radius = Math.Min(ActualWidth, ActualHeight) / 2d - 8;

            if (radius <= 0)
            {
                return;
            }

            System.Windows.Point center = new(ActualWidth / 2d, ActualHeight / 2d);

            if (progress <= 0)
            {
                ProgressPath.Data = Geometry.Empty;
                return;
            }

            if (progress >= 100)
            {
                ProgressPath.Data = new EllipseGeometry(center, radius, radius);
                return;
            }

            double startAngle = -90;
            double endAngle = startAngle + angle;

            System.Windows.Point startPoint = GetPoint(center, radius, startAngle);
            System.Windows.Point endPoint = GetPoint(center, radius, endAngle);

            bool isLargeArc = angle > 180;

            PathFigure figure = new()
            {
                StartPoint = startPoint,
                IsClosed = false
            };

            figure.Segments.Add(new ArcSegment
            {
                Point = endPoint,
                Size = new System.Windows.Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = isLargeArc
            });

            PathGeometry geometry = new();
            geometry.Figures.Add(figure);

            ProgressPath.Data = geometry;
        }

        private static System.Windows.Point GetPoint(System.Windows.Point center, double radius, double angle)
        {
            double radians = angle * Math.PI / 180d;

            return new System.Windows.Point(center.X + radius * Math.Cos(radians), center.Y + radius * Math.Sin(radians));
        }
    }
}
