using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FolderBrowserEx;
using Microsoft.Win32;

namespace Tic_Tac_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string userRoot = "HKEY_LOCAL_MACHINE\\SOFTWARE";
        const string subkey = "TicTacLauncher";
        const string keyName = userRoot + "\\" + subkey;
        const string ubkey = "SOFTWARE\\" + subkey;
        public MainWindow()
        {
            InitializeComponent();
            Bar.Visibility = Visibility.Hidden;
            DownloadAPI downloadApi = new DownloadAPI();
            if (!File.Exists(Directory.GetCurrentDirectory()+"\\currentversion.txt"))
            {
                ButtonText.Content = "Install";
            }
            else
            {
                StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() +"\\currentversion.txt");
                string installedversion = sr.ReadLine().Replace("\n", "").Replace("\n", "").Replace(@"
                ","");;
                sr.Close();
                
                string currentversion = downloadApi.GetCurrentVersion().Replace("\n", "").Replace("\n", "").Replace(@"
                ","");
                if (installedversion.Equals(currentversion))
                {
                    ButtonText.Content = "Play";
                }
                else
                {
                    ButtonText.Content = "Update";
                }
            }

            PatchNote.Text = downloadApi.GetCurrentPatchNotes();

        }

        private void UpdateInstallOnClick(object sender, RoutedEventArgs e)
        {
            if (!checkInstalled())
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.Title = "Select a folder";
                folderBrowserDialog.InitialFolder = @"C:\Program Files\TicTacGame";
                folderBrowserDialog.AllowMultiSelect = false;
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() +"\\installationfolder.txt");
                    sw.WriteLine(folderBrowserDialog.SelectedFolder);
                    sw.Close();
                    ButtonXAs.IsEnabled = false;
                    Thread thread = new Thread(()=> UpdateInstall());
                    thread.Start();

                }
            }
            else
            {
                Thread thread = new Thread(()=> UpdateInstall());
                thread.Start();
            }
        }


        public void UpdateInstall()
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() +"\\installationfolder.txt");
            string InstallLocation = sr.ReadLine();
            sr.Close();
            int fCount = Directory.GetFiles(InstallLocation, "*", SearchOption.AllDirectories).Length;
            if (fCount <= 3)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>  
                {  
  
                    Bar.Visibility = Visibility.Visible;
                    Bar.Value = 0;
  
                }));  
                
                WebClient webClient = new WebClient();
                DownloadAPI downloadApi = new DownloadAPI();
                string currentversion = downloadApi.GetCurrentVersion().Replace("\n", "").Replace("\n", "").Replace(@"
                ","");
                StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() +"\\currentversion.txt");
                sw.WriteLine(currentversion);
                sw.Close();
                webClient.DownloadFile("https://lp4.dev/tictacgame/"+currentversion+".zip", "TempTicTac.zip");
                this.Dispatcher.BeginInvoke(new Action(() =>  
                {  
  
                    Bar.Value = 50;
  
                }));
                System.IO.Compression.ZipFile.ExtractToDirectory(Directory.GetCurrentDirectory()+"\\TempTicTac.zip", InstallLocation);
                this.Dispatcher.BeginInvoke(new Action(() =>  
                {  
  
                    Bar.Value = 100;
  
                }));
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = InstallLocation+"\\SpaceSurvival.exe";
                Process.Start(start);
                this.Dispatcher.BeginInvoke(new Action(() =>  
                {  
  
                    Application.Current.Shutdown();
  
                }));
                
            }
            else
            {
                DownloadAPI downloadApi = new DownloadAPI();
                string version = "";
                if (File.Exists(Directory.GetCurrentDirectory() + "\\currentversion.txt"))
                {
                    StreamReader sr2 = new StreamReader(Directory.GetCurrentDirectory() +"\\currentversion.txt");
                    version = sr2.ReadLine();
                    sr2.Close();
                }
                
                string tescsad = downloadApi.GetCurrentVersion().Replace("\n", "").Replace("\n", "").Replace(@"
                ","");
                if (version.Equals(tescsad))
                {
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = InstallLocation+"\\SpaceSurvival.exe";
                    Debug.WriteLine(InstallLocation+"\\SpaceSurvival.exe");
                    Process.Start(start);
                    Application.Current.Shutdown();
                }
                else
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>  
                    {  
  
                        Bar.Visibility = Visibility.Visible;
                        Bar.Value = 0;
  
                    }));  
                    WebClient webClient = new WebClient();
                    System.IO.DirectoryInfo di = new DirectoryInfo(InstallLocation);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete(); 
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true); 
                    }
                    string entversion = downloadApi.GetCurrentVersion().Replace("\n", "").Replace("\n", "").Replace(@"
                ","");
                    webClient.DownloadFile("https://lp4.dev/tictacgame/"+entversion+".zip", "TempTicTac.zip");
                    this.Dispatcher.BeginInvoke(new Action(() =>  
                    {  
  
                        Bar.Value = 50;
  
                    }));
                    try
                    {
                        StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\currentversion.txt");
                        sw.WriteLine(downloadApi.GetCurrentVersion());
                        sw.Close();
                    }
                    catch
                    {
                        Debug.WriteLine("Error Writing version file");
                    }
                    
                    System.IO.Compression.ZipFile.ExtractToDirectory(Directory.GetCurrentDirectory()+"\\TempTicTac.zip", InstallLocation);
                    this.Dispatcher.BeginInvoke(new Action(() =>  
                    {  
  
                        Bar.Value = 100;
  
                    }));
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = InstallLocation+"\\SpaceSurvival.exe";
                    Debug.WriteLine(InstallLocation+"\\SpaceSurvival.exe");
                    Process.Start(start);
                    this.Dispatcher.BeginInvoke(new Action(() =>  
                    {  
  
                        Application.Current.Shutdown();
  
                    }));
                }
            }

        }
        
        
        public static bool checkInstalled()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\installationfolder.txt"))
            {
                return true;
            }

            return false;
        }
    }
}