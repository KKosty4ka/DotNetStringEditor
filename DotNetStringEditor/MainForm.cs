using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Windows.Forms;

namespace DotNetStringEditor
{
    public partial class MainForm : Form
    {
        private readonly ModuleDefMD module;


        public MainForm(string filename)
        {
            InitializeComponent();


            module = ModuleDefMD.Load(filename);

            foreach (TypeDef type in module.GetTypes() )
            {
                foreach (MethodDef method in type.Methods)
                {
                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        if (method.Body.Instructions[i].OpCode != OpCodes.Ldstr) continue;

                        lstStrings.Items.Add(method.Body.Instructions[i].Operand.ToString() );
                    }
                }
            }


            lstStrings.SelectedIndex = 0;
            lstStrings.SelectedItem = lstStrings.Items[0];
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            int count = 0;

            foreach (TypeDef type in module.GetTypes() )
            {
                foreach (MethodDef method in type.Methods)
                {
                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        if (method.Body.Instructions[i].OpCode != OpCodes.Ldstr) continue;

                        method.Body.Instructions[i].Operand = lstStrings.Items[count];
                        count++;
                    }
                }
            }

            using (SaveFileDialog d = new SaveFileDialog() )
            {
                d.Title = "Save the patched file";
                d.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                d.DefaultExt = "*.exe";
                d.AddExtension = true;
                d.Filter = ".NET executables|*.exe|.NET libraries|*.dll";

                if (d.ShowDialog() != DialogResult.OK) Environment.Exit(0);

                module.Write(d.FileName);
            }

            module.Dispose();
        }


        private void lstStrings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstStrings.SelectedIndex != -1)
            {
                txtString.Text = lstStrings.Items[lstStrings.SelectedIndex].ToString();
            }
        }

        private void txtString_TextChanged(object sender, EventArgs e)
        {
            lstStrings.Items[lstStrings.SelectedIndex] = txtString.Text;
        }
    }
}
