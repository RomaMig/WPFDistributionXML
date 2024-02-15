using Parser;
using System.IO;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client.MVVM.Model
{
    class DataModel : Presenter
    {
        private DateTime date;
        private string from;
        private string text;
        private string hexTextColor;
        private string binaryImage;
        private ImageSource image;

        public DateTime Date
        {
            get { return date; }
            set 
            { 
                date = value; 
                OnPropertyChanged("Date");
            }
        }

        public string From
        {
            get { return from; }
            set
            {
                from = value;
                OnPropertyChanged("From");
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }
        public string HexTextColor
        {
            get { return hexTextColor; }
            set
            {
                hexTextColor = value;
                OnPropertyChanged("HexTextColor");
            }
        }
        public string BinaryImage
        {
            get { return binaryImage; }
            set
            {
                binaryImage = value;

                byte[] bitmapBytes = Convert.FromBase64String(binaryImage);
                BitmapImage bmp = new BitmapImage();
                using (MemoryStream memoryStream = new MemoryStream(bitmapBytes))
                {
                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.StreamSource = memoryStream;
                    bmp.EndInit();
                    Image = bmp;
                }
                OnPropertyChanged("BinaryImage");
            }
        }
        public ImageSource Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }

        public void setPage(Page page)
        {
            From = page.From;
            Text = page.Text;
            HexTextColor = "#ff" + page.TextColor;
            BinaryImage = page.Image;
        }
    }
}
