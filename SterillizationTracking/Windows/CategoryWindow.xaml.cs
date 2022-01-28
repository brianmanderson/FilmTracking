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
    public partial class CategoryWindow : Window
    {
        private string applicator_directory;
        private string category_name;
        private string total_uses;
        public CategoryWindow(string applicator_directory)
        {
            InitializeComponent();
            this.applicator_directory = applicator_directory;
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
        }
    }
}
