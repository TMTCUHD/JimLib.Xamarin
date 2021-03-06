﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Mvvm;
using JimBobBennett.JimLib.Xamarin.Extensions;
using JimBobBennett.JimLib.Xamarin.Mvvm;
using JimBobBennett.JimLib.Xamarin.Navigation;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Views
{
    [ContentProperty("PortraitContent")]
    public class BaseContentPage : ContentPage, IView
    {
        public static readonly BindableProperty OpacityBackgroundColorProperty =
            BindableProperty.Create<BaseContentPage, Color>(p => p.OpacityBackgroundColor, Color.White,
            propertyChanged: (s, o, n) => ((BaseContentPage)s)._opacityGrid.BackgroundColor = n);

        public static readonly BindableProperty BusyIndicatorBackgroundColorProperty =
            BindableProperty.Create<BaseContentPage, Color>(p => p.BusyIndicatorBackgroundColor, Color.FromRgba(0, 0, 0, 0.5),
                propertyChanged: (s, o, n) => ((BaseContentPage) s)._activityFrame.BackgroundColor = n);

        public static readonly BindableProperty ShowSeperatorProperty =
            BindableProperty.Create<BaseContentPage, bool>(p => p.ShowSeperator, false,
            propertyChanged: (s, o, n) => ((BaseContentPage)s)._seperator.IsVisible = n);

        public static readonly BindableProperty BackgroundImageSourceProperty =
            BindableProperty.Create<BaseContentPage, ImageSource>(p => p.BackgroundImageSource, null,
            propertyChanged: (s, o, n) => ((BaseContentPage)s)._backgroundImage.Source = n);

        public static readonly BindableProperty BackgroundImageOpacityProperty =
            BindableProperty.Create<BaseContentPage, double>(p => p.BackgroundImageOpacity, 1,
            propertyChanged: (s, o, n) => ((BaseContentPage)s)._backgroundImage.Opacity = n);

        public static readonly BindableProperty BackgroundImageAspectProperty =
            BindableProperty.Create<BaseContentPage, Aspect>(p => p.BackgroundImageAspect, Aspect.AspectFill,
            propertyChanged: (s, o, n) => ((BaseContentPage)s)._backgroundImage.Aspect = n);

        public static readonly BindableProperty ContentPaddingProperty =
            BindableProperty.Create<BaseContentPage, Thickness>(p => p.ContentPadding, new Thickness(0),
            propertyChanged: (s, o, n) => ((BaseContentPage)s)._contentView.Padding = n);
        
        public Color OpacityBackgroundColor
        {
            get { return (Color)GetValue(OpacityBackgroundColorProperty); }
            set { SetValue(OpacityBackgroundColorProperty, value); }
        }

        public Color BusyIndicatorBackgroundColor
        {
            get { return (Color)GetValue(BusyIndicatorBackgroundColorProperty); }
            set { SetValue(BusyIndicatorBackgroundColorProperty, value); }
        }

        public bool ShowSeperator
        {
            get { return (bool)GetValue(ShowSeperatorProperty); }
            set { SetValue(ShowSeperatorProperty, value); }
        }

        public ImageSource BackgroundImageSource
        {
            get { return (ImageSource)GetValue(BackgroundImageSourceProperty); }
            set { SetValue(BackgroundImageSourceProperty, value); }
        }

        public double BackgroundImageOpacity
        {
            get { return (double)GetValue(BackgroundImageOpacityProperty); }
            set { SetValue(BackgroundImageOpacityProperty, value); }
        }

        public Aspect BackgroundImageAspect
        {
            get { return (Aspect)GetValue(BackgroundImageAspectProperty); }
            set { SetValue(BackgroundImageAspectProperty, value); }
        }

        public Thickness ContentPadding
        {
            get { return (Thickness)GetValue(ContentPaddingProperty); }
            set { SetValue(ContentPaddingProperty, value); }
        }

        public INavigationStackManager NavigationStackManager { get; protected set; }

        private readonly ContentView _contentView;
        private readonly Grid _opacityGrid;
        private readonly Grid _mainGrid;
        private readonly ActivityIndicator _activityIndicator;
        private readonly Label _activityLabel;
        private readonly Frame _activityFrame;
        private readonly ProgressBar _seperator;
        private readonly Image _backgroundImage;

        protected BaseContentPage()
        {
            _contentView = new ContentView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            _backgroundImage = new Image
            {
                Source = BackgroundImageSource,
                Opacity = BackgroundImageOpacity,
                Aspect = BackgroundImageAspect
            };

            _opacityGrid = CreateFillGrid();
            _opacityGrid.IsVisible = false;
            _opacityGrid.Opacity = 0.75;
            _opacityGrid.BackgroundColor = OpacityBackgroundColor;

            var activityGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            activityGrid.AddAutoRowDefinition();
            activityGrid.AddAutoRowDefinition();
            activityGrid.AddAutoColumnDefinition();
            
            _activityIndicator = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Color = Color.White
            };

            _activityLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                LineBreakMode = LineBreakMode.WordWrap
            };

            Grid.SetRow(_activityIndicator, 0);
            Grid.SetRow(_activityLabel, 1);

            activityGrid.Children.Add(_activityIndicator);
            activityGrid.Children.Add(_activityLabel);

            _activityFrame = new Frame
            {
                BackgroundColor = BusyIndicatorBackgroundColor,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = activityGrid,
                IsVisible = false
            };

            _seperator = new ProgressBar
            {
                VerticalOptions = LayoutOptions.Fill,
                IsVisible = ShowSeperator
            };

            _mainGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0
            };

            _mainGrid.AddStarColumnDefinition();
            _mainGrid.AddAutoRowDefinition();
            _mainGrid.AddStarRowDefinition();

            _mainGrid.Children.Add(_backgroundImage);
            _mainGrid.Children.Add(_contentView);
            _mainGrid.Children.Add(_opacityGrid);
            _mainGrid.Children.Add(_activityFrame);
            _mainGrid.Children.Add(_seperator);
            
            Grid.SetRow(_contentView, 1);
            Grid.SetRow(_opacityGrid, 1);
            Grid.SetRow(_activityFrame, 1);
            Grid.SetRow(_backgroundImage, 1);
            Grid.SetRow(_seperator, 0);
            
            Content = _mainGrid;
            _mainGrid.IsVisible = false;
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
        }

        protected BaseContentPage(object viewModel, INavigationStackManager navigationStackManager)
            : this()
        {
            NavigationStackManager = navigationStackManager;
            SetViewModel(viewModel);
        }

        private static Grid CreateFillGrid()
        {
            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            return grid;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is IBusy)
            {
                SetBinding(IsBusyProperty, new Binding("IsBusy"));
            }
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (child != _mainGrid)
            {
                Content = _mainGrid;
                _contentView.Content = null;

                var childView = child as View;
                if (childView != null)
                {
                    _contentView.Content = childView;
                    _portraitContent = childView;
                }
            }
        }

        public async Task<string> GetOptionFromUserAsync(string title, string cancel, string destruction, params string[] buttons)
        {
            if (DisplayAlertFunc != null)
                return await DisplayAlertFunc(title, cancel, destruction, buttons);

            return await DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public async Task<bool> ShowAlertAsync(string title, string message, string cancel, string accept = null)
        {
            if (title == null)
                throw new ArgumentNullException("title");

            if (cancel == null)
                throw new ArgumentNullException("title");

            return await DisplayAlert(title, message, accept, cancel);
        }

        protected void SetViewModel(object viewModel)
        {
            BindingContext = viewModel;

            var vm = viewModel as IContentPageViewModelBase;
            if (vm != null)
            {
                vm.View = this;
                vm.PropertyChanged += ViewModelOnPropertyChanged;

                if (NavigationStackManager != null)
                    vm.NeedClose += (s, e) =>
                        {
                            if (NavigationStackManager != null)
                                Device.BeginInvokeOnMainThread(async () => await NavigationStackManager.PopAsync());
                        };
            }
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var busy = BindingContext as IBusy;

            if (busy == null) return;
            if (e.PropertyNameMatches(() => busy.IsBusy))
            {
                _opacityGrid.IsVisible = busy.IsBusy;
                _activityFrame.IsVisible = busy.IsBusy;
                _activityIndicator.IsRunning = busy.IsBusy;
            }

            if (e.PropertyNameMatches(() => busy.BusyMessage))
                _activityLabel.Text = busy.BusyMessage;
        }

        private View _portraitContent;
        private View _landscapeContent;

        public View PortraitContent
        {
            get { return _portraitContent; }
            set
            {
                if (_portraitContent == value) return;

                OnPropertyChanging();
                _portraitContent = value;
                OnPropertyChanged();

                SetContent();
            }
        }

        public View LandscapeContent
        {
            get { return _landscapeContent; }
            set
            {
                if (_landscapeContent == value) return;

                OnPropertyChanging();
                _landscapeContent = value;
                OnPropertyChanged();

                SetContent();
            }
        }

        private void SetContent()
        {
            if (_portraitContent == null && _landscapeContent == null)
            {
                // do nothing
            }
            else if (_portraitContent != null && _landscapeContent == null)
                _contentView.Content = _portraitContent;
            else if (_portraitContent == null && _landscapeContent != null)
                _contentView.Content = _landscapeContent;
            else
                _contentView.Content = Orientation == Orientation.Landscape ? _landscapeContent : _portraitContent;
        }

        protected Orientation Orientation { get; private set; }

        internal void OrientationChanged(Orientation orientation)
        {
            if (Orientation == orientation) return;

            Orientation = orientation;

            SetContent();
            OnOrientationChanged();
            RaiseOrientationChanged();
        }

        private void RaiseOrientationChanged()
        {
            var vm = BindingContext as IContentPageViewModelBase;

            if (vm != null)
                vm.OnOrientationChanged(Orientation);
        }

        protected virtual void OnOrientationChanged()
        {
            
        }

        protected internal virtual void OrientationChanging()
        {
            _contentView.Content = null;
        }
        
        private void SetPadding()
        {
            if (ParentIsNavigationPage(Parent) && Padding == new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0))
                Padding = new Thickness(0, 0, 0, 0);
        }

        private static bool ParentIsNavigationPage(Element parent)
        {
            while (true)
            {
                if (parent == null) return false;
                if (parent is NavigationPage) return true;
                if (parent.Parent == null) return false;

                parent = parent.Parent;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetPadding();

            RaiseOrientationChanged();
            _mainGrid.IsVisible = true;
        }

        internal Func<string, string, string, string[], Task<string>> DisplayAlertFunc { get; set; }
    }
}
