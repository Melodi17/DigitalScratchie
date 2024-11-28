namespace DigitalScratchie
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            ScratchTicket st = new(
                id: "919191",
                set: "I <3 Gambling",
                baseLayer: Image.FromFile("scratchie_base.png"),
                scratchLayer: Image.FromFile("scratchie_top.png"));

            Random r = new();
            for (int x = 0; x < st.ScratchMask.GetLength(0); x++)
            for (int y = 0; y < st.ScratchMask.GetLength(1); y++)
                st.ScratchMask[x, y] = false;

            Application.Run(new ScratchForm(st));
        }
    }

    public class ScratchTicket
    {
        public string Id;
        public string Set;
        public Image ScratchLayer;
        public Image BaseLayer;
        public bool[,] ScratchMask;

        public ScratchTicket(string id, string set, Image scratchLayer, Image baseLayer)
        {
            Id = id;
            Set = set;

            if (baseLayer.Width != scratchLayer.Width || baseLayer.Height != scratchLayer.Height)
                throw new("baselayer and scratchlayer must have the same dimensions");

            ScratchLayer = scratchLayer;
            BaseLayer = baseLayer;

            ScratchMask = new bool[BaseLayer.Width, BaseLayer.Height];
        }
    }
}