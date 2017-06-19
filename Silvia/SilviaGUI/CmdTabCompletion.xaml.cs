using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using SilviaCore.Commands;
using System.ComponentModel;

namespace SilviaGUI
{
    /// <summary>
    /// Interaction logic for CmdTabCompletion.xaml
    /// </summary>
    public partial class CmdTabCompletion : Window
    {
        public BindingList<Command> Entries { get; set; }
        public int MinimumLength { get; } = 3;

        public CmdTabCompletion()
        {
            InitializeComponent();

            Entries = new BindingList<Command>();

            TabCompletionList.ItemsSource = this.Entries;

            SilviaCore.Effects.WindowBlur.Apply(this);
        }

        public void Deselect()
        {
            TabCompletionList.SelectedIndex = -1;
        }

        public Command SelectNextItem()
        {
            if (Entries.Count == 0)
                return null;

            if (TabCompletionList.SelectedIndex < Entries.Count - 1)
                TabCompletionList.SelectedIndex++;
            else
                TabCompletionList.SelectedIndex = 0;

            return Entries[TabCompletionList.SelectedIndex];
        }

        public Command SelectPreviousItem()
        {
            if (Entries.Count == 0)
                return null;

            if (TabCompletionList.SelectedIndex > 0)
                TabCompletionList.SelectedIndex--;
            else
                TabCompletionList.SelectedIndex = Entries.Count - 1;

            return Entries[TabCompletionList.SelectedIndex];
        }

        private List<Command> cmds = new List<Command>();
        private string previousString = "";

        public void UpdateEntires(string input)
        {
            if (input.Length < MinimumLength)
            {
                previousString = "";
                return;
            }

            Command[] workingList;

            if (previousString != "" && input.Length >= previousString.Length && input.Substring(0, previousString.Length) == previousString)
            {
                workingList = Entries.ToArray();
            }
            else
            {
                workingList = CmdHandler.Cmds.ToArray();
            }

            previousString = input;
            Entries.Clear();

            for (int i = 0; i < workingList.Length; i++)
            {
                if (!workingList[i].IsHidden && workingList[i].PredictMatch(input))
                {
                    Entries.Add(workingList[i]);
                }
            }
        }
    }
}
