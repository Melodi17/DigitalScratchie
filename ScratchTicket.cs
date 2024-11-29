using System.Drawing.Imaging;
using System.Text;

namespace DigitalScratchie;

public partial class ScratchTicket
{
    public string Id;
    public string Set;
    public string Data;
    public Image ScratchLayer;
    public Image BaseLayer;
    public int Width;
    public int Height;
        
    public bool[,] ScratchMask;

    public ScratchTicket(string id, string set, string data, Image scratchLayer, Image baseLayer, bool[,] scratchMask)
    {
        this.Id = id;
        this.Set = set;
        this.Data = data;

        if (baseLayer.Width != scratchLayer.Width || baseLayer.Height != scratchLayer.Height)
            throw new("baselayer and scratchlayer must have the same dimensions");

        this.ScratchLayer = scratchLayer;
        this.BaseLayer = baseLayer;
            
        this.Width = this.BaseLayer.Width;
        this.Height = this.BaseLayer.Height;

        this.ScratchMask = scratchMask;
    }

    public ScratchTicket(string id, string set, string data, Image scratchLayer, Image baseLayer) : this(id, set, data, scratchLayer,
        baseLayer, new bool[baseLayer.Width, baseLayer.Height]) { }
}
