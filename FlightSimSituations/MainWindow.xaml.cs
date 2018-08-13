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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimSituations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool hasChangeBeenMadeSinceLastSave;

        public MainWindow()
        {
            InitializeComponent();
            Situations.PopulateTreeView(situationlistTreeView, Situations.allSituations);
        }

        private void saveSituationConfigButton_Click(object sender, RoutedEventArgs e)
        {

            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "SituationsConfig", // Default file name
                DefaultExt = ".scfg", // Default file extension
                Filter = "Situation Config Files (.scfg)|*.scfg" // Filter files by extension
            };

            // Show save file dialog box
            var result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                var filename = dlg.FileName;
                var cfgBuilder = new StringBuilder();

                foreach (TreeViewItem item in situationlistTreeView.Items) // category
                {
                    foreach (CheckBox cb in item.Items)
                    {
                        if (cb.IsChecked != null)
                        {
                            if (cb.IsChecked.Value)
                            {
                                cfgBuilder.Append(cb.Name + "|1");
                            }
                            else
                            {
                                cfgBuilder.Append(cb.Name + "|0");
                            }
                            cfgBuilder.Append(Environment.NewLine);
                        }
                    }
                }

                using (var sw = new System.IO.StreamWriter(filename))
                {
                    sw.Write(cfgBuilder.ToString());
                }
            }
            else
            {
                MessageBox.Show("Save cancelled!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void loadSituationConfigButton_Click(object sender, RoutedEventArgs e)
        {

            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "SituationsConfig", // Default file name
                DefaultExt = ".scfg", // Default file extension
                Filter = "Situation Config Files (.scfg)|*.scfg" // Filter files by extension
            };

            // Show save file dialog box
            var result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                var filename = dlg.FileName;

                var allConfigLines = System.IO.File.ReadAllLines(filename);
                var cbCheckState = new Dictionary<string, bool?>();

                foreach (var line in allConfigLines)
                {
                    var splitLine = line.Split('|');
                    bool? cbState = null;
                    try
                    {
                        if (splitLine[1] == "1")
                        {
                            cbState = true;
                        }
                        else if (splitLine[1] == "0")
                        {
                            cbState = false;
                        }
                        else
                        {
                            MessageBox.Show("Error loading cfg: Line " + Array.IndexOf(allConfigLines, line) + " of " + allConfigLines.Length + ": no state specified for checkbox.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        cbCheckState.Add(splitLine[0], cbState);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        MessageBox.Show("Couldn't get value of checkbox. No string splitting char was found to separate the ID and state. Maybe your config is corrupt?");
                        disableAllSituationsButton_Click(this, new RoutedEventArgs());
                        return;
                    }
                }


                foreach (TreeViewItem item in situationlistTreeView.Items) // category
                {
                    foreach (CheckBox cb in item.Items)
                    {
                        if (cbCheckState.TryGetValue(cb.Name, out bool? state))
                        {
                            cb.IsChecked = state;
                        }
                        else
                        {
                            cb.IsChecked = false;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Load cancelled!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void enableAllSituationsButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (TreeViewItem item in situationlistTreeView.Items) // category
            {
                foreach (CheckBox cb in item.Items)
                {
                    cb.IsChecked = true;
                }
            }
        }

        private void disableAllSituationsButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (TreeViewItem item in situationlistTreeView.Items) // category
            {
                foreach (CheckBox cb in item.Items)
                {
                    cb.IsChecked = false;
                }
            }
        }

        private void ApplicationClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (hasChangeBeenMadeSinceLastSave)
            {
                var r = MessageBox.Show(
                        "Changes have been made to your situation configuration. Do you wish to save these changes?",
                        "Save changes?",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Warning
                    );

                if (r != MessageBoxResult.No)
                    e.Cancel = true;

                if (r == MessageBoxResult.Yes)
                    saveSituationConfigButton_Click(this, new RoutedEventArgs());
            }
        }
    }
}
