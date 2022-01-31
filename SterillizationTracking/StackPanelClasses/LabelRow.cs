using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace SterillizationTracking.StackPanelClasses
{
    class LabelRow : StackPanel
    {
        Label category, kit_number, location_label, uses_left, to_use_replace_reorder, number_in_box, status;
        public LabelRow()
        {
            Orientation = Orientation.Horizontal;
            category = new Label();
            category.Width = 150;
            category.Content = "Category";
            category.Padding = new Thickness(10);
            Children.Add(category);

            kit_number = new Label();
            kit_number.Width = 75;
            kit_number.Content = "Kit #";
            kit_number.Padding = new Thickness(10);
            Children.Add(kit_number);

            location_label = new Label();
            location_label.Width = 150;
            location_label.Content = "Location";
            location_label.Padding = new Thickness(10);
            Children.Add(location_label);

            uses_left = new Label();
            uses_left.Width = 100;
            uses_left.Content = "Total uses left";
            uses_left.Padding = new Thickness(10);
            Children.Add(uses_left);

            uses_left = new Label();
            uses_left.Width = 75;
            uses_left.Content = "";
            uses_left.Padding = new Thickness(10);
            Children.Add(uses_left);

            to_use_replace_reorder = new Label();
            to_use_replace_reorder.Width = 175;
            to_use_replace_reorder.Content = "Use, replace, or reorder";
            to_use_replace_reorder.Padding = new Thickness(10);
            Children.Add(to_use_replace_reorder);

            number_in_box = new Label();
            number_in_box.Width = 175;
            number_in_box.Content = "# per box";
            number_in_box.Padding = new Thickness(10);
            Children.Add(number_in_box);
        }
    }
}
