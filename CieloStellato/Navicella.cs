using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace cieloStellato
{
    class Navicella
    {
        string id;

        public Image N { get; set; }

        public Navicella(int x0, int y0, Canvas x)
        {
            BitmapImage immagine = new BitmapImage(new Uri(@"img/Ship.png", UriKind.Relative));
            N = new Image();
            N.Source = immagine;
            N.Width = immagine.Width;
            N.Height = immagine.Height;
            Canvas.SetLeft(N, x0);
            Canvas.SetTop(N, y0);
            id = "Navicella: " + x.Children.Add(N);
        }

        //public void Rimuovi(Canvas x)
        //{
        //    x.Children.RemoveAt(id);
        //}

        public override string ToString()
        {
            return id;
        }
    }
}
