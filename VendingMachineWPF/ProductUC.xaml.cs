using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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
using VendingMachineWPF.DataAccess.concrete;
using VendingMachineWPF.DataAccess.context;
using VendingMachineWPF.Model;

namespace VendingMachineWPF
{
    /// <summary>
    /// Interaction logic for ProductUC.xaml
    /// </summary>
    public partial class ProductUC : UserControl, INotifyPropertyChanged
    {
        private Book product;
        public Book Product
        {
            get { return product; }
            set { product = value; OnPropertyRaised(); }
        }
        private void OnPropertyRaised([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyRaised(); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public ProductUC(Book book)
        {
            InitializeComponent();
            Product = book;
            DataContext = this;
            Text = book.BookName;
        }
    }
}
