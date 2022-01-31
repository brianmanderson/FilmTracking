using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Runtime.CompilerServices;


namespace SterillizationTracking.Windows
{
    /// <summary>
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window, INotifyPropertyChanged
    {
        private string applicator_directory;
        private string category_name;
        private string total_uses;
        public event PropertyChangedEventHandler PropertyChanged;
        private List<string> _category_names = new List<string> {};
        public List<string> Category_Names
        {
            get
            {
                return _category_names;
            }
            set
            {
                _category_names = value;
                OnPropertyChanged("Category_Names");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        public CategoryWindow(string applicator_directory)
        {
            InitializeComponent();
            this.applicator_directory = applicator_directory;
            Rebuild();
        }
        private void Rebuild()
        {
            string[] applicator_list = Directory.GetDirectories(applicator_directory);
            _category_names = new List<string> { "" };
            foreach (string applicator in applicator_list)
            {
                string[] temp_list = applicator.Split('\\');
                string applicator_name = temp_list[temp_list.Length - 1];
                if (applicator_name != "Archived")
                {
                    _category_names.Add(applicator_name);
                }
            }
            bind();
            CheckBoxStatusCheck();
        }
        private void bind()
        {
            Binding category_binding = new Binding("Category_Names");
            category_binding.Source = this;
            CategoriesComboBox.SetBinding(ComboBox.ItemsSourceProperty, category_binding);
        }
        private void CheckButton()
        {
            AddCategoryButton.IsEnabled = false;
            if (CategoryText.Text != "")
            {
                if (UsesText.Text != "")
                {
                    if (int.TryParse(UsesText.Text, out _))
                    {
                        AddCategoryButton.IsEnabled = true;
                    }
                }
            }
        }
        private void CategoryNameChanged(object sender, TextChangedEventArgs e)
        {
            this.category_name = CategoryText.Text;
            CheckButton();
        }

        private void UsesChanged(object sender, TextChangedEventArgs e)
        {
            this.total_uses = UsesText.Text;
            CheckButton();
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            string out_path = Path.Join(applicator_directory, category_name);
            string out_file = Path.Join(out_path, "Total_Uses.txt");
            if (!Directory.Exists(out_path))
            {
                Directory.CreateDirectory(out_path);
            }
            if (!File.Exists(out_file))
            {
                string[] info = { $"Total Uses:{total_uses}" };
                File.WriteAllLines(out_file, info);
            }
            UsesText.Text = "";
            CategoryText.Text = "";
            Rebuild();
        }

        private void CheckBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            bool delete_checked = DeleteCheckBox.IsChecked ?? false;
            bool archived_check = ArchiveCheckBox.IsChecked ?? false;
            if (delete_checked)
            {
                ArchiveCheckBox.IsEnabled = false;
                ProcessButton.Content = "Delete";
                ProcessButton.IsEnabled = true;
            }
            else if (archived_check)
            {
                DeleteCheckBox.IsEnabled = false;
                ProcessButton.Content = "Archive";
                ProcessButton.IsEnabled = true;
            }
            else
            {
                reset();
            }
        }
        private void reset()
        {
            ProcessButton.IsEnabled = false;
            ProcessButton.Content = "Process";
            ArchiveCheckBox.IsEnabled = true;
            DeleteCheckBox.IsEnabled = true;
        }
        private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckBoxStatusCheck();
        }
        private void CheckBoxStatusCheck()
        {
            if (CategoriesComboBox.SelectedIndex != -1)
            {
                ArchiveCheckBox.IsEnabled = true;
                DeleteCheckBox.IsEnabled = true;
            }
            else
            {
                ArchiveCheckBox.IsEnabled = false;
                DeleteCheckBox.IsEnabled = false;
                ProcessButton.Content = "Process";
            }
        }
        public static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
            {
                return;
            }
            foreach (var dir in baseDir.EnumerateDirectories())
            {
                RecursiveDelete(dir);
            }
            baseDir.Delete(true);
        }
        private void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            bool delete_checked = DeleteCheckBox.IsChecked ?? false;
            bool archived_check = ArchiveCheckBox.IsChecked ?? false;
            string category = Category_Names[CategoriesComboBox.SelectedIndex];
            if (delete_checked)
            {
                RecursiveDelete(new DirectoryInfo(Path.Combine(applicator_directory, category)));
            }
            else if (archived_check)
            {

            }
            DeleteCheckBox.IsChecked = false;
            ArchiveCheckBox.IsChecked = false;
            Rebuild();
        }
    }
}
