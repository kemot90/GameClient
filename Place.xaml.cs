using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RPGClient
{
    /// <summary>
    /// Interaction logic for Place.xaml
    /// </summary>
    public partial class Place : Window
    {
        private Spot spot;
        private Enemies en;

        private List<Border> mobs;

        public Place(Spot _spot, Enemies _en)
        {
            InitializeComponent();

            mobs = new List<Border>();

            spot = _spot;
            en = _en;

            idLoc.Text = spot.IdLoc.ToString();
            idCity.Text = spot.IdCity.ToString();
            SpotType.Text = spot.Type.ToString();
            SpotName.Text = spot.Name;

            //foreach (Mob mb in en.EnemiesList)
            //{
            //    Moby.Text = Moby.Text + "\n" + mb.Id.ToString() + " " + mb.Name + " " + mb.Level.ToString() + " " + mb.BonusHP + " " + mb.Strength.ToString() + " " + mb.Luck.ToString() + " " + mb.Dexterity.ToString() + " " + mb.Stamina.ToString() + " " + mb.GoldDrop.ToString();
            //}
            CreateMobList();
        }

        private void CreateMobList()
        {
            foreach (Mob mob in en.EnemiesList)
            {
                Border border = new Border();
                border.CornerRadius = new CornerRadius(12);
                border.Margin = new Thickness(5);
                border.Padding = new Thickness(5);
                border.Background = new SolidColorBrush(Color.FromArgb(136, 0, 0, 0));
                border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                border.BorderThickness = new Thickness(2);

                Grid gridContainer = new Grid();

                ColumnDefinition col0 = new ColumnDefinition();
                col0.Width = new GridLength(50, GridUnitType.Pixel);
                ColumnDefinition col1 = new ColumnDefinition();
                col1.Width = new GridLength(130, GridUnitType.Pixel);
                ColumnDefinition col2 = new ColumnDefinition();
                col2.Width = new GridLength(50, GridUnitType.Pixel);

                gridContainer.ColumnDefinitions.Add(col0);
                gridContainer.ColumnDefinitions.Add(col1);
                gridContainer.ColumnDefinitions.Add(col2);

                Image img = new Image();
                img.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Mobs/" + mob.IconName));
                img.Width = 50;
                img.Height = 50;

                StackPanel infoPanel = new StackPanel();
                TextBlock name = new TextBlock();
                name.Foreground = Brushes.White;
                name.Text = mob.Name;
                TextBlock lvl = new TextBlock();
                lvl.Foreground = Brushes.White;
                lvl.Text = "Poziom: " + mob.Level;
                TextBlock exp = new TextBlock();
                exp.Foreground = Brushes.White;
                exp.Text = "Exp: " + mob.ExpDrop;

                infoPanel.Children.Add(name);
                infoPanel.Children.Add(lvl);
                infoPanel.Children.Add(exp);

                Button fight = new Button();
                fight.Width = 50;
                fight.Content = "Walcz";
                fight.Tag = mob;

                Grid.SetColumn(img, 0);
                gridContainer.Children.Add(img);
                Grid.SetColumn(infoPanel, 1);
                gridContainer.Children.Add(infoPanel);
                Grid.SetColumn(fight, 2);
                gridContainer.Children.Add(fight);

                

                border.Child = gridContainer;

                mobList.Children.Add(border);

                mobs.Add(border);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
