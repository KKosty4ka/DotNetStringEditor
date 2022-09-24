using System;
using System.Windows.Forms;

namespace DotNetStringEditor
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string filename;
            using (OpenFileDialog d = new OpenFileDialog() )
            {
                d.Title = "Select a .NET file";
                d.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                d.DefaultExt = "*.exe";
                d.AddExtension = true;
                d.Filter = ".NET executables|*.exe|.NET libraries|*.dll";

                if (d.ShowDialog() != DialogResult.OK) Environment.Exit(0);

                filename = d.FileName;
            }

            Application.Run(new MainForm(filename) );
        }
    }
}
