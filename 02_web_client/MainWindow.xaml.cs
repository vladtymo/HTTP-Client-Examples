 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
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

namespace _02_web_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebClient client = new();
        public MainWindow()
        {
            InitializeComponent();

            client.DownloadProgressChanged += Client_DownloadProgressChanged;
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private async void DownloadBtnClick(object sender, RoutedEventArgs e)
        {
            string url = urlTxtBox.Text;
            string dest = GenerateFilePath(url);

            await client.DownloadFileTaskAsync(url, dest);

            AddHistoryItem(url);
        }

        private void AddHistoryItem(string fileName)
        {
            historyList.Items.Add($"{DateTime.Now.ToShortTimeString()}: {Path.GetFileName(fileName)}");
        }
        private string GenerateFilePath(string originalPath)
        {
            // get desktop path
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // generate random name
            string name = Guid.NewGuid().ToString();
            // get original extension
            string extension = Path.GetExtension(originalPath);
            // combine destination path
            return Path.Combine(desktop, name + extension);
        }
    }
}
