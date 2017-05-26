using SilviaCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SilviaGUI
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();

            SilviaCore.SilviaApp.OnApplicationInit += SilviaApp_OnApplicationInit;
            this.FormClosing += Options_FormClosing;
            listBoxPlugIns.SelectedIndexChanged += ListBoxPlugIns_SelectedIndexChanged;
        }

        private void ListBoxPlugIns_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkBoxDelayedLoad.Enabled = true;
            btnUnload.Enabled = true;
        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void SilviaApp_OnApplicationInit()
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            foreach (KeyValuePair<string, Plugin> kv in SilviaCore.PluginLoader.Plugins)
            {
                list.Add(new Tuple<string, string>(kv.Value.PluginName + " (" + kv.Key + ")", kv.Key));
            }

            Console.WriteLine("Adding list");

            listBoxPlugIns.DataSource = list;
            listBoxPlugIns.DisplayMember = "Item1";
            listBoxPlugIns.ValueMember = "Item2";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
