using Parser;
using System.IO;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Server.MVVM.Model
{
    internal class DataModel : Presenter
    {
        private int formatVersion;
        private string from;
        private string to;
        private int id;
        private string text;
        private string hexTextColor;
        private string binaryImage;
        private ImageSource image;

        public bool isFilled { get; set; } = false;

        public int FormatVersion
        {
            get { return formatVersion; }
            set
            {
                formatVersion = value;
                OnPropertyChanged("FormatVersion");
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
        public string To
        {
            get { return to; }
            set
            {
                to = value;
                OnPropertyChanged("To");
            }
        }
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
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
            FormatVersion = page.FormatVersion;
            From = page.From;
            To = page.To;
            Id = page.Id;
            Text = page.Text;
            HexTextColor = "#ff" + page.TextColor;
            BinaryImage = page.Image;

            isFilled = true;
        }
    }
}
