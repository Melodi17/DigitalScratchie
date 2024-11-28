namespace DigitalScratchie
{
    public partial class ScratchForm : Form
    {
        private ScratchRenderer sr = new();
        private ScratchTicket st;
        private bool mouseDown;
        public ScratchForm(ScratchTicket ticket)
        {
            st = ticket;
            InitializeComponent();

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
            e.Graphics.DrawImage(sr.GetRenderedImage(st), 0, 0);
        }

        private void ScratchForm_MouseDown(object sender, MouseEventArgs e) => mouseDown = true;

        private void ScratchForm_MouseUp(object sender, MouseEventArgs e) => mouseDown = false;

        private void ScratchForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown) return;

            if (e.X < 0 || e.Y < 0 || e.X >= st.ScratchMask.GetLength(0) || e.Y >= st.ScratchMask.GetLength(1))
                return;

            // st.ScratchMask[e.X, e.Y] = true;
            SetScratchMaskRadius(e.X, e.Y, 10);
            this.Refresh();
        }
        
        private void SetScratchMaskRadius(int x, int y, int radius)
        {
            int width = st.ScratchMask.GetLength(0);
            int height = st.ScratchMask.GetLength(1);

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    int newX = x + i;
                    int newY = y + j;

                    if (newX >= 0 && newY >= 0 && newX < width && newY < height && (i * i + j * j <= radius * radius))
                    {
                        st.ScratchMask[newX, newY] = true;
                    }
                }
            }
        }
    }
}
