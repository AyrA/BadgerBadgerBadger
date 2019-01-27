using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadgerBadgerBadger
{
    /// <summary>
    /// Provides a simple text grid, similar to that of a console window
    /// </summary>
    public class StringGrid
    {
        /// <summary>
        /// Character to fill the grid
        /// </summary>
        private const char BLANK = ' ';

        /// <summary>
        /// Characters that are replaced with blanks
        /// </summary>
        private const string INVALID = "\t\r\n";

        /// <summary>
        /// Gets the width of the grid
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// Gets the height of the grid
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Character grid
        /// </summary>
        private char[/*X*/,/*Y*/] Grid;

        /// <summary>
        /// Gets the full grid as string
        /// </summary>
        public string FullGrid
        {
            get
            {
                var SB = new StringBuilder();
                for (var y = 0; y < Height; y++)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        SB.Append(Grid[x, y]);
                    }
                    SB.Append("\r\n");
                }
                return SB.ToString();
            }
        }

        /// <summary>
        /// Initializes a new grid
        /// </summary>
        /// <param name="W">Width in characters</param>
        /// <param name="H">Height in characters</param>
        public StringGrid(int W, int H)
        {
            Reset(W, H);
        }

        /// <summary>
        /// Resets the grid to empty and keeps the current dimensions
        /// </summary>
        public void Reset()
        {
            Reset(int.MinValue, int.MinValue);
        }

        /// <summary>
        /// Resets the grid to new dimensions
        /// </summary>
        /// <param name="W">New Width</param>
        /// <param name="H">New Height</param>
        public void Reset(int W, int H)
        {
            if (W < 1 && W > int.MinValue)
            {
                throw new ArgumentOutOfRangeException("W", "Minimum value is 1");
            }
            if (H < 1 && H > int.MinValue)
            {
                throw new ArgumentOutOfRangeException("H", "Minimum value is 1");
            }
            if (W > int.MinValue)
            {
                Width = W;
            }
            if (H > int.MinValue)
            {
                Height = H;
            }
            if (Width < 1 || Height < 1)
            {
                throw new InvalidOperationException("Can't perform first grid reset without coordinates");
            }

            Grid = new char[Width, Height];
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    Grid[x, y] = BLANK;
                }
            }
        }

        /// <summary>
        /// Sets a line of text at the given coordinates
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="X">Start X</param>
        /// <param name="Y">Start Y</param>
        /// <remarks>Text overflows properly. The grid wraps around</remarks>
        public void Set(string Text, int X, int Y, bool Transparent, char TransparencyMask = BLANK)
        {
            X = Math.Min(X, Width - 1);
            Y = Math.Min(Y, Height - 1);

            foreach (var C in Text.Select(m => INVALID.Contains(m) ? BLANK : m))
            {
                if (!Transparent || C != TransparencyMask)
                {
                    Grid[X, Y] = C;
                }
                ++X;
                if (X == Width)
                {
                    X = 0;
                    ++Y;
                    if (Y == Height)
                    {
                        Y = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Sets multiple lines of text
        /// </summary>
        /// <param name="Text">Text lines</param>
        /// <param name="X">Start X</param>
        /// <param name="Y">Start Y</param>
        /// <remarks>Text overflows properly as long as lines are not wider than the grid. The grid wraps around</remarks>
        public void Set(IEnumerable<string> Text, int X, int Y, bool Transparent, char TransparencyMask = BLANK)
        {
            X = Math.Max(0, X % Width);
            Y = Math.Max(0, Y % Height);
            foreach (var T in Text)
            {
                Set(T, X, Y, Transparent, TransparencyMask);
                Y = (Y + 1) % Height;
            }
        }

        /// <summary>
        /// Gets the given number of characters from the given start position
        /// </summary>
        /// <param name="X">Start X</param>
        /// <param name="Y">Start Y</param>
        /// <param name="Count">Number of characters to get</param>
        /// <returns>String of text</returns>
        /// <remarks>The string will not contain line breaks if it goes over multiple lines</remarks>
        public string Get(int X, int Y, int Count)
        {
            char[] Data = new char[Count];

            X = Math.Abs(Math.Min(X, X % Width));
            Y = Math.Abs(Math.Min(Y, X % Height));

            for (var i = 0; i < Count; i++)
            {
                Data[i] = Grid[X, Y];
                ++X;
                if (X == Width)
                {
                    X = 0;
                    ++Y;
                    if (Y == Height)
                    {
                        Y = 0;
                    }
                }
            }
            return new string(Data);
        }
    }
}
