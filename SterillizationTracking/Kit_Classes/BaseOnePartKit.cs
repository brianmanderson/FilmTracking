using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using SterillizationTracking.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SterillizationTracking.Kit_Classes
{
    public class BaseOnePartKit : INotifyPropertyChanged
    {
        private List<string> usageDates = new List<string>();
        private string usesLeft_string, description;
        private System.Windows.Media.Brush statusColor;
        private string name;
        private string kitnumber;
        private string useFileLocation;
        private string _present;
        private bool can_reorder, canAdd;
        private int usesLeft, total_uses;

        public int warning_uses;
        public string KitDirectoryPath;
        public string ReorderDirectoryPath;
        public int TotalUses
        {
            get
            {
                return total_uses;
            }
            set
            {
                total_uses = value;
                OnPropertyChanged("TotalUses");
            }
        }
        public List<string> UsageDates
        {
            get
            {
                return usageDates;
            }
            set
            {
                usageDates = value;
                OnPropertyChanged("UsageDates");
            }
        }
        public string UsesLeftString
        {
            get { return usesLeft_string; }
            set
            {
                usesLeft_string = value;
                OnPropertyChanged("UsesLeftString");
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        public string Present
        {
            get { return _present; }
            set
            {
                _present = value;
                OnPropertyChanged("Present");
            }
        }
        public bool CanAdd
        {
            get { return canAdd; }
            set
            {
                canAdd = value;
                OnPropertyChanged("CanAdd");
            }
        }
        public BaseOnePartKit(string name, string kitnumber, string file_path) //string name, int allowed_steralizaitons, int warning_use
        {
            Name = name;
            StatusColor = statusColor;
            KitNumber = $"Kit #: {kitnumber}";
            KitDirectoryPath = Path.Combine(file_path, name, $"Kit {kitnumber}");
            UseFileLocation = Path.Combine(KitDirectoryPath, "Uses.txt");

            string TotalUsesPath = Path.Combine(file_path, name, "Total_Uses.txt");
            List<string> lines = File.ReadAllLines(TotalUsesPath).ToList();
            total_uses = int.Parse(lines[0].Split("Uses:")[1]);
            warning_uses = Convert.ToInt32(total_uses * .75);
            CanReorder = false;
            Description = "";
            CanAdd = true;
            UsageDates = new List<string>();
            build_read_use_file();
        }

        public void build_read_use_file()
        {
            UsesLeft = total_uses;
            if (File.Exists(UseFileLocation))
            {
                List<string> lines = File.ReadAllLines(UseFileLocation).ToList();
                Description = lines[0].Split("Description:")[1];
                UsesLeft = Convert.ToInt32(lines[1].Split("Use:")[1]);
                total_uses = Convert.ToInt32(lines[2].Split("Uses:")[1]);
                warning_uses = Convert.ToInt32(lines[3].Split("Uses:")[1]);
                Present = lines[4].Split("updated:")[1];
                if (lines.Count > 5)
                {
                    UsageDates = lines.GetRange(5, lines.Count - 5);
                }
                else
                {
                    UsageDates = new List<string>();
                }

            }
            else
            {
                string[] info ={ $"Description:{Description}", $"Left Use:{total_uses}", $"Total Uses:{total_uses}", $"Warning Uses:{warning_uses}", $"Last updated:{Present}" };
                if (!Directory.Exists(KitDirectoryPath))
                {
                    Directory.CreateDirectory(KitDirectoryPath);
                }
                File.WriteAllLines(UseFileLocation, info);
            }
            check_status();
            UsesLeftString = $"Uses left: {UsesLeft}";
        }

        public void create_reorder_file()
        {
            ReorderDirectoryPath = Path.Combine(KitDirectoryPath, "Reorders");
            if (!Directory.Exists(ReorderDirectoryPath))
            {
                Directory.CreateDirectory(ReorderDirectoryPath);
            }
            DateTime moment = DateTime.Now;
            Present = (moment.ToLongDateString() + " " + moment.ToLongTimeString()).Replace(":", ".");
            string file_path = Path.Combine(ReorderDirectoryPath, Present.Replace(":", ".") + ".txt");
            File.WriteAllLines(file_path, UsageDates);
        }
        public void update_file()
        {
            List<string> info = new List<string>() { $"Description:{Description}", $"Left Use:{UsesLeft}",
                $"Total Uses:{total_uses}", $"Warning Uses:{warning_uses}", $"Last updated:{Present}" };
            info.AddRange(UsageDates);
            if (!Directory.Exists(KitDirectoryPath))
            {
                Directory.CreateDirectory(KitDirectoryPath);
            }
            File.WriteAllLines(UseFileLocation, info);
            check_status();
        }

        public int UsesLeft
        {
            get { return usesLeft; }
            set
            {
                usesLeft = value;
                OnPropertyChanged("UsesLeft");
            }
        }

        public bool CanReorder
        {
            get { return can_reorder; }
            set
            {
                can_reorder = value;
                OnPropertyChanged("CanReorder");
            }
        }

        public string UseFileLocation
        {
            get { return useFileLocation; }
            set
            {
                useFileLocation = value;
                OnPropertyChanged("UseFileLocation");
            }
        }

        public string KitNumber
        {
            get { return kitnumber; }
            set
            {
                kitnumber = value;
                OnPropertyChanged("KitNumber");
            }
        }

        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public System.Windows.Media.Brush StatusColor
        {
            get { return statusColor; }
            set
            {
                statusColor = value;
                OnPropertyChanged("StatusColor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        public void add_use(int use, object sender, RoutedEventArgs e)
        {
            DateTime moment = DateTime.Now;
            Present = moment.ToLongDateString() + " " + moment.ToLongTimeString();
            UsesLeft -= use;
            UsesLeftString = $"Uses left: {UsesLeft}";
            UsageDates.Add($"{use} Used: {Present}");
            update_file();
            check_status();
        }

        public void remove_use(int use, object sender, RoutedEventArgs e)
        {
            UsesLeft += use;
            UsesLeftString = $"Uses left: {UsesLeft}";
            DateTime moment = DateTime.Now;
            Present = moment.ToLongDateString() + " " + moment.ToLongTimeString();
            UsageDates.Add($"{use} Replaced: {Present}");
            update_file();
            check_status();
        }

        public void update(object sender, RoutedEventArgs e)
        {
            update_file();
        }
        public void reorder(object sender, RoutedEventArgs e)
        {
            create_reorder_file();
            UsesLeft += total_uses;
            UsesLeftString = $"Uses left: {UsesLeft}";
            UsageDates = new List<string>();
            update_file();
            check_status();
        }

        public void check_status()
        {
            if (UsesLeft == 0)
            {
                StatusColor = System.Windows.Media.Brushes.Red;
                CanReorder = true;
            }
            else if (UsesLeft <= warning_uses * 0.75)
            {
                StatusColor = System.Windows.Media.Brushes.Yellow;
                CanReorder = false;
            }
            else
            {
                StatusColor = System.Windows.Media.Brushes.Green;
                CanReorder = false;
            }
        }
    }
}
