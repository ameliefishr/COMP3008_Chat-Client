using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChatClient
{
    class ViewManager
    {
        public void ShowView<T>(object viewModel) where T: UserControl, new()
        {
            var view = new T();
            var window = new Window();
            view.DataContext = viewModel;
            window.Show();
        }
    }
}
