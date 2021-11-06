﻿using System;
using System.Collections.Generic;
using System.IO;
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

namespace TourFirm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            MainFrame.Navigate(new ToursPage());
            Manager.MainFrame = MainFrame;
        }

        private void ImportImagesTours()
        {
            var fileData = File.ReadAllLines(@"C:\Users\user\Desktop\учеба\подготовка к демо\ДЭ\import\Туры.txt");
            var images = Directory.GetFiles(@"C:\Users\user\Desktop\учеба\подготовка к демо\ДЭ\import\Туры фото");

            foreach(var line in fileData)
            {
                var data = line.Split('\t');
                var TempTour = new Tour
                {
                    Name = data[0].Replace("\"", ""),
                    TicketCount = int.Parse(data[2]),
                    Price = decimal.Parse(data[3]),
                    IsActual = (data[4] == "0") ? false : true
                };
                foreach (var tourType in data[5].Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries))
                {
                    var currentType = TurismEntities.GetContext().Types.ToList().FirstOrDefault(p => p.Name == tourType);
                    if (currentType != null)
                        TempTour.Types.Add(currentType);
                }

                try
                {
                    TempTour.ImagePreview = File.ReadAllBytes(images.FirstOrDefault(p => p.Contains(TempTour.Name)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                TurismEntities.GetContext().Tours.Add(TempTour);
                TurismEntities.GetContext().SaveChanges();
            }
           
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if(MainFrame.CanGoBack)
            {
                BtnBack.Visibility = Visibility.Visible;
            }
            else
            {
                BtnBack.Visibility = Visibility.Hidden;
            }
        }

        private void Hotel_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HotelsPage());
        }
    }
}
