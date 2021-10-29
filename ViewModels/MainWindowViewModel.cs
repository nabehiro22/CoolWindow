using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.ComponentModel;
using System.Reactive.Disposables;

// 以下、デフォルト状態から追加されたusing
using System;
using System.Reactive.Linq;
using System.Windows;

namespace CoolWindow.ViewModels
{
	public class MainWindowViewModel : BindableBase, INotifyPropertyChanged
	{
		/// <summary>
		/// タイトルバー
		/// </summary>
		public ReactivePropertySlim<string> Title { get; } = new("Title");

		/// <summary>
		/// Disposeが必要な処理をまとめてやる
		/// </summary>
		private CompositeDisposable Disposable { get; } = new();

		/// <summary>
		/// 最大化、通常サイズのボタンデザイン切り替え
		/// </summary>
		public ReactivePropertySlim<string> ButtonStyle { get; } = new("1");

		/// <summary>
		/// 最大化、通常のツールチップ文字列
		/// </summary>
		public ReactivePropertySlim<string> ToolTip { get; } = new("最大化");

		/// <summary>
		/// 最小化ボタンが押された時
		/// </summary>
		public ReactiveCommand WindowMinimum { get; } = new();

		/// <summary>
		/// 最大化、通常サイズのボタンが押された時
		/// </summary>
		public ReactiveCommand WindowSize { get; } = new();

		/// <summary>
		/// ウインドウを閉じるボタンが押された時
		/// </summary>
		public ReactiveCommand WindowClose { get; } = new();

		/// <summary>
		/// ウインドウのサイズが変更されるイベントが発生した時(最大化と通常の変化)
		/// </summary>
		public ReactiveCommand SizeChangedCommand { get; } = new();

		/// <summary>
		/// ウィンドウを閉じるイベントが発生した時
		/// </summary>
		public ReactiveCommand ClosedCommand { get; } = new();

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MainWindowViewModel()
		{
			// 最小化ボタンが押された
			_ = WindowMinimum.Subscribe(_ => Application.Current.MainWindow.WindowState = WindowState.Minimized).AddTo(Disposable);
			// 最大化、通常サイズボタンが押された
			_ = WindowSize.Subscribe(_ => Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal).AddTo(Disposable);
			// 閉じるボタンが押された
			_ = WindowClose.Subscribe(_ => Window.GetWindow(Application.Current.MainWindow).Close()).AddTo(Disposable);
			// ウィンドウサイズが変わった
			_ = SizeChangedCommand.Subscribe(_ =>
			{
				ButtonStyle.Value = Application.Current.MainWindow.WindowState == WindowState.Normal ? "1" : "2";
				ToolTip.Value = Application.Current.MainWindow.WindowState == WindowState.Normal ? "最大化" : "元に戻す";
			}).AddTo(Disposable);
			// ウィンドウが閉じるイベント
			_ = ClosedCommand.Subscribe(Close).AddTo(Disposable);
		}

		/// <summary>
		/// アプリが閉じられる時
		/// </summary>
		private void Close()
		{
			Disposable.Dispose();
		}
	}
}
