using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using static System.Double;

namespace VisualStudioFractals
{
    /// <summary>
    /// This page draws fractals onto a canvas.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Properties

        /// <summary>
        /// The maximum number of iterations.
        /// </summary>
        private const int MaxIterations = 100;

        /// <summary>
        /// The color parameter.
        /// </summary>
        private double ColorParameter { get; set; }

        /// <summary>
        /// The motion parameter.
        /// </summary>
        private double MotionParameter { get; set; }

        /// <summary>
        /// Gets or sets the fractal algorithm.
        /// </summary>
        private IFractalAlgorithm Algorithm { get; }

        /// <summary>
        /// Gets or sets the coloring technique.
        /// </summary>
        /// <value>
        /// The coloring technique.
        /// </value>
        private IColoringTechnique[] ColoringTechniques { get; }

        /// <summary>
        /// Gets the fractal painter.
        /// </summary>
        private FractalPainter FractalPainter { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            this.FractalPainter = new FractalPainter((int)this.ActualWidth, (int)this.ActualHeight);
            var coreWindow = CoreWindow.GetForCurrentThread();
            coreWindow.KeyDown += OnKeyDown;
            coreWindow.PointerPressed += OnPointerPressed;

            this.ColorParameter = 0;
            this.MotionParameter = 0;
            this.Algorithm = new JuliaFractalAlgorithm(MaxIterations);
            this.ColoringTechniques = new IColoringTechnique[] { new fractalsColoringTechnique() };
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Draws the fractal asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task DrawFractalAsync()
        {
            if (Math.Abs(this.ActualWidth) < Epsilon ||
                Math.Abs(this.ActualHeight) < Epsilon)
                return;

            var started = DateTime.Now;

            // resizes the fractal painter just in case the image size changed since the last draw.
            this.FractalPainter.Resize((int)this.ActualWidth, (int)this.ActualHeight);

            // draw the fractal and return the bitmap array.
            var bitmapArray = FractalPainter.Draw(
                this.ColorParameter,
                this.MotionParameter,
                this.Algorithm,
                this.ColoringTechniques,
                colors => colors.First());

            // create a bitmap and assign the background.
            await CreateBitmapAsync(bitmapArray);

            // logs the elapsed time.
            var ended = DateTime.Now;

            this.Elapsed.Text = $"Elapsed: {ended.Subtract(started).TotalSeconds:n2}s";
            this.Color.Text = $"[C] Color: {this.ColorParameter:n2}";
            this.Motion.Text = $"[M] Motion: {this.MotionParameter:n2}";
        }

        /// <summary>
        /// Creates the bitmap asynchronously.
        /// </summary>
        /// <param name="imageArray">The image array.</param>
        /// <returns></returns>
        private async Task CreateBitmapAsync(byte[] imageArray)
        {
            var bitmap = new WriteableBitmap(this.FractalPainter.Width, this.FractalPainter.Height);
            var bitmapStream = bitmap.PixelBuffer.AsStream();

            using (var stream = bitmap.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(imageArray, 0, imageArray.Length);
            }

            this.Background.Source = bitmap;
            bitmapStream.Dispose();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Called when the page loaded.
        /// </summary>
        /// <param name="sender">The coreWindow.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await this.DrawFractalAsync();
        }

        /// <summary>
        /// Called when the page size changed.
        /// </summary>
        /// <param name="sender">The coreWindow.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        private async void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            await this.DrawFractalAsync();
        }

        /// <summary>
        /// Called when the user pressed a key down.
        /// </summary>
        /// <param name="coreWindow">The coreWindow.</param>
        /// <param name="e">The <see cref="KeyRoutedEventArgs"/> instance containing the event data.</param>
        private async void OnKeyDown(CoreWindow coreWindow, KeyEventArgs e)
        {
            switch (e.VirtualKey)
            {
                case VirtualKey.C:
                    this.ColorParameter += 0.05;

                    if (this.ColorParameter >= 1)
                        this.ColorParameter = 0;
                    break;

                case VirtualKey.M:
                    this.MotionParameter += 0.05;

                    if (this.MotionParameter >= 1)
                        this.MotionParameter = 0;
                    break;
            }

            await this.DrawFractalAsync();
        }

        /// <summary>
        /// Called when the user pressed a key down.
        /// </summary>
        /// <param name="coreWindow">The coreWindow.</param>
        /// <param name="e">The <see cref="KeyRoutedEventArgs"/> instance containing the event data.</param>
        private async void OnPointerPressed(CoreWindow coreWindow, PointerEventArgs e)
        {
            this.MotionParameter += 0.05;

            if (this.MotionParameter >= 1)
                this.MotionParameter = 0;

            await this.DrawFractalAsync();
        }
        #endregion
    }
}