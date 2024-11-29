namespace DigitalScratchie
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // ScratchTicket st = new(
            //     id: "919191",
            //     set: "I <3 Gambling",
            //     baseLayer: Image.FromFile("scratchie_base.png"),
            //     scratchLayer: Image.FromFile("scratchie_top.png"));
            // st.Save("source.scratchie");
            
            string file = string.Join(' ', args);

            if (file.Trim().Length == 0)
            {
                MessageBox.Show("Please provide a scratch ticket", "No .scratchie file provided", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            
            ScratchTicket st = ScratchTicket.Load(file);
            Application.Run(new ScratchForm(st, file));
        }
    }
}