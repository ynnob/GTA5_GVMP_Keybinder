using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GTA5_Keybinder_Bonnyfication.Controls
{
    class FadingContentControl : ContentControl
    {
        Storyboard _fadeOut;
        Storyboard _fadeIn;
        Rectangle _oldContent;
        ContentPresenter _mainArea;

        public bool FadeVertically
        {
            get { return (bool)GetValue(FadeVerticallyProperty); }
            set { SetValue(FadeVerticallyProperty, value); }
        }

        public static readonly DependencyProperty FadeVerticallyProperty =
            DependencyProperty.Register("FadeVertically", typeof(bool), typeof(FadingContentControl), new PropertyMetadata(false, new PropertyChangedCallback(FadeVerticalCallBack)));

        private static void FadeVerticalCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FadingContentControl control = (FadingContentControl)d;
            control.InitStoryBoards((bool)e.NewValue);
        }

        public bool FadeVisibility
        {
            get { return (bool)GetValue(FadeVisibilityProperty); }
            set { SetValue(FadeVisibilityProperty, value); }
        }

        public static readonly DependencyProperty FadeVisibilityProperty =
            DependencyProperty.Register("FadeVisibility", typeof(bool), typeof(FadingContentControl), new PropertyMetadata(true, FadeVisibilityCallBack));

        private static void FadeVisibilityCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FadingContentControl control = (FadingContentControl)d;
            control.ChangeVisibility((bool)e.NewValue);
        }


        public FadingContentControl()
        {
            InitStoryBoards(FadeVertically);
            this.RenderTransform = new TranslateTransform();
        }

        private void InitStoryBoards(bool fadeVertical)
        {
            Contract.Ensures(_fadeIn != null, "Failed to init fade in storyboard");
            Contract.Ensures(_fadeOut != null, "Failed to init fade out storyboard");
            _fadeIn = fadeVertical ? (Storyboard)FindResource("fadeInVertical") : (Storyboard)FindResource("fadeIn");
            _fadeOut = fadeVertical ? (Storyboard)FindResource("fadeOutVertical") : (Storyboard)FindResource("fadeOut");
        }

        private void ChangeVisibility(bool visible)
        {
            if (!visible)
            {
                Storyboard fadeOutClone = _fadeOut.Clone();
                fadeOutClone.Completed += (s, e) => { this.Visibility = System.Windows.Visibility.Collapsed; };
                fadeOutClone.Begin(this);
            }
            else
            {
                Storyboard fadeInClone = _fadeIn.Clone();
                this.Visibility = System.Windows.Visibility.Visible;
                fadeInClone.Begin(this);
            }
        }

        private Brush GetVisualBrush(Visual visual)
        {
            Contract.Requires(visual != null, "Visual is null");
            var target = new RenderTargetBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            target.Render(visual);
            var brush = new ImageBrush(target);
            brush.Freeze();
            return brush;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (oldContent != null && newContent != null)
            {
                _oldContent.Fill = GetVisualBrush(_mainArea);
                _oldContent.Visibility = System.Windows.Visibility.Visible;
                _mainArea.Visibility = System.Windows.Visibility.Collapsed;
                Storyboard fadeOutClone = _fadeOut.Clone();
                Storyboard fadeInClone = _fadeIn.Clone();
                fadeOutClone.Completed += (s, e) =>
                {
                    fadeInClone.Begin(_mainArea);
                    _oldContent.Visibility = System.Windows.Visibility.Collapsed;
                    _mainArea.Visibility = System.Windows.Visibility.Visible;
                };
                fadeOutClone.Begin(_oldContent);
            }

            base.OnContentChanged(oldContent, newContent);
        }

        public override void OnApplyTemplate()
        {
            Contract.Ensures(_oldContent != null, "Failed to init paint area");
            Contract.Ensures(_mainArea != null, "Failed to init main area");
            base.OnApplyTemplate();
            _oldContent = (Rectangle)Template.FindName("paintArea", this);
            _mainArea = (ContentPresenter)Template.FindName("mainArea", this);
        }
    }
}
