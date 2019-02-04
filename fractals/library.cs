using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;

namespace VisualStudioFractals
{
    /// <summary>
    /// Represents a color in the RGB space, as double values.
    /// Each color coordinate (R, G, B) are represented using doubles.
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// The red channel.
        /// </summary>
        public double R;

        /// <summary>
        /// The green channel.
        /// </summary>
        public double G;

        /// <summary>
        /// The blue channel.
        /// </summary>
        public double B;

        /// <summary>
        /// The alpha channel.
        /// </summary>
        public double A;

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="g">The g.</param>
        /// <param name="b">The b.</param>
        public Color(double r, double g, double b) : this()
        {
            this.A = 1.0;
            this.R = MathExtensions.Clamp(r, 0.0, 1.0);
            this.G = MathExtensions.Clamp(g, 0.0, 1.0);
            this.B = MathExtensions.Clamp(b, 0.0, 1.0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct.
        /// </summary>
        /// <param name="a">The alpha channel.</param>
        /// <param name="r">The red channel.</param>
        /// <param name="g">The green channel.</param>
        /// <param name="b">The blue channel.</param>
        public Color(double a, double r, double g, double b)
        {
            this.A = MathExtensions.Clamp(a, 0.0, 1.0);
            this.R = MathExtensions.Clamp(r, 0.0, 1.0);
            this.G = MathExtensions.Clamp(g, 0.0, 1.0);
            this.B = MathExtensions.Clamp(b, 0.0, 1.0);
        }

        /// <summary>
        /// Creates a new color from a given hue.
        /// </summary>
        /// <param name="hue">The hue component.</param>
        /// <param name="a">The alpha channel.</param>
        /// <returns></returns>
        public static Color FromHue(double hue, double a = 1.0)
        {
            var r = Math.Abs(hue * 6.0 - 3.0) - 1.0;
            var g = 2.0 - Math.Abs(hue * 6.0 - 2.0);
            var b = 2.0 - Math.Abs(hue * 6.0 - 4.0);

            return new Color(a, r, g, b);
        }

        /// <summary>
        /// Creates a color from the HSV space.
        /// </summary>
        /// <param name="hue">The hue component.</param>
        /// <param name="saturation">The saturation component.</param>
        /// <param name="value">The value component.</param>
        /// <param name="a">The alpha channel.</param>
        /// <returns></returns>
        public static Color FromHsv(double hue, double saturation, double value, double a = 1.0)
        {
            var color = FromHue(hue);

            var r = ((color.R - 1.0) * saturation + 1.0) * value;
            var g = ((color.G - 1.0) * saturation + 1.0) * value;
            var b = ((color.B - 1.0) * saturation + 1.0) * value;

            return new Color(a, r, g, b);
        }

        /// <summary>
        /// Creates a color from the HSL space.
        /// </summary>
        /// <param name="hue">The hue component.</param>
        /// <param name="saturation">The saturation component.</param>
        /// <param name="lightness">The lightness component.</param>
        /// <param name="a">The alpha channel.</param>
        /// <returns></returns>
        public static Color FromHsl(double hue, double saturation, double lightness, double a = 1.0)
        {
            var rgb = FromHue(hue);
            var c = (1.0 - Math.Abs(2.0 * lightness - 1.0)) * saturation;
            var r = (rgb.R - 0.5) * c + lightness;
            var g = (rgb.G - 0.5) * c + lightness;
            var b = (rgb.B - 0.5) * c + lightness;

            return new Color(a, r, g, b);
        }

        /// <summary>
        /// Changes the contrast of the color.
        /// </summary>
        /// <param name="c">The contrast.</param>
        /// <returns>A new color.</returns>
        public Color Contrast(double c)
        {
            var t = 0.5 - c * 0.5;

            return new Color(
                A,
                R * c + t,
                G * c + t,
                B * c + t);
        }

        /// <summary>
        /// Changes the brightness of the color.
        /// </summary>
        /// <param name="b">The brightness.</param>
        /// <returns>A new color.</returns>
        public Color Brightness(double b)
        {
            return new Color(
                A,
                R + b,
                G + b,
                B + b);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Color operator *(Color left, Color right)
        {
            return new Color(
                left.A * right.A,
                left.R * right.R,
                left.G * right.G,
                left.B * right.B);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Color operator /(Color left, Color right)
        {
            return new Color(
                left.A / right.A,
                left.R / right.R,
                left.G / right.G,
                left.B / right.B);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Color operator *(Color left, double right)
        {
            return new Color(
                left.A * right,
                left.R * right,
                left.G * right,
                left.B * right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Color operator /(Color left, double right)
        {
            return new Color(
                left.A / right,
                left.R / right,
                left.G / right,
                left.B / right);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Color operator +(Color left, Color right)
        {
            return new Color(
                left.A + right.A,
                left.R + right.R,
                left.G + right.G,
                left.B + right.B);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Color operator -(Color left, Color right)
        {
            return new Color(
                left.A - right.A,
                left.R - right.R,
                left.G - right.G,
                left.B - right.B);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Color operator +(Color left, double right)
        {
            return new Color(
                left.A + right,
                left.R + right,
                left.G + right,
                left.B + right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Color operator -(Color left, double right)
        {
            return new Color(
                left.A - right,
                left.R - right,
                left.G - right,
                left.B - right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="right">The right.</param>
        /// <param name="left">The left.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Color operator -(double right, Color left)
        {
            return new Color(
                right - left.A,
                right - left.R,
                right - left.G,
                right - left.B);
        }

        /// <summary>
        /// Averages the specified colors.
        /// </summary>
        /// <param name="colors">The colors.</param>
        /// <returns>A new color.</returns>
        public static Color Average(params Color[] colors)
        {
            var r = 0.0;
            var g = 0.0;
            var b = 0.0;
            var a = 0.0;
            var count = (double)colors.Length;

            foreach (var color in colors)
            {
                a += color.A;
                r += color.R;
                g += color.G;
                b += color.B;
            }

            return new Color(a / count, r / count, g / count, b / count);
        }

        /// <summary>
        /// Averages the colors using a weighted sum.
        /// </summary>
        /// <param name="colors">The colors.</param>
        /// <returns>A new color.</returns>
        public static Color WeightedSum(params Tuple<Color, float>[] colors)
        {
            var r = 0.0;
            var g = 0.0;
            var b = 0.0;
            var a = 0.0;
            var count = (double)colors.Length;

            foreach (var color in colors)
            {
                a += color.Item1.A * color.Item2;
                r += color.Item1.R * color.Item2;
                g += color.Item1.G * color.Item2;
                b += color.Item1.B * color.Item2;
            }

            return new Color(a / count, r / count, g / count, b / count);
        }

        /// <summary>
        /// Clamps the specified minimum.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public Color Clamp(double min = 0.0, double max = 1.0)
        {
            return new Color(
                MathExtensions.Clamp(this.A, min, max),
                MathExtensions.Clamp(this.R, min, max),
                MathExtensions.Clamp(this.G, min, max),
                MathExtensions.Clamp(this.B, min, max));
        }

        /// <summary>
        /// Converts this instance to a <see cref="Windows.UI.Color"/>
        /// </summary>
        /// <returns>A windows color.</returns>
        public Windows.UI.Color ToSystemColor()
        {
            return Windows.UI.Color.FromArgb((byte)(A * 255), (byte)(R * 255), (byte)(G * 255), (byte)(B * 255));
        }

        /// <summary>
        /// Returns a <see cref="String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"({(int)(A * 255)}, {(int)(R * 255)}, {(int)(G * 255)}, {(int)(B * 255)})";
        }
    }

    /// <summary>
    /// Represents a complex number that has been iterated a number of times using a <see cref="IFractalAlgorithm"/>.
    /// </summary>
    public struct FractalAlgorithmResult
    {
        /// <summary>
        /// Gets or sets the original number.
        /// </summary>
        /// <value>
        /// The original number.
        /// </value>
        public Complex OriginalNumber { get; set; }

        /// <summary>
        /// Gets or sets the final number.
        /// </summary>
        /// <value>
        /// The final number.
        /// </value>
        public Complex FinalNumber { get; set; }

        /// <summary>
        /// Gets or sets the iterations.
        /// </summary>
        /// <value>
        /// The iterations.
        /// </value>
        public int Iterations { get; set; }

        /// <summary>
        /// Gets or sets the maximum iterations.
        /// </summary>
        /// <value>
        /// The maximum iterations.
        /// </value>
        public int MaxIterations { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FractalAlgorithmResult"/> struct.
        /// </summary>
        /// <param name="originalNumber">The original number.</param>
        /// <param name="finalNumber">The final number.</param>
        /// <param name="iterations">The iterations.</param>
        /// <param name="maxIterations">The maximum iterations.</param>
        public FractalAlgorithmResult(Complex originalNumber, Complex finalNumber, int iterations, int maxIterations)
        {
            OriginalNumber = originalNumber;
            FinalNumber = finalNumber;
            Iterations = iterations;
            MaxIterations = maxIterations;
        }
    }

    /// <summary>
    /// Represents a slice of a memory array.
    /// </summary>
    /// <typeparam name="T">The inner array type.</typeparam>
    public class MemorySlice<T>
    {
        /// <summary>
        /// Gets the memory.
        /// </summary>
        /// <value>
        /// The memory.
        /// </value>
        public Memory<T> Memory { get; }

        /// <summary>
        /// Gets the span.
        /// </summary>
        /// <value>
        /// The span.
        /// </value>
        public Span<T> Span => this.Memory.Span;

        /// <summary>
        /// Gets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public int Start { get; }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemorySlice{T}" /> class.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        public MemorySlice(Memory<T> array, int start, int length)
        {
            this.Start = start;
            this.Length = length;
            this.Memory = array.Slice(start, length);
        }
    }

    /// <summary>
    /// Provides extension methods for the class <see cref="Complex"/>
    /// </summary>
    public static class ComplexExtensions
    {
        /// <summary>
        /// Raises a complex number to a double power.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="power">The power.</param>
        /// <returns>A raised complex number.</returns>
        public static Complex Pow(this Complex number, double power)
        {
            var exp = Math.Exp(Math.Log(number.Magnitude) * power);
            var phase = number.Phase * power;

            return new Complex(Math.Cos(phase), Math.Sin(phase)) * exp;
        }

        /// <summary>
        /// Returns a new complex number with the absolute value of both components.
        /// </summary>
        /// <returns>A new complex number.</returns>
        public static Complex Abs(this Complex number)
        {
            return new Complex(Math.Abs(number.Real), Math.Abs(number.Imaginary));
        }
    }

    /// <summary>
    /// Provides extension methods for arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Slices the array in even spans of memory.
        /// </summary>
        /// <typeparam name="T">The array item type.</typeparam>
        /// <param name="array">The array to be sliced.</param>
        /// <param name="slices">The slices.</param>
        /// <returns></returns>
        public static List<MemorySlice<T>> SliceArray<T>(this T[] array, int slices)
        {
            var memorySlices = new List<MemorySlice<T>>();
            var bytesPerSpan = array.Length / 4 / slices * 4;
            var memory = new Memory<T>(array);

            for (var p = 0; p < slices; p++)
            {
                var start = p * bytesPerSpan;
                var length = Math.Min(bytesPerSpan, array.Length - start);
                memorySlices.Add(new MemorySlice<T>(memory, start, length));
            }

            return memorySlices;
        }

    }

    /// <summary>
    /// Provides math methods.
    /// </summary>
    public static class MathExtensions
    {
        /// <summary>
        /// Clamps the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static double Clamp(double value, double min, double max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }
    }

    /// <summary>
    /// Provides an interface for different fractal algorithms.
    /// </summary>
    public interface IFractalAlgorithm
    {
        /// <summary>
        /// Calculates the fractal formula for a specified number in the complex coordinate system.
        /// </summary>
        /// <param name="number">The fractal number.</param>
        /// <param name="motion">The motion parameter.</param>
        /// <returns>The result of applying the fractal algorithm to a given complex number.</returns>
        FractalAlgorithmResult Calculate(Complex number, double motion);
    }

    /// <summary>
    /// Provides an interface for a coloring technique.
    /// </summary>
    public interface IColoringTechnique
    {
        /// <summary>
        /// Colorizes the specified algorithm result.
        /// </summary>
        /// <param name="algorithmResult">The algorithm result.</param>
        /// <param name="color">The color parameter.</param>
        /// <returns>A color calculated from the algorithm result.</returns>
        Color Colorize(FractalAlgorithmResult algorithmResult, double color);
    }

    /// <summary>
    /// Calculates the Julia set algorithm for a given complex number.
    /// </summary>
    /// <seealso cref="IFractalAlgorithm" />
    public class JuliaFractalAlgorithm : IFractalAlgorithm
    {
        #region Properties

        /// <summary>
        /// Gets the iterations.
        /// </summary>
        /// <value>
        /// The iterations.
        /// </value>
        private int Iterations { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JuliaFractalAlgorithm"/> class.
        /// </summary>
        /// <param name="iterations">The iterations.</param>
        public JuliaFractalAlgorithm(int iterations)
        {
            Iterations = iterations;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates the fractal formula for a specified number in the complex coordinate system.
        /// </summary>
        /// <param name="number">The fractal number.</param>
        /// <param name="motion">The motion parameter.</param>
        /// <returns>
        /// The result of applying the fractal algorithm to a given complex number.
        /// </returns>
        public FractalAlgorithmResult Calculate(Complex number, double motion)
        {
            int iterations;
            var currentValue = number;
            var piMotion = Math.PI * 2 * motion;
            var c = new Complex(Math.Cos(piMotion), Math.Sin(piMotion)) * 0.7885;

            for (iterations = 0; iterations < this.Iterations; iterations++)
            {
                // Julia Formula: z -> z^2 + c
                currentValue = currentValue.Pow(2) + c;

                // if the magnitude is bigger than the circumference with a radius of 2,
                // the number is escaping the circumference and we can end the algorithm.
                // we could optimize this calculus by comparing the squares:
                //
                // SquareRoot(real^2 + imaginary^2) >= 2
                // real^2 + imaginary^2 >= 2^2
                // real * real + imaginary * imaginary >= 4
                //
                // doing this we avoid doing an extra square root on each iteration, for each
                // complex number in the final map being calculated.
                if (currentValue.Magnitude >= 2)
                {
                    break;
                }
            }

            return new FractalAlgorithmResult(number, currentValue, iterations, this.Iterations);
        }

        #endregion
    }

    /// <summary>
    /// Calculates the Newton algorithm for a given complex number.
    /// </summary>
    /// <seealso cref="IFractalAlgorithm" />
    public class NewtonFractalAlgorithm : IFractalAlgorithm
    {
        #region Properties

        /// <summary>
        /// Gets the iterations.
        /// </summary>
        /// <value>
        /// The iterations.
        /// </value>
        private int Iterations { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonFractalAlgorithm"/> class.
        /// </summary>
        /// <param name="iterations">The iterations.</param>
        public NewtonFractalAlgorithm(int iterations)
        {
            Iterations = iterations;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates the fractal formula for a specified number in the complex coordinate system.
        /// </summary>
        /// <param name="number">The fractal number.</param>
        /// <param name="motion">The motion parameter.</param>
        /// <returns>
        /// The result of applying the fractal algorithm to a given complex number.
        /// </returns>
        public FractalAlgorithmResult Calculate(Complex number, double motion)
        {
            int iterations;
            var currentValue = number;
            var piMotion = Math.PI * 2 * motion;
            var expMotion = (((Math.Sin(piMotion) + 1) / 2) * 3) + 2.5;

            for (iterations = 0; iterations < this.Iterations; iterations++)
            {
                // calculates a new value.
                var newValue = currentValue - this.Function(currentValue, expMotion) / this.Derivative(currentValue, expMotion);

                var distance = (newValue - currentValue).Magnitude;

                // check if the distance between the newest value and the previous
                // one is close to 0. If the value is not moving any more, we break.
                if (distance <= 0.00001)
                    break;

                currentValue = newValue;
            }

            return new FractalAlgorithmResult(number, currentValue, iterations, this.Iterations);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates the function f(x) = x^exp - 1
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="exp">The exp.</param>
        /// <returns>A new complex number.</returns>
        private Complex Function(Complex number, double exp)
        {
            return number.Pow(exp) - new Complex(1, 0);
        }

        /// <summary>
        /// Calculates the derivative of <see cref="Function"/> f'(x) = number^(exp - 1) * exp
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="exp">The exp.</param>
        /// <returns>A new complex number.</returns>
        private Complex Derivative(Complex number, double exp)
        {
            return number.Pow(exp - 1) * exp;
        }

        #endregion
    }

    /// <summary>
    /// Calculates a variant of the Julia set algorithm for a given complex number.
    /// </summary>
    /// <seealso cref="IFractalAlgorithm" />
    public class JuliaExp4FractalAlgorithm : IFractalAlgorithm
    {
        #region Properties

        /// <summary>
        /// Gets the iterations.
        /// </summary>
        /// <value>
        /// The iterations.
        /// </value>
        private int Iterations { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JuliaExp4FractalAlgorithm"/> class.
        /// </summary>
        /// <param name="iterations">The iterations.</param>
        public JuliaExp4FractalAlgorithm(int iterations)
        {
            Iterations = iterations;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates the fractal formula for a specified number in the complex coordinate system.
        /// </summary>
        /// <param name="number">The fractal number.</param>
        /// <param name="motion">The motion parameter.</param>
        /// <returns>
        /// The result of applying the fractal algorithm to a given complex number.
        /// </returns>
        public FractalAlgorithmResult Calculate(Complex number, double motion)
        {
            int iterations;
            var currentValue = number;
            var piMotion = Math.PI * 2 * motion;
            var expMotion = ((Math.Cos(piMotion) + 1) / 2) * 5 + 2;
            var c = new Complex(Math.Cos(piMotion), Math.Sin(piMotion)) * 0.7885;

            for (iterations = 0; iterations < this.Iterations; iterations++)
            {
                currentValue = currentValue.Abs().Pow(expMotion) + c;

                // if the magnitude is bigger than the circumference with a radius of 2,
                // the number is escaping the circumference and we can end the algorithm.
                // we could optimize this calculus by comparing the squares:
                //
                // SquareRoot(real^2 + imaginary^2) >= 2
                // real^2 + imaginary^2 >= 2^2
                // real * real + imaginary * imaginary >= 4
                //
                // doing this we avoid doing an extra square root on each iteration, for each
                // complex number in the final map being calculated.
                if (currentValue.Magnitude >= 2)
                {
                    break;
                }
            }

            return new FractalAlgorithmResult(number, currentValue, iterations, this.Iterations);
        }

        #endregion
    }

    /// <summary>
    /// Calculates a variant of the Julia set algorithm for a given complex number.
    /// </summary>
    /// <seealso cref="IFractalAlgorithm" />
    public class JuliaExp5FractalAlgorithm : IFractalAlgorithm
    {
        #region Properties

        /// <summary>
        /// Gets the iterations.
        /// </summary>
        /// <value>
        /// The iterations.
        /// </value>
        private int Iterations { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="JuliaExp5FractalAlgorithm"/> class.
        /// </summary>
        /// <param name="iterations">The iterations.</param>
        public JuliaExp5FractalAlgorithm(int iterations)
        {
            Iterations = iterations;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates the fractal formula for a specified number in the complex coordinate system.
        /// </summary>
        /// <param name="number">The fractal number.</param>
        /// <param name="motion">The motion parameter.</param>
        /// <returns>
        /// The result of applying the fractal algorithm to a given complex number.
        /// </returns>
        public FractalAlgorithmResult Calculate(Complex number, double motion)
        {
            int iterations;
            var currentValue = number;
            var piMotion = Math.PI * 2 * motion;
            var c = new Complex(Math.Cos(piMotion), Math.Sin(piMotion)) * 0.7885;

            for (iterations = 0; iterations < this.Iterations; iterations++)
            {
                currentValue = currentValue.Pow(5) + c;

                // if the magnitude is bigger than the circumference with a radius of 2,
                // the number is escaping the circumference and we can end the algorithm.
                // we could optimize this calculus by comparing the squares:
                //
                // SquareRoot(real^2 + imaginary^2) >= 2
                // real^2 + imaginary^2 >= 2^2
                // real * real + imaginary * imaginary >= 4
                //
                // doing this we avoid doing an extra square root on each iteration, for each
                // complex number in the final map being calculated.
                if (currentValue.Magnitude >= 2)
                {
                    break;
                }
            }

            return new FractalAlgorithmResult(number, currentValue, iterations, this.Iterations);
        }

        #endregion
    }

    /// <summary>
    /// Colors the fractal in the same style and design as the Visual Studio Fractals Application.
    /// </summary>
    /// <seealso cref="IColoringTechnique" />
    public class fractalsColoringTechnique : IColoringTechnique
    {
        /// <summary>
        /// Colorizes the specified algorithm result.
        /// </summary>
        /// <param name="algorithmResult">The algorithm result.</param>
        /// <param name="color">The color parameter.</param>
        /// <returns>
        /// A color calculated from the algorithm result.
        /// </returns>
        public Color Colorize(FractalAlgorithmResult algorithmResult, double color)
        {
            // calculates the iteration percentage or how many iterations were completed. the value ranges [0.0..1.0].
            var iteration = MathExtensions.Clamp(algorithmResult.Iterations / (double)algorithmResult.MaxIterations, 0.0, 1.0);

            // calculates the sin of the angle. the domain of the sin function oscillates between [-1.0..1.0],
            // so we need to transform the result to be in the range of [0.0..1.0].
            var phase = MathExtensions.Clamp((Math.Sin(algorithmResult.FinalNumber.Phase) + 1.0) / 2.0, 0.0, 1.0);

            // calculates the magnitude of modulus of the complex number, and the applies some
            // transformations leaving the number in the range [0.0..0.5]
            var magnitude = algorithmResult.FinalNumber.Real * algorithmResult.FinalNumber.Real +
                            algorithmResult.FinalNumber.Imaginary * algorithmResult.FinalNumber.Imaginary;

            magnitude = MathExtensions.Clamp(magnitude >= 4.0 ? magnitude / 8.0 : magnitude / 2, 0.0, 1.0);

            // calculates different colors and then integrates all into one
            // final result, to colorize the complex number.
            var mixColor = Color.FromHsv(phase, magnitude * phase, 1.0 - magnitude);
            var hueColor = Color.FromHsl(color, 0.7, 0.7);
            var phaseColor = new Color(phase, phase, phase);
            var magnitudeColor = new Color(magnitude, magnitude, magnitude);
            var iterationColor = new Color(iteration, iteration, iteration);

            var r = (hueColor.R * ((phaseColor.R + magnitudeColor.R + iterationColor.R) / 3.0) * 3.0 + mixColor.R) / 4.0;
            var g = (hueColor.G * ((phaseColor.G + magnitudeColor.G + iterationColor.G) / 3.0) * 3.0 + mixColor.G) / 4.0;
            var b = (hueColor.B * ((phaseColor.B + magnitudeColor.B + iterationColor.B) / 3.0) * 3.0 + mixColor.B) / 4.0;

            return new Color(r, g, b).Brightness(0.2).Contrast(2.5);
        }
    }

    /// <summary>
    /// Paints a fractal using a <see cref="IFractalAlgorithm"/> and a combination of <see cref="IColoringTechnique"/>.
    /// </summary>
    /// <remarks>
    /// The painter will try to utilize every cpu available to balance the work load.
    /// Fractal algorithms are really calculation intensive, and doing parallel calculations
    /// really improve the performance.
    /// </remarks>
    public class FractalPainter
    {
        #region Properties

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the complex number that represents the top-left corner of the fractal.
        /// </summary>
        /// <value>
        /// The Top Left Corner.
        /// </value>
        public Complex TopLeftCorner { get; private set; }

        /// <summary>
        /// Gets the complex number that represents the bottom-right corner of the fractal.
        /// </summary>
        /// <value>
        /// The bottom right corner.
        /// </value>
        public Complex BottomRightCorner { get; private set; }

        /// <summary>
        /// Gets the increment.
        /// </summary>
        /// <value>
        /// The increment.
        /// </value>
        public Complex Increment { get; private set; }

        /// <summary>
        /// Gets the zoom level.
        /// </summary>
        /// <value>
        /// The zoom level.
        /// </value>
        public double ZoomLevel { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FractalPainter" /> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public FractalPainter(int width, int height)
        {
            this.Resize(width, height);
            this.Reset();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.TopLeftCorner = new Complex(-2.0f, -2.0f);
            this.BottomRightCorner = new Complex(2.0f, 2.0f);
        }

        /// <summary>
        /// Resizes the fractal painter.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void Resize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Zooms the specified pixel from.
        /// </summary>
        /// <param name="pixelFrom">The pixel from.</param>
        /// <param name="pixelTo">The pixel to.</param>
        public void Zoom(Point pixelFrom, Point pixelTo)
        {
            var newFrom = this.ToFractalSpace(pixelFrom);
            var newTo = this.ToFractalSpace(pixelTo);

            var incrementX = (newTo.Real - newFrom.Real) / this.Width;
            var incrementY = (newTo.Imaginary - newFrom.Imaginary) / this.Height;

            const int oldDx = 4;
            var newDx = newTo.Real - newFrom.Real;
            var zoomLevel = oldDx / newDx;

            this.ZoomLevel = zoomLevel;
            this.TopLeftCorner = newFrom;
            this.BottomRightCorner = newTo;
            this.Increment = new Complex(incrementX, incrementY);
        }

        /// <summary>
        /// Draws the specified fractal algorithm.
        /// </summary>
        /// <param name="color">The color parameter.</param>
        /// <param name="motion">The motion parameter.</param>
        /// <param name="algorithm">The fractal algorithm.</param>
        /// <param name="techniques">The coloring techniques.</param>
        /// <param name="combinationFunction">The combination function.</param>
        /// <param name="maxParallelism">The maximum parallelism. By default will take all the CPUs available.</param>
        /// <returns>The final bitmap.</returns>
        public byte[] Draw(
            double color,
            double motion,
            IFractalAlgorithm algorithm,
            IEnumerable<IColoringTechnique> techniques,
            Func<Color[], Color> combinationFunction,
            int maxParallelism = -1)
        {
            // if the max parallelism is not provided, the system will try to create
            // one thread per logical processor available.
            if (maxParallelism <= 0)
                maxParallelism = this.GetMaxParallelism();

            // creates an memory array for all the pixels in the final drawing.
            var bitmapArray = new byte[this.Width * this.Height * 4];

            // separates the image array into smaller slices, once per thread. The idea is to
            // calculate sectors of the fractal in parallel.
            var memorySlices = bitmapArray.SliceArray(maxParallelism);

            // converts the enumeration to a list just to prevent multiple enumerations later.
            var colorTechniques = techniques.ToList();

            //  runs the algorithm and then the coloring techniques for each slice, in parallel.
            Parallel.ForEach(memorySlices, new ParallelOptions { MaxDegreeOfParallelism = maxParallelism }, memorySlice =>
            {
                var span = memorySlice.Span;
                var start = memorySlice.Start;

                for (var i = 0; i < span.Length; i += 4)
                {
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // first we need to calculate the pixel position based on the linear array index:
                    //
                    //    x = index % width
                    //    y = index % height
                    //
                    // but take in account that the index is slice-based, so 0 is not really 0, but 0 + slice start.
                    // so the formula now looks like:
                    //
                    //    x = (index + start) % width
                    //    y = (index + start) / width
                    //
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////
                    var realIndex = i + start;
                    var x = (int)(realIndex / 4.0f) % this.Width;
                    var y = (int)(realIndex / 4.0f) / this.Width;

                    // now we have the correct pixel coordinates, but we are working with a different coordinate system
                    // in the complex space, dictated by the TopLeft and BottomRight corners, so we need to convert the
                    // pixel coordinates to the fractal space.
                    var complexNumber = this.ToFractalSpace(new Point(x, y));

                    // calculates the fractal algorithm for the complex number.
                    var result = algorithm.Calculate(complexNumber, motion);

                    // calculates all the colors based on the techniques.
                    var colors = colorTechniques.Select(t => t.Colorize(result, color)).ToArray();

                    // calculates a final color combining all the different colors.
                    var finalColor = combinationFunction(colors).ToSystemColor();

                    // sets the color coordinates in the slice.
                    span[i] = finalColor.B;
                    span[i + 1] = finalColor.G;
                    span[i + 2] = finalColor.R;
                    span[i + 3] = finalColor.A;
                }
            });

            return bitmapArray;
        }

        /// <summary>
        /// Transforms a given complex number to a bitmap space.
        /// </summary>
        /// <param name="complex">The complex number.</param>
        /// <returns>The pixel coordinates representation of the complex number.</returns>
        public Point ToBitmapSpace(Complex complex)
        {
            var dx = this.BottomRightCorner.Real - this.TopLeftCorner.Real;
            var dy = this.BottomRightCorner.Imaginary - this.TopLeftCorner.Imaginary;

            var px = (complex.Real - this.TopLeftCorner.Real) / dx;
            var py = (complex.Imaginary - this.TopLeftCorner.Imaginary) / dy;

            return new Point(px * this.Width, py * this.Height);
        }

        /// <summary>
        /// Transforms a given pixel to a complex space.
        /// </summary>
        /// <param name="pixel">The given pixel.</param>
        /// <returns>The complex coordinates representation of the given pixel.</returns>
        public Complex ToFractalSpace(Point pixel)
        {
            var px = pixel.X / this.Width;
            var py = pixel.Y / this.Height;

            var dx = this.BottomRightCorner.Real - this.TopLeftCorner.Real;
            var dy = this.BottomRightCorner.Imaginary - this.TopLeftCorner.Imaginary;

            return new Complex(px * dx + this.TopLeftCorner.Real, py * dy + this.TopLeftCorner.Imaginary);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the maximum parallelism.
        /// </summary>
        /// <returns>Number of processors.</returns>
        private int GetMaxParallelism()
        {
            return Environment.ProcessorCount;
        }

        #endregion
    }
}