using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazoView.Config
{
    /// <summary>
    /// 動的なステータスを格納予定
    /// </summary>
    public class State
    {
        /// <summary>
        /// 拡縮モード
        /// </summary>
        public bool ScalingMode = false;

        /// <summary>
        /// トリミングモード
        /// </summary>
        public bool TrimmingMode = false;
    }
}
