using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;

namespace KralAppLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ReadSettingsAndBuildButtons();
        }

        /// <summary>
        /// Načtení settings a vytvoření tlačítek na ploše aplikace
        /// </summary>
        private void ReadSettingsAndBuildButtons()
        {
            List<desktopapp> result = ReadSettings();

            int nRow = 1;
            int nCol = 1;
            Button btn;
            int nAppCounter = 0;
            foreach (desktopapp app in result)
            {
                nAppCounter++;
                btn = CreateButton(nRow, nCol, app.name, app.path, 26);
                Grid.SetColumnSpan(btn, 2);

                grdMain.Children.Add(btn);
                BuildManuals(nRow + 1, nCol, app.manuals);
                nCol += 3;
                if (nAppCounter == 3)
                {
                    nRow += 3;
                    nCol = 1;
                }
                if (nAppCounter > 5)
                    break;
            }
        }

        /// <summary>
        /// Načtení settings
        /// </summary>
        /// <returns></returns>
        private static List<desktopapp> ReadSettings()
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "desktopappgroup";
            xRoot.IsNullable = true;
            XmlSerializer serializer = new XmlSerializer(typeof(List<desktopapp>), xRoot);
            List<desktopapp> result;

            using (var stream = new StreamReader("Settings.xml"))
            using (var reader = XmlReader.Create(stream))
            {
                result = (List<desktopapp>)serializer.Deserialize(reader);
            }

            return result;
        }

        /// <summary>
        /// Vytvoření tlačítek pro manuál(y)
        /// </summary>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <param name="manuals"></param>
        private void BuildManuals(int nRow, int nCol, List<manual> manuals)
        {
            int nManualCounter = 0;
            Button btn = null;
            foreach (manual manual in manuals)
            {
                nManualCounter++;
                if (nManualCounter > 1)
                {
                    grdMain.Children.Add(btn);
                    nCol++;
                }
                if (nManualCounter > 2)
                {
                    break;
                }
                btn = CreateButton(nRow, nCol, manual.name, manual.path, 16);
            }
            if (nManualCounter == 1)
            {
                Grid.SetColumnSpan(btn, 2);
            }

            grdMain.Children.Add(btn);
        }

        /// <summary>
        /// Vytvoření tlačítka
        /// </summary>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <param name="sName"></param>
        /// <param name="sPath"></param>
        /// <param name="nFontSize"></param>
        /// <returns></returns>
        private Button CreateButton(int nRow, int nCol, string sName, string sPath, int nFontSize)
        {
            Button btn = new Button();
            TextBlock txbl = new TextBlock() { Text = sName };
            txbl.FontSize = nFontSize;
            btn.Margin = new Thickness(10);
            btn.Content = txbl;
            btn.Tag = sPath;
            btn.IsEnabled = System.IO.File.Exists(sPath);
            btn.Click += btnOpen_Click;
            Grid.SetRow(btn, nRow);
            Grid.SetColumn(btn, nCol);
            return btn;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(((Button)sender).Tag.ToString());
        }

    }
}
