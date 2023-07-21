//Group contains Alexander Moran, Anthony Arellano, and Thomas Hadley

using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.Design;

namespace PDC_LAB_5
{
    class Program
    {
        static public int ITERATIONS = 1000;
        static public int RESOLUTION = 1000;
        static void Main(string[] args)
        {
            Draw(Mandelbrot());
        }

        static public int[,] Mandelbrot()
        {
            int[,] grid = new int[RESOLUTION,RESOLUTION];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                Parallel.For(0, grid.GetLength(1), j =>
                {
                    Complex z = new Complex(0, 0);
                    int t = 0;
                    Complex cc = new Complex(2.5 * (j - RESOLUTION / 1.5) / (RESOLUTION), 2.5 * (i - RESOLUTION / 2.0) / (RESOLUTION));
                    for (t = 0; t <= ITERATIONS; t++)
                    {
                        z = KernelFunction(z, cc);
                        if(z.Magnitude >= 2)
                        {

                            break;
                        }
                    }
                    grid[i, j] = t;
                });
            }
            return grid;
        }

        static public void Draw(int[,] grid)
        {
            var bitmap = new System.Drawing.Bitmap(1000, 1000);
            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    int r = 255-((Convert.ToInt32(Math.Round(grid[i*RESOLUTION / 1000, j*RESOLUTION / 1000] * 0xFFFFFF / 1000.0)) & 0xFF0000)>>16);
                    int g = 255-((Convert.ToInt32(Math.Round(grid[i * RESOLUTION / 1000, j * RESOLUTION / 1000] * 0xFFFFFF / 1000.0)) & 0xFF00)>>8);
                    int b = 255-(Convert.ToInt32(Math.Round(grid[i * RESOLUTION / 1000, j * RESOLUTION / 1000] * 0xFFFFFF / 1000.0)) & 0xFF);
                    bitmap.SetPixel(i, j, Color.FromArgb((r), (g), (b)));
                }
            }

            bitmap.Save("test.bmp");
        }

        static public Complex KernelFunction (Complex z, Complex cc)
        {
            return z * z + cc;
        }
    }
}
