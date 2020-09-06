using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Config
{
    public class ThemeBase
    {
        public virtual string WindowBackground { get; set; }        //  ウィンドウ全体の背景色
        public virtual string DragOverBackground { get; set; }      //  ファイルをDragOver時の背景色
        public virtual string TitleForeground { get; set; }         //  タイトルバーのフォントの文字色
        public virtual string ImageInfoForeground { get; set; }     //  ImageInfoパネルのフォントの文字色
    }
}
