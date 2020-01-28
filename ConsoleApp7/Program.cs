using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Numerics;
using System.IO;

namespace TupperFormula
{
    class Program
    {
        static int Width { get; set;}
        static int Height { get; set;}
        static string Bytes { get; set;}
        static BigInteger k = 0;

        static void ConvertToImage(string way)
        {
            Bitmap b = new Bitmap(106, 17);
            Width = b.Width;
            Height = b.Height;
            Bytes = null;
            int WH = Width * Height;
            k /= 17;
                
            while (k > 0)
            {
                Bytes = k % 2 + Bytes;
                k >>= 1;
            }
            if (Bytes.Length < WH)
                Bytes = new string('0', WH- Bytes.Length) + Bytes;

            string[] li = new string[17];

            for (int i = 0; i < WH; i++)
            {
                li[i % Height] = Bytes[WH-1 - i] + li[i % Height];
            }

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    int pix = Convert.ToInt32(li[y][x].ToString()) * 255;
                    b.SetPixel(Width-1 - x, Height-1 - y, Color.FromArgb(pix, pix, pix));
                }

            b.Save(@way);

        }

        public static void ConvertToK(Bitmap srch)
        {
            Width = srch.Width;
            Height = srch.Height;
            Bytes = null;

            for (int x = Width - 1; x >= 0; x--)
                for (int y = 0; y < Height; y++)
                {
                    var any = srch.GetPixel(x, y);
                    if (any.ToArgb()< Color.Gray.ToArgb())//if color of pixel is darker then gray
                        Bytes += "0";
                    else Bytes += "1";
                }

            for (int i = Bytes.Length - 1; i >= 0; i--)
                k += Convert.ToInt32(Bytes[i].ToString()) * BigInteger.Pow(2, Bytes.Length - i - 1);
            k *= 17;
        }

        static void Main()
        {
            
            Console.WriteLine("Изображение в k? [y] [n]");
            if (Console.ReadLine() == "y")
            {
                Console.WriteLine("Укажите путь к картинке 106x17");
                string way = Console.ReadLine();// you can write smth like C:\1.png or D:\image.bmp
                Bitmap image = new Bitmap(@way, true);
                ConvertToK(image);
                Console.WriteLine(k.ToString());
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Введите число k");

                Stream inputStream = Console.OpenStandardInput(1024);
                byte[] bytes = new byte[1024];
                int outputLength = inputStream.Read(bytes, 0, 1024);
                char[] chars = Encoding.UTF8.GetChars(bytes, 0, outputLength);
                string chislo = new string(chars);

                k = BigInteger.Parse(chislo);
                Console.WriteLine("Создайте файл изображения и укажите к нему путь для сохранения");
                string way = Console.ReadLine();
                ConvertToImage(way);
                Console.ReadLine();
            }

        }
    }
}
