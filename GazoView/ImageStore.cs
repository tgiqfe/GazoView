using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using SharpVectors.Renderers.Wpf;
using SharpVectors.Converters;
using SharpVectors.Dom.Svg;
using GazoView.Functions;

namespace GazoView
{
    class ImageStore
    {
        private static string[] _imageExtensions = new string[]
        {
            ".jpg", ".jpeg", ".png", ".tif", ".tiff", ".svg", ".bmp"
        };
        private WpfDrawingSettings _drawingSettings = null;

        public List<string> Items { get; set; }

        private bool _isAllImage = false;
        public bool IsAllImage { get { return _isAllImage; } }

        private int _currentIndex;
        public int CurrentIndex
        {
            get
            {
                if (_currentIndex < 0)
                {
                    _currentIndex = Items.Count - 1;
                }
                else if (_currentIndex >= Items.Count)
                {
                    _currentIndex = 0;
                }
                return _currentIndex;
            }
            set { this._currentIndex = value; }
        }

        private BitmapImage _currentBitmapImage = null;
        private DrawingImage _currentDrawingImage = null;
        public ImageSource CurrentImageSource
        {
            get
            {
                if (_currentBitmapImage != null)
                {
                    return _currentBitmapImage;
                }
                else if (_currentDrawingImage != null)
                {
                    return _currentDrawingImage;
                }
                return null;
            }
        }

        public string CurrentPath
        {
            get { return (Items != null && Items.Count > 0) ? Items[CurrentIndex] : null; }
        }

        private string _currentParentDirectory = null;
        public string CurrentParentDirectory
        {
            get { return _currentParentDirectory; }
        }

        private double _currentWidth = 0;
        public double CurrentWidth
        {
            get
            {
                if (_currentBitmapImage != null)
                {
                    _currentWidth = _currentBitmapImage.PixelWidth;
                }
                else if (_currentDrawingImage != null)
                {
                    _currentWidth = _currentDrawingImage.Width;
                }
                return _currentWidth;
            }
        }

        private double _currentHeight = 0;
        public double CurrentHeight
        {
            get
            {
                if (_currentBitmapImage != null)
                {
                    _currentHeight = _currentBitmapImage.PixelHeight;
                }
                else if (_currentDrawingImage != null)
                {
                    _currentHeight = _currentDrawingImage.Height;
                }
                return _currentHeight;
            }
        }

        public ImageStore() { }
        public ImageStore(string[] itemPaths)
        {
            SetItems(itemPaths);
        }

        /// <summary>
        /// 指定したインデックスの画像をBitmapImageで返す
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private BitmapImage GetBitMapImage(string targetImagePath)
        {
            _currentDrawingImage = null;

            using (FileStream fs = new FileStream(targetImagePath, FileMode.Open, FileAccess.Read))
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.CreateOptions = BitmapCreateOptions.None;
                bmp.StreamSource = fs;
                bmp.EndInit();
                bmp.Freeze();

                _currentBitmapImage = bmp;
                return _currentBitmapImage;
            }
        }

        /// <summary>
        /// 指定したインデックスのSVG画像をDrawingImageで返す
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private DrawingImage GetDrawingImage(string targetImagePath)
        {
            _currentBitmapImage = null;

            if (_drawingSettings == null)
            {
                _drawingSettings = new WpfDrawingSettings();
                _drawingSettings.IncludeRuntime = true;
                _drawingSettings.TextAsGeometry = false;
            }
            using (var converter = new StreamSvgConverter(_drawingSettings))
            using (MemoryStream ms = new MemoryStream())
            {
                if (converter.Convert(targetImagePath, ms))
                {
                    _currentDrawingImage = new DrawingImage(converter.Drawing);
                    return _currentDrawingImage;
                }
            }
            return null;
        }

        /// <summary>
        /// 現在のインデックスの画像をBitmapImageで返す
        /// </summary>
        /// <returns></returns>
        public ImageSource GetCurrentImageSource()
        {
            if (CurrentIndex >= 0 && CurrentIndex < Items.Count)
            {
                string targetImagePath = Items[CurrentIndex];

                if (Path.GetExtension(targetImagePath) == ".svg")
                {
                    return GetDrawingImage(targetImagePath);
                }
                return GetBitMapImage(targetImagePath);
            }
            _currentBitmapImage = null;
            _currentDrawingImage = null;
            return null;
        }

        /// <summary>
        /// ファイルパス(文字列配列)からItemsにセット
        /// </summary>
        /// <param name="itemPaths"></param>
        public void SetItems(string[] itemPaths)
        {
            if (Items == null) { this.Items = new List<string>(); }
            if (Items.Count > 0) { this.Items.Clear(); }
            if (itemPaths.Length == 1)
            {
                //  同フォルダー内の全画像ファイルを対象
                this._isAllImage = true;
                this._currentParentDirectory = Path.GetDirectoryName(itemPaths[0]);
                this.Items = Directory.GetFiles(_currentParentDirectory).
                    Where(x => _imageExtensions.Contains(Path.GetExtension(x).ToLower())).
                    OrderBy(x => x, new NaturalStringComparer()).
                    ToList();
                CurrentIndex = Items.IndexOf(itemPaths[0]);
            }
            else if (itemPaths.Length > 1)
            {
                //  指定したファイルのみが対象
                this._isAllImage = false;
                this._currentParentDirectory = null;
                this.Items = itemPaths.
                    OrderBy(x => x, new NaturalStringComparer()).ToList();
                CurrentIndex = 0;
            }
        }

        /// <summary>
        /// ファイルパス (List&lt;string&gt;)からItemsにセット
        /// </summary>
        /// <param name="itemPaths"></param>
        public void SetItems(List<string> itemPaths)
        {
            SetItems(itemPaths.ToArray());
        }

        /// <summary>
        /// ファイルパス (1つだけ) からItemsにセット
        /// </summary>
        /// <param name="itemPath"></param>
        public void SetItem(string itemPath)
        {
            SetItems(new string[] { itemPath });
        }

        /// <summary>
        /// タイトルバーに表示する為の画像ファイルの名前とパスを返す。
        /// </summary>
        /// <returns></returns>
        public string GetCurrentImageTitle()
        {
            if (Items.Count > 0 && CurrentIndex >= 0)
            {
                return string.Format("[ {0} ] {1}",
                    Path.GetFileName(Items[CurrentIndex]), Items[CurrentIndex]);
            }
            return "";
        }

        /// <summary>
        /// Itemsの更新
        /// </summary>
        public void UpdateItems()
        {
            if (Items == null) { return; }

            string tempImagePath = Items[CurrentIndex];

            List<string> tempItems = Directory.GetFiles(_currentParentDirectory).
                Where(x => _imageExtensions.Contains(Path.GetExtension(x).ToLower())).
                OrderBy(x => x, new NaturalStringComparer()).
                ToList();
            if (!Items.SequenceEqual(tempItems))
            {
                this.Items = tempItems;
                CurrentIndex = Items.IndexOf(tempImagePath);
            }
        }
    }
}
