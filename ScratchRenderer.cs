namespace DigitalScratchie;

public class ScratchRenderer
{
    public Image GetRenderedImage(ScratchTicket ticket)
    {
        Bitmap baseLayer = new(ticket.BaseLayer);
        Bitmap scratchLayer = new(ticket.ScratchLayer);
        bool[,] scratchMask = ticket.ScratchMask;

        Bitmap bitmap = new(baseLayer.Width, baseLayer.Height);
        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                bool isMasked = scratchMask[x, y];

                Color baseLayerColor = baseLayer.GetPixel(x, y);
                Color scratchLayerColor = scratchLayer.GetPixel(x, y);

                bitmap.SetPixel(x, y, isMasked 
                    ? baseLayerColor 
                    : ColorBlender.BlendColors(scratchLayerColor, baseLayerColor));
            }
        }

        return bitmap;
    }
}