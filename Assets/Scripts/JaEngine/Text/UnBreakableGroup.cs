using System.Collections.Generic;

namespace JaEngine.Text
{
    /// <summary>
    /// 行で分割が不可能なJCharのグループ。
    /// </summary>
    public class UnBreakableGroup
    {
        public List<JChar> JChars { get; }
        public float Width { get; private set; }
        public float MaeAki { get; private set; }

        private JTextConfig _config;

        public UnBreakableGroup(JTextConfig config)
        {
            _config = config;
            JChars = new List<JChar>();
        }
        
        /// <summary>
        /// 文字を追加する。
        /// </summary>
        /// <param name="jChar"></param>
        public void Add(JChar jChar)
        {
            JChars.Add(jChar);
            Width = jChar.Width;
            MaeAki = jChar.MaeAki;
            
            // 文字が1文字目なら、そのまま終了。
            if (JChars.Count == 1)
            {
                return;
            }
            
            // 2文字目以降なら、リストの最後の文字とのアキ量取得し、前アキに加算する。
            var aki = _config.GetDefaultAkiByPair(JChars[^2], jChar);
            jChar.AddMaeAki(aki);
        }

        /// <summary>
        /// 前アキを追加する。
        /// </summary>
        /// <param name="value"></param>
        public void AddMaeAki(float value)
        {
            MaeAki += value;
        }
    }
}