using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
//using System.Reflection.Metadata;
//using Pfim;
//using Pfim.dds;
using System.Runtime.InteropServices;

namespace P3DSpriteDivider
{
	public partial class Images
	{


        public static Bitmap PadWidthandHeight(Bitmap image)
        {
            int newWidth = image.Width;
            int newHeight = image.Height;
            while ((newWidth & (newWidth - 1)) != 0)
            {
                newWidth++;
            }
            while ((newHeight & (newHeight - 1)) != 0)
            {
                newHeight++;
            }
            Bitmap newBmp = new Bitmap(newWidth, newHeight);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color px = new Color();
                    int a, r, g, b;
                    if (x > image.Width || y > image.Height)
                    {
                        a = 255;
                        r = 0;
                        g = 0;
                        b = 0;
                    }
                    else
                    {
                        px = image.GetPixel(x, y);
                        a = px.A;
                        r = px.R;
                        g = px.G;
                        b = px.B;
                    }


                    newBmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            return newBmp;
        }

        public static List<Bitmap> DivideBitmap (Bitmap image, int minPO2, int[] size)
        {
            List<int> widths = new List<int>();
            List<int> heights = new List<int>();

            List<Bitmap> maps = new List<Bitmap>();

            int tmp0 = image.Width;
            while (tmp0 > minPO2)
            {
                tmp0 /= 2;
                widths.Add(tmp0);
            }

            tmp0 = image.Height;
            while (tmp0 > minPO2)
            {
                tmp0 /= 2;
                heights.Add(tmp0);
            }

            int yOff = 0;
            foreach(int h in heights)
            {
                int xOff = 0;
                foreach(int w in widths)
                {
                    Bitmap img = new Bitmap(w, h);

                    for (int x=0; x<w; x++)
                    {
                        for (int y = 0; y < h; y++)
                        {
                            Color px = image.GetPixel(x + xOff, y + yOff);
                            img.SetPixel(x, y, px);
                        }
                    }
                    
                    maps.Add(img);
                    xOff += w;
                    if (xOff > size[0]) break;
                }
                yOff += h;
                if (yOff > size[1]) break;
            }

            return maps;
        }
    }
}
