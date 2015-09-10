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
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;

namespace InteractiveWall
{
    /// <summary>
    /// Interaction logic for Movies.xaml
    /// </summary>
    public partial class Movies : Page, ISwipeablePage
    {
        public List<Movie> MoviesList;

        public event EventHandler OnSwipeLeft;
        public event EventHandler OnSwipeRight;

        private int cursor { get; set; }

        public Movies()
        {
            InitializeComponent();

            MoviesList = new List<Movie>()
            {
                new Movie("Dune", "/Images/dune.jpg"),
                new Movie("Harry Potter", "Images/harrypotter.jpg"),
                new Movie("Ironman", "Images/ironman.jpg"),
                new Movie("Lord of the Rings", "Images/lordoftherings.jpg"),
                new Movie("Matrix", "Images/matrix.jpg"),
                new Movie("Robo cop", "Images/robocop.jpg"),
                new Movie("Star trek", "Images/startrek.jpg"),
                new Movie("Star wars", "Images/starwars.png"),
                new Movie("Superman", "Images/superman.jpg"),
            };

            for (var i = 0; i < MoviesList.Count; i++)
            {
                Button button = CreateImage(MoviesList[i].Path, i);
                Grid.SetRow(button, (int)Math.Floor((decimal)(i / 3)));
                Grid.SetColumn(button, i % 3);
                MovieGrid.Children.Add(button);
            }

            cursor = 4;
            updateCursor(4);
        }

        private Button CreateImage(string path, int i )
        {
            Button button = new Button();
            button.HorizontalAlignment = HorizontalAlignment.Stretch;
            button.VerticalAlignment = VerticalAlignment.Stretch;
            button.Click += ButtonOnClick;
            button.Name = "Button" + i;

            Image image = new Image();
            image.Margin = new Thickness(5, 5, 5, 5);
            image.Source = new BitmapImage(new Uri(path, UriKind.Relative));
            image.HorizontalAlignment = HorizontalAlignment.Stretch;
            image.VerticalAlignment = VerticalAlignment.Stretch;

            button.Content = image;

            return button;
        }

        private void ButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            NavigationService.Navigate(new Uri("Player.xaml", UriKind.Relative));
        }

        private void updateCursor(int newIndex)
        {
            Console.WriteLine(newIndex);
            if(newIndex >= 0 && newIndex < 9)
            {
                Button currentBtn = (Button)MovieGrid.Children[cursor];
                currentBtn.ClearValue(Button.BackgroundProperty);

                cursor = newIndex;

                Button newBtn = (Button)MovieGrid.Children[cursor];
                newBtn.Background = new SolidColorBrush(Color.FromRgb(210, 210, 250));
            }
        }

        public void SwipeLeft()
        {
            updateCursor(cursor - 1);
        }

        public void SwipeRight()
        {
            updateCursor(cursor + 1);
        }

        public void Open()
        {
            NavigationService.Navigate(new Uri("Player.xaml", UriKind.Relative));
        }
    }
}
