using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace GenderSelector.Controls
{
	public partial class Slider2D : UserControl
	{
		public static readonly StyledProperty<double> MinimumXProperty =
		AvaloniaProperty.Register<Slider2D, double>(nameof(MinimumX), 0);

		public static readonly StyledProperty<double> MinimumYProperty =
		AvaloniaProperty.Register<Slider2D, double>(nameof(MinimumY), 0);

		public static readonly StyledProperty<double> MaximumXProperty =
		AvaloniaProperty.Register<Slider2D, double>(nameof(MaximumX), 1);

		public static readonly StyledProperty<double> MaximumYProperty =
		AvaloniaProperty.Register<Slider2D, double>(nameof(MaximumY), 1);

		public static readonly StyledProperty<double> ValueXProperty =
		AvaloniaProperty.Register<Slider2D, double>(nameof(ValueX), 1);

		public static readonly StyledProperty<double> ValueYProperty =
		AvaloniaProperty.Register<Slider2D, double>(nameof(ValueY), 1);

		public double MinimumX { get => GetValue(MinimumXProperty); set => SetValue(MinimumXProperty, value); }
		public double MinimumY { get => GetValue(MinimumYProperty); set => SetValue(MinimumYProperty, value); }
		public double MaximumX { get => GetValue(MaximumXProperty); set => SetValue(MaximumXProperty, value); }
		public double MaximumY { get => GetValue(MaximumYProperty); set => SetValue(MaximumYProperty, value); }
		public double ValueX { get => GetValue(ValueXProperty); set => SetValue(ValueXProperty, value); }
		public double ValueY { get => GetValue(ValueYProperty); set => SetValue(ValueYProperty, value); }

		private double MaxSliderWidth => canvas.Bounds.Width - thumb.Bounds.Width;
		private double MaxSliderHeight => canvas.Bounds.Height - thumb.Bounds.Height;

		public Slider2D()
		{
			InitializeComponent();

			Thumb.DragDeltaEvent.AddClassHandler<Slider2D>((x, e) => x.OnDragDeltaEvent(e), RoutingStrategies.Bubble);
			TappedEvent.AddClassHandler<Slider2D>((x, e) => x.OnTappedEvent(e), RoutingStrategies.Bubble);
			ValueXProperty.Changed.AddClassHandler<Slider2D>((x, e) => x.OnValueXChanged(e));
			ValueYProperty.Changed.AddClassHandler<Slider2D>((x, e) => x.OnValueYChanged(e));
			BoundsProperty.Changed.AddClassHandler<Slider2D>((x, e) => { x.SetXInternal(ValueX); x.SetYInternal(ValueY); });
		}

		protected virtual void OnValueXChanged(AvaloniaPropertyChangedEventArgs e)
		{
			if (e.NewValue is not double newVal) return;
			if (newVal > MaximumX || newVal < MinimumX) throw new ArgumentOutOfRangeException();

			SetXInternal(newVal);
		}

		protected virtual void OnValueYChanged(AvaloniaPropertyChangedEventArgs e)
		{
			if (e.NewValue is not double newVal) return;
			if (newVal > MaximumY || newVal < MinimumY) throw new ArgumentOutOfRangeException();

			SetYInternal(newVal);
		}

		protected virtual void OnTappedEvent(TappedEventArgs e)
		{
			var point = e.GetPosition(canvas);
			var top = Math.Max(Math.Min(MaxSliderWidth, point.Y - thumb.Bounds.Height / 2), 0);
			var left = Math.Max(Math.Min(MaxSliderHeight, point.X - thumb.Bounds.Width / 2), 0);

			ValueX = (MaximumX - MinimumX) * left / MaxSliderWidth + MinimumX;
			ValueY = (MaximumY - MinimumY) * top / MaxSliderHeight + MinimumY;
		}

		protected virtual void OnDragDeltaEvent(VectorEventArgs e)
		{
			var top = Math.Max(Math.Min(MaxSliderWidth, Canvas.GetTop(thumb) + e.Vector.Y), 0);
			var left = Math.Max(Math.Min(MaxSliderHeight, Canvas.GetLeft(thumb) + e.Vector.X), 0);

			ValueX = (MaximumX - MinimumX) * left / MaxSliderWidth + MinimumX;
			ValueY = (MaximumY - MinimumY) * top / MaxSliderHeight + MinimumY;
		}

		private void SetXInternal(double newVal)
		{
			var left = (newVal - MinimumX) * MaxSliderWidth / (MaximumX - MinimumX);
			Canvas.SetLeft(thumb, left);
		}

		private void SetYInternal(double newVal)
		{
			var top = (newVal - MinimumY) * MaxSliderHeight / (MaximumY - MinimumY);
			Canvas.SetTop(thumb, top);
		}
	}
}
