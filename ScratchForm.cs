using System.Drawing.Imaging;

namespace DigitalScratchie
{
    internal partial class ScratchForm : Form
    {
        private ScratchTicket st;
        private bool mouseDown;
        private Bitmap baseLayer;
        private Bitmap scratchLayer;
        private int penSize;
        private string? path;
        private bool isDirty = false;
        public ScratchForm(ScratchTicket ticket, string? path)
        {
            this.st = ticket;
            this.path = path;
            this.baseLayer = new Bitmap(ticket.BaseLayer);
            this.scratchLayer = new Bitmap(ticket.ScratchLayer);

            this.penSize = (int)(Math.Sqrt(ticket.Width * ticket.Height) / 15);

            this.InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();

            this.ClientSize = new(ticket.BaseLayer.Width, ticket.BaseLayer.Height);
            this.Text = $"{ticket.Set} #{ticket.Id}";
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(this.baseLayer, 0, 0);
            e.Graphics.DrawImage(this.scratchLayer.FastMask(st.ScratchMask, true), 0, 0);
        }

        private void ScratchForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Shift)
            {
                Bitmap bmp = (Bitmap)this.baseLayer.Clone();
                using Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(this.scratchLayer.FastMask(st.ScratchMask, true), 0, 0);
                
                var filename = "ticket.png";
                var path = Path.Combine(Path.GetTempPath(), filename);
                bmp.Save(path, ImageFormat.Png);

                DoDragDrop(new DataObject(DataFormats.FileDrop, new[] {path}), DragDropEffects.Copy);
                return;
            }
            
            this.mouseDown = true;
        }

        private void ScratchForm_MouseUp(object sender, MouseEventArgs e) => this.mouseDown = false;

        private void ScratchForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.mouseDown) return;

            if (e.X < 0 || e.Y < 0 || e.X >= this.st.Width || e.Y >= this.st.Height)
                return;

            this.SetScratchMaskRadius(e.X, e.Y, this.penSize);
            this.isDirty = true;
        }

        private void ScratchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.path != null)
                this.st.Save(this.path);
            
            this.baseLayer.Dispose();
            this.scratchLayer.Dispose();
        }

        private void SetScratchMaskRadius(int x, int y, int radius)
        {
            int width = this.st.ScratchMask.GetLength(0);
            int height = this.st.ScratchMask.GetLength(1);

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    int newX = x + i;
                    int newY = y + j;

                    if (newX >= 0 && newY >= 0 && newX < width && newY < height && (i * i + j * j <= radius * radius))
                    {
                        this.st.ScratchMask[newX, newY] = true;
                    }
                }
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!this.isDirty)
                return;
            
            this.Refresh();
            this.isDirty = false;
        }
    }
}
