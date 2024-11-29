using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DigitalScratchie;

internal static class ImageExtensions
{
    public static Bitmap Mask(this Bitmap image, bool[,] mask, bool invert = false)
    {
        //Get the bitmap data
        var bitmapData = image.LockBits(
            new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.ReadWrite,
            image.PixelFormat
        );

        //Initialize an array for all the image data
        byte[] imageBytes = new byte[bitmapData.Stride * image.Height];

        //Copy the bitmap data to the local array
        Marshal.Copy(bitmapData.Scan0, imageBytes, 0, imageBytes.Length);

        //Unlock the bitmap
        image.UnlockBits(bitmapData);

        //Find pixelsize
        int pixelSize = Image.GetPixelFormatSize(image.PixelFormat);

        // An example on how to use the pixels, lets make a copy
        int x = 0;
        int y = 0;
        var bitmap = new Bitmap(image.Width, image.Height);

        //Loop pixels
        for (int i = 0; i < imageBytes.Length; i += pixelSize / 8)
        {
            //Copy the bits into a local array
            var pixelData = new byte[3];
            Array.Copy(imageBytes, i, pixelData, 0, 3);

            //Get the color of a pixel
            var color = Color.FromArgb(pixelData[0], pixelData[1], pixelData[2]);

            //Set the color of a pixel
            bitmap.SetPixel(x, y, mask[x, y] ^ invert ? color : Color.Transparent);

            //Map the 1D array to (x,y)
            x++;
            if (x >= bitmap.Width)
            {
                x = 0;
                y++;
            }
        }

        //Return the new image
        return bitmap;
    }
    
    public static Bitmap FastMask(this Bitmap image, bool[,] mask, bool invert = false)
    {
        // Get the bitmap data
        var bitmapData = image.LockBits(
            new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.ReadWrite,
            image.PixelFormat
        );

        // Initialize an array for all the image data
        byte[] imageBytes = new byte[bitmapData.Stride * image.Height];

        // Copy the bitmap data to the local array
        Marshal.Copy(bitmapData.Scan0, imageBytes, 0, imageBytes.Length);

        // Find pixel size
        int pixelSize = Image.GetPixelFormatSize(image.PixelFormat) / 8;

        // Loop pixels
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                int index = (y * bitmapData.Stride) + (x * pixelSize);

                if (mask[x, y] ^ invert)
                {
                    // Keep the original color
                    continue;
                }
                else
                {
                    // Set the pixel to transparent
                    imageBytes[index] = 0;     // Blue
                    imageBytes[index + 1] = 0; // Green
                    imageBytes[index + 2] = 0; // Red
                    if (pixelSize == 4)
                    {
                        imageBytes[index + 3] = 0; // Alpha
                    }
                }
            }
        }

        // Copy the modified data back to the bitmap
        Marshal.Copy(imageBytes, 0, bitmapData.Scan0, imageBytes.Length);

        // Unlock the bitmap
        image.UnlockBits(bitmapData);

        return image;
    }
}