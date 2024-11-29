using System.Drawing.Imaging;
using System.Text;

namespace DigitalScratchie;

public partial class ScratchTicket
{
    public static ScratchTicket Load(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        return Load(bytes);
    }
    
    public void Save(string path)
    {
        byte[] data = Save();
        File.WriteAllBytes(path, data);
    }
    
    public static ScratchTicket Load(byte[] bytes)
    {
        // Reading table of contents
        byte[][] sections = GetSectionsFromFile(bytes);
        byte[] headerBytes = sections[0];
        byte[] maskBytes = sections[1];
        byte[] baseLayerBytes = sections[2];
        byte[] scratchLayerBytes = sections[3];
        byte[] dataBytes = sections[4];
        
        // Header
        string header = Encoding.UTF8.GetString(headerBytes);
        if (!header.StartsWith("st") || !header.EndsWith(";"))
            throw new("Invalid file format");
        
        string[] headerParts = header[2..].Split(';');
        string id = headerParts[0];
        string set = headerParts[1];
        int width = int.Parse(headerParts[2]);
        int height = int.Parse(headerParts[3]);
        
        // Mask
        bool[,] scratchMask = new bool[width, height];
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
            scratchMask[x, y] = maskBytes[x * height + y] == 1;
        
        // Images
        Image baseLayer = ByteArrayToImage(baseLayerBytes);
        Image scratchLayer = ByteArrayToImage(scratchLayerBytes);
        
        // Data
        string data = Encoding.UTF8.GetString(dataBytes);

        return new(id, set, data, scratchLayer, baseLayer, scratchMask);
    }
    
    public byte[] Save()
    {
        // Image is saved like this:
        // st[ID];[set];[width];[height];
        // [mask as 2d array of bytes]
        // [base layer as byte array]
        // [scratch layer as byte array]

        byte[] headerBytes = Encoding.UTF8.GetBytes(
            $"st{this.Id};{this.Set};{this.Width};{this.Height};");
        
         byte[] maskBytes = new byte[this.Width * this.Height];
        
        for (int x = 0; x < this.Width; x++)
        for (int y = 0; y < this.Height; y++)
            maskBytes[x * this.Height + y] = this.ScratchMask[x, y] ? (byte)1 : (byte)0;
        
        byte[] baseLayerBytes = ImageToByteArray(this.BaseLayer);
        byte[] scratchLayerBytes = ImageToByteArray(this.ScratchLayer);
        
        byte[] dataBytes = Encoding.UTF8.GetBytes(this.Data);
        
        byte[] data = MakeSectionedFile([headerBytes, maskBytes, baseLayerBytes, scratchLayerBytes, dataBytes]);
        return data;
    }

    private static byte[][] GetSectionsFromFile(byte[] data)
    {
        List<byte[]> sections = new();
        int offset = 0;
        while (offset < data.Length)
        {
            int length = BitConverter.ToInt32(data, offset);
            offset += 4;
            byte[] section = new byte[length];
            Array.Copy(data, offset, section, 0, length);
            sections.Add(section);
            offset += length;
        }
        
        return sections.ToArray();
    }

    private static byte[] MakeSectionedFile(byte[][] sections)
    {
        int totalLength = sections.Sum(s => s.Length);
        int headersLength = sections.Length * 4;
        
        byte[] data = new byte[totalLength + headersLength];
        int offset = 0;
        foreach (byte[] section in sections)
        {
            byte[] header = BitConverter.GetBytes(section.Length);
            header.CopyTo(data, offset);
            offset += 4;
            section.CopyTo(data, offset);
            offset += section.Length;
        }
        
        return data;
    }
    
    private static byte[] ImageToByteArray(Image image)
    {
        using MemoryStream ms = new();
        image.Save(ms, ImageFormat.Png);
        return ms.ToArray();
    }
    
    private static Image ByteArrayToImage(byte[] bytes)
    {
        using MemoryStream ms = new(bytes);
        return Image.FromStream(ms);
    }
}