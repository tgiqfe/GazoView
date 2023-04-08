using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Lib.Config
{
    internal class ImageCollection
    {
        /// <summary>
        /// 使用可能な拡張子
        /// </summary>
        private static string[] _extensions = new string[]
        {
            ".jpg", ".jpeg", ".png", ".tif", ".tiff", ".svg", ".bmp", ".gif"
        };

        private List<string> _fileNameList = null;

        private int _currentIndex = 0;

        public int CurrentIndex
        {
            get
            {
                if (_currentIndex < 0)
                {
                    _currentIndex = _fileNameList.Count - 1;
                }
                else if (_currentIndex >= _fileNameList.Count)
                {
                    _currentIndex = 0;
                }
                return _currentIndex;
            }
            set { _currentIndex = value; }
        }


    }
}
