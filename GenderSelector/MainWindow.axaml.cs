using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;

namespace GenderSelector
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainVM();
		}
	}

	class MainVM : ReactiveObject
	{
		private readonly Color ZeroColor = Colors.Blue;
		private readonly Color OneColor = Colors.Red;
		private decimal _zeroToOne;

		public MainVM()
		{
			ZeroToOne = 0;
		}

		public decimal ZeroToOne
		{
			get => _zeroToOne;
			set
			{
				this.RaiseAndSetIfChanged(ref _zeroToOne, value);

				var resultRed = ZeroColor.R + _zeroToOne * (OneColor.R - ZeroColor.R);
				var resultGreen = ZeroColor.G + _zeroToOne * (OneColor.G - ZeroColor.G);
				var resultBlue = ZeroColor.B + _zeroToOne * (OneColor.B - ZeroColor.B);

				FillColor = new SolidColorBrush(new Color(0xff, (byte)resultRed, (byte)resultGreen, (byte)resultBlue));

				Rotate1 = (double)((360 - 225) * _zeroToOne + 225);
				Rotate2 = (double)(45 - 45 * _zeroToOne);
				Rotate3 = -Rotate2;

				this.RaisePropertyChanged(nameof(FillColor));
				this.RaisePropertyChanged(nameof(Rotate1));
				this.RaisePropertyChanged(nameof(Rotate2));
				this.RaisePropertyChanged(nameof(Rotate3));
			}
		}

		public SolidColorBrush? FillColor { get; private set; }
		public double Rotate1 { get; private set; }
		public double Rotate2 { get; private set; }
		public double Rotate3 { get; private set; }
	}
}