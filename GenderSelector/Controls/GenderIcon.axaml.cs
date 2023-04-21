using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using System;

namespace GenderSelector.Controls
{
	public partial class GenderIcon : UserControl
	{
		public static readonly StyledProperty<double> ValueProperty =
		AvaloniaProperty.Register<GenderIcon, double>(nameof(Value), 0);
		public double Value { get => GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

		private readonly GenderIconVM vm;
		public GenderIcon()
		{
			InitializeComponent();
			canvas.DataContext = vm = new GenderIconVM();
			ValueProperty.Changed.AddClassHandler<GenderIcon>((x, e) => x.vm.ZeroToOne = e.NewValue is double d ? d : 0);
		}
	}
	public class GenderIconVM : ReactiveObject
	{
		private double _zeroToOne;

		private readonly double[] Widths = { 50, 100, 80 };
		private readonly double[] Rotations = { 0, 40, 45 };
		private readonly double[] LegTranslatesX = { -30, 0, -25 };
		private readonly double[] LegTranslatesY = { -10, 0, 25 };
		private readonly Color[] Colors = { Avalonia.Media.Colors.Blue, Avalonia.Media.Colors.Green, Avalonia.Media.Colors.Red };

		public GenderIconVM()
		{
			ZeroToOne = 0;
		}

		public double ZeroToOne
		{
			get => _zeroToOne;
			set
			{
				this.RaiseAndSetIfChanged(ref _zeroToOne, value);

				Rotate1 = 360 * _zeroToOne;

				Rotate2 = intermediate(Rotate1, Rotations, r => r);
				LegTranslateY = intermediate(Rotate1, LegTranslatesY, r => r);
				LegTranslateX = intermediate(Rotate1, LegTranslatesX, r => r);
				Width = intermediate(Rotate1, Widths, r => r);
				CanvasLeft = (100 - Width) / 2;


				var resultRed = intermediate(Rotate1, Colors, c => c.R);
				var resultGreen = intermediate(Rotate1, Colors, c => c.G);
				var resultBlue = intermediate(Rotate1, Colors, c => c.B);

				FillColor = new SolidColorBrush(new Color(0xff, (byte)resultRed, (byte)resultGreen, (byte)resultBlue));

				this.RaisePropertyChanged(nameof(FillColor));
				this.RaisePropertyChanged(nameof(Rotate1));
				this.RaisePropertyChanged(nameof(Rotate2));
				this.RaisePropertyChanged(nameof(CanvasLeft));
				this.RaisePropertyChanged(nameof(Width));
				this.RaisePropertyChanged(nameof(LegTranslateX));
				this.RaisePropertyChanged(nameof(LegTranslateY));
			}
		}


		private readonly double[] breaks = { 0, 135, 225 };

		private double intermediate<T>(double rotation, T[] values, Func<T, double> selector)
		{
			rotation %= 360;
			if (rotation < 0) rotation += 360;

			int i1 = breaks.Length - 1;

			while (rotation < breaks[i1])
				i1--;

			int i2 = (i1 + 1) % breaks.Length;

			var arcAngle = breaks[i2] - breaks[i1];
			if (arcAngle < 0) arcAngle += 360;

			double p = (rotation - breaks[i1]) / arcAngle;

			return selector(values[i1]) + (selector(values[i2]) - selector(values[i1])) * p;
		}
		public SolidColorBrush? FillColor { get; private set; }
		public double Rotate1 { get; private set; }
		public double Rotate2 { get; private set; }
		public double CanvasLeft { get; private set; }
		public double Width { get; private set; }
		public double LegTranslateX { get; private set; }
		public double LegTranslateY { get; private set; }
	}
}
