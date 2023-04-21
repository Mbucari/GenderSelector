using Avalonia.Controls;
using ReactiveUI;
using System;

namespace GenderSelector
{
	public partial class MainWindow : Window
	{
		private IDisposable? xObserver;
		private IDisposable? yObserver;
		public MainWindow()
		{
			InitializeComponent();			
			queer.IsCheckedChanged += Queer_IsCheckedChanged;
			Queer_IsCheckedChanged(null, null);
		}

		private void Queer_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs? e)
		{
			if (queer.IsChecked is true)
			{
				xObserver?.Dispose();
				yObserver?.Dispose();
			}
			else
			{
				slider.ValueX = slider.ValueY;
				xObserver = slider.ObservableForProperty(s => s.ValueX).Subscribe(d => d.Sender.ValueY = d.Value);
				yObserver = slider.ObservableForProperty(s => s.ValueY).Subscribe(d => d.Sender.ValueX = d.Value);
			}
		}
	}
}