using System;
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
using System.Windows.Shapes;
using System.Runtime.CompilerServices;


namespace SterillizationTracking.Windows
{
    /// <summary>
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {
        public CategoryWindow()
        {
            InitializeComponent();
        }
        int n;
        private void CheckButton()
        {
            AddCategoryButton.IsEnabled = false;
            if (CategoryText.Text != "")
            {
                if (UsesText.Text != "")
                {
                    if (int.TryParse(UsesText.Text, out n))
                    {
                        AddCategoryButton.IsEnabled = true;
                    }
                }
            }
        }
        private void CategoryNameChanged(object sender, TextChangedEventArgs e)
        {
            CheckButton();
        }

        private void UsesChanged(object sender, TextChangedEventArgs e)
        {
            CheckButton();
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
