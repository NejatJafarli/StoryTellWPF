using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using VendingMachineWPF.DataAccess.context;

namespace VendingMachineWPF
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }

        }
        private string _author;

        public string Author
        {
            get { return _author; }
            set { _author = value; OnPropertyRaised(); }
        }

        private string _voice;

        public string Voice
        {
            get { return _voice; }
            set { _voice = value; OnPropertyRaised(); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyRaised(); }
        }

        private string _language;

        public string Languagee
        {
            get { return _language; }
            set { _language = value; OnPropertyRaised(); }
        }

        private string _category;

        public string Category
        {
            get { return _category; }
            set { _category = value; OnPropertyRaised(); }
        }

        private string _image;

        public string Image
        {
            get { return _image; }
            set { _image = value; OnPropertyRaised(); }
        }

        private string _bookName;

        public string BookName
        {
            get { return _bookName; }
            set { _bookName = value; OnPropertyRaised(); }
        }

        private int _rating;

        public int Rating
        {
            get { return _rating; }
            set { _rating = value; OnPropertyRaised(); }
        }

        public string SoundUrl { get; set; }

        public InfoWindow(Book book)
        {
            InitializeComponent();

            DataContext = this;
            SoundUrl = book.SoundLink;
            Languagee = book.Language;
            Category = book.Category;
            Description = book.Descriptions;
            BookName = book.BookName;
            Rating = Convert.ToInt32(book.Rating);
            Author = book.Author;
            Voice = book.VoiceOver;
            Image = book.ImageLink;
        }
        public WaveOutEvent Wo { get; set; } = new WaveOutEvent();

        private async Task PlayMedia()
        {
            await Task.Run(() =>
            {
                var url = SoundUrl;
                using (var mf = new MediaFoundationReader(url))
                using (Wo)
                {
                    Wo.Init(mf);
                    Wo.Play();
                    while (Wo.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(1000);
                    }
                }
            });
        }

        private async void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Wo.PlaybackState == PlaybackState.Playing)
            {
                Wo.Stop();
                return;
            }
            await PlayMedia();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (Wo.PlaybackState == PlaybackState.Playing)
                    Wo.Stop();
                this.Close();
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
