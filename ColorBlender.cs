namespace DigitalScratchie;

public class ColorBlender
{
    public static Color BlendColors(Color color1, Color color2)
    {
        // Extract the RGBA values of the two colors
        int r1 = color1.R, g1 = color1.G, b1 = color1.B, a1 = color1.A;
        int r2 = color2.R, g2 = color2.G, b2 = color2.B, a2 = color2.A;

        // Perform the alpha blending
        int r = (a1 * r1 + (255 - a1) * r2) / 255;
        int g = (a1 * g1 + (255 - a1) * g2) / 255;
        int b = (a1 * b1 + (255 - a1) * b2) / 255;
        int a = a1 + (a2 * (255 - a1)) / 255;

        // Return the blended color
        return Color.FromArgb(a, r, g, b);
    }
}