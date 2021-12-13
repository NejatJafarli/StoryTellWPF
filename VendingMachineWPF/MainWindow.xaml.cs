using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public GenericRepository<Book> Repository { get; set; } = new GenericRepository<Book>();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
        public ObservableCollection<ProductUC> ProductsUC { get; set; } = new ObservableCollection<ProductUC>();

        public ObservableCollection<string> CategoryCB { get; set; }
        public ObservableCollection<string> LanguageCB { get; set; }
        public IQueryable<Book> DefaultBook { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            PreviewKeyDown += EscHandle;

            DefaultBook = Repository.GetAll();
            QueryForAllBooks();

            CategoryCB = new ObservableCollection<string>(DefaultBook.Select(x => x.Category).Distinct().ToList());
            CategoryCB.Insert(0, "ALL");
            CatCB.SelectedIndex = 0;

            SortCB.Items.Add("Kitab Adi");
            SortCB.Items.Add("Yazar");
            SortCB.Items.Add("Yayin Tarihi");
            LanguageCB = new ObservableCollection<string>(DefaultBook.Select(x => x.Language).Distinct().ToList());
            MyDB = DefaultBook;
        }
        private void QueryForAllBooks()
        {
            var temp = (DefaultBook.ToList());

            ProductsUC.Clear();
            for (int i = 0; i < temp.Count; i++)
            {
                var ProductUC = new ProductUC(temp[i]);
                ProductUC.Window.Width = 220;
                ProductUC.Window.Height = 280;
                ProductsUC.Add(ProductUC);
            }
        }
        private void EscHandle(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Application.Current.Shutdown();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => SearchTxt.Focus();
        private void SearchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox txt)
            {

                if (!string.IsNullOrWhiteSpace(txt.Text))
                {
                    var temp = DefaultBook;
                    if (!(CatCB.SelectedItem is null))
                        if (CatCB.SelectedItem.ToString() != "ALL")
                            temp = temp.Where(c => c.Category == CatCB.SelectedItem.ToString());
                    if (!(LanCB.SelectedItem is null))
                        temp = temp.Where(c => c.Language == LanCB.SelectedItem.ToString());

                    MyDB = temp.Where(p => p.BookName.ToLower().Contains(txt.Text.ToLower()));
                    SortCB_SelectionChanged(null, null);
                }
                else
                {
                    MyDB = DefaultBook;
                    if (!(CatCB.SelectedItem is null))
                        if (CatCB.SelectedItem.ToString() != "ALL")
                            MyDB = MyDB.Where(c => c.Category == CatCB.SelectedItem.ToString());
                    if (!(LanCB.SelectedItem is null))
                        MyDB = MyDB.Where(c => c.Language == LanCB.SelectedItem.ToString());

                    SortCB_SelectionChanged(null, null);
                }
            }
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => Application.Current.Shutdown();
        public IQueryable<Book> MyDB { get; set; }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (sender is ComboBox CB)
            {
                if (CB.SelectedItem is null) return;
                if (CB.SelectedItem.ToString() == "ALL")
                    MyDB = DefaultBook;
                else
                    MyDB = DefaultBook.Where(x => x.Category == CB.SelectedItem.ToString());

                if (!(LanCB.SelectedItem is null))
                    MyDB = MyDB.Where(c => c.Language == LanCB.SelectedItem.ToString());
                SortCB_SelectionChanged(null, null);
            }
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox CB)
            {
                if (CB.SelectedItem is null) return;

                MyDB = MyDB.Where(b => b.Language == CB.SelectedItem.ToString());
                SortCB_SelectionChanged(null, null);
            }
        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Book> temp = new List<Book>();
            if (SortCB.SelectedIndex == 0)
                temp = MyDB.OrderBy(b => b.BookName).ToList();
            else if (SortCB.SelectedIndex == 1)
                temp = MyDB.OrderBy(b => b.Author).ToList();
            else if (SortCB.SelectedIndex == 2)
                temp = MyDB.OrderBy(b => b.PublishedDate).ToList();
            else
                temp = MyDB.ToList();
            ProductsUC.Clear();
            for (int i = 0; i < temp.Count; i++)
            {
                var ProductUC = new ProductUC(temp[i]);
                ProductUC.Window.Width = 220;
                ProductUC.Window.Height = 280;
                ProductsUC.Add(ProductUC);
            }
        }

        private void ListBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox lt)
            {
                if (lt.SelectedItem is null) return;

                var newWindow = new InfoWindow((lt.SelectedItem as ProductUC).Product);
                newWindow.ShowDialog();

            }
        }
    }
}
