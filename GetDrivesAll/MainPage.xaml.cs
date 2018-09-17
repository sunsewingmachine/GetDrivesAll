using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GetDrivesAll
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ShowDrives();
            //ShowDrives2();
        }

        private async  void ShowDrives()
        {

            TxtDriveNames.Text = "";
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            
            foreach (DriveInfo d in allDrives)
            {
                //    Debug.WriteLine("Drive: " + d.Name);
                //   Debug.WriteLine("Drive: " + d.AvailableFreeSpace);
                //  Debug.WriteLine("Drive: " + d.TotalSize);


                AddtoText("Drive: ", d.Name);
                AddtoText("Drive: ", await ShowDrives2( d.Name));

                //AddtoText("Type: ", d.DriveType);
                // AddtoText("Space: ", d.AvailableFreeSpace.ToString());
                //GetTotalSpace();
                //AddtoText("  Root : {0}", d.RootDirectory);
                //DriveInfo's RootDirectory.GetDirectories()
                //DriveInfo's AvailableFreeSpace()

                var mm = await StorageFolder.GetFolderFromPathAsync(d.Name);

                //var kk =  mm.GetResults() ;                
                //mm = await StorageFolder.GetFolderFromPathAsync(@"C:\Recycle.Bin");
                
                //StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(@"D:\$Recycle.Bin");
                //var ss = folder.GetBasicPropertiesAsync().GetResults();
                //return FormatBytes(ss.Size);


                var ab = await mm.GetFoldersAsync();
                string sFolders = "";
                foreach (var item in ab)
                {
                    sFolders = sFolders + ", " + item.Name;
                }

                AddtoText("Folders: " , sFolders);
                //AddtoText( "Folder : " , aa. .FirstOrDefault().FullName);


                AddtoText("", "");
                try
                {                 
                    var aa = d.RootDirectory.GetDirectories();
                    AddtoText( "Folder : " , aa.FirstOrDefault().FullName);
                }
                catch (Exception ex) { }




                if (d.IsReady == true)
                {
                    try
                    {
                        AddtoText("  Volume label: {0}", d.VolumeLabel);
                    }
                    catch (Exception ex)  { }


                    try
                    {
                        AddtoText("  File system: {0}", d.DriveFormat);
                    }
                    catch (Exception ex)  { }


                    try
                    {
                        AddtoText(
                            "  Available space to current user:{0, 15} bytes",
                            d.AvailableFreeSpace);
                    }
                    catch (Exception ex) { }


                    try
                    {
                        AddtoText(
                            "  Total available space:          {0, 15} bytes",
                            d.TotalFreeSpace);
                    }
                    catch (Exception ex) { }

                    try
                    {
                        AddtoText(
                            "  Total size of drive:            {0, 15} bytes ",
                            d.TotalSize);
                    }
                    catch (Exception ex) { }
                }
            }
        }
        
        private void AddtoText(string txt1, object txt2)
        {
            TxtDriveNames.Text = TxtDriveNames.Text + Environment.NewLine + txt1 + txt2.ToString();

        }

        private async Task<string> ShowDrives2(string DriveName)
        {
            const String k_freeSpace = "System.FreeSpace";
            const String k_totalSpace = "System.Capacity";
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                    try
                    {
                        if (d.Name == DriveName)
                        {
                            Debug.WriteLine("Drive: " + d.Name);
                            Debug.WriteLine("RootDir: " + d.RootDirectory.FullName);

                            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(d.RootDirectory.FullName);
                            var props = await folder.Properties.RetrievePropertiesAsync(new string[] { k_freeSpace, k_totalSpace });
                            string ff = "FreeSpace: " + FormatBytes( (UInt64)props[k_freeSpace]);
                            string tt = "Capacity:  " + FormatBytes( (UInt64)props[k_totalSpace]);

                            Debug.WriteLine(ff);
                            Debug.WriteLine(tt);
                            return ff + Environment.NewLine + tt;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(String.Format("Couldn't get info for drive {0}.  Does it have media in it?", d.Name));
                        return "";
                    }
                
            }
            return "";
            //AddtoText("Drive: ", d.Name);

        }


        private static string FormatBytes(UInt64 bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

    }
}



//                    try
//                    {
//                        AddtoText("  Volume label: {0}", d.VolumeLabel);
//AddtoText("  File system: {0}", d.DriveFormat);
//AddtoText(
//                            "  Available space to current user:{0, 15} bytes",
//    d.AvailableFreeSpace);

//AddtoText(
//                            "  Total available space:          {0, 15} bytes",
//    d.TotalFreeSpace);

//AddtoText(
//                            "  Total size of drive:            {0, 15} bytes ",
//    d.TotalSize);
//                    }
//                    catch (Exception ex)  { }