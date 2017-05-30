using BookFinder.Amazon;
using BookFinder.Apress;
using BookFinder.GoogleBooksAPI;
using BookFinder.Interfaces;
using BookFinder.Model;
using BookFinder.Model.GoogleBooks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace BookFinder.View
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region properties
        public string BookTitle
        {
            get { return _bookTitle; }
            set
            {
                if(string.Equals(_bookTitle, value)) {
                    return;
                }
                _bookTitle = value;
                OnPropertyChanged();
            }
        }
        public string BookISBN13
        {
            get { return _bookISBN13; }
            set
            {
                if (string.Equals(_bookISBN13, value))
                {
                    return;
                }
                _bookISBN13 = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<VolumeInfo> VolumesInfo
        {
            get { return _volumesInfo; }
            set
            {
                _volumesInfo = value;
                OnPropertyChanged();
            }
        }
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region fields
        private string _bookTitle;
        private string _bookISBN13;
        private ObservableCollection<VolumeInfo> _volumesInfo;
        private bool _isLoading;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            _volumesInfo = new ObservableCollection<VolumeInfo>();
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

        #region private methods
        private Book FindBestOffer(List<Book> foundBooks)
        {
            Book bestOffer = null;

            foreach(var book in foundBooks)
            {
                var price = book.Price.Convert(Currency.PLN);
                if (bestOffer == null)
                    bestOffer = book;
                else if (price.Cost < bestOffer.Price.Cost)
                    bestOffer = book;
            }

            return bestOffer;
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(BookTitle))
                return;

            _isLoading = true;
            var client = new GoogleBooksClient();
            var booksResult = await client.SearchBookByTitle(BookTitle);

            VolumesInfo = new ObservableCollection<VolumeInfo>(booksResult);
            _isLoading = false;
        }

        private async void btnCompareBooksWithISBN_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(BookISBN13))
                return;

            _isLoading = true;
            await FindBestOffer(BookISBN13);
            _isLoading = false;
        }

        private Task FindBestOffer(string isbn13)
        {
            isbn13 = isbn13.Replace("-", "");
            return Task.Run(() =>
            {
                List<Book> foundBooks = new List<Book>();
                List<IBookSearch> bookMarkets = new List<IBookSearch>();
                bookMarkets.Add(new AmazonClient());
                bookMarkets.Add(new ApressClient());

                foreach (var market in bookMarkets)
                {
                    var book = market.SearchBook(isbn13);
                    if (book != null)
                    {
                        foundBooks.Add(book);
                    }
                }

                var bestOffer = FindBestOffer(foundBooks);
                
                if(bestOffer == null)
                {
                    var msg = "Nie znaleziono oferty dla poszukiwanej książki.";
                    MessageBox.Show(msg, "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    string msg = "Najlepsza oferta znaleziona w " + bestOffer.Market + " (" + string.Format("{0:N2}", bestOffer.Price.Cost) + " PLN)\nCzy chcesz przejść do sklepu?";
                    if (MessageBox.Show(msg, "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(bestOffer.Url);
                    }
                }
            });
        }

        private async void lblVolumes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selected = lblVolumes.SelectedItem as VolumeInfo;
            if (selected != null && !string.IsNullOrEmpty(selected.ISBN13))
            {
                await FindBestOffer(selected.ISBN13);
            }                        
        }
        #endregion
    }
}
