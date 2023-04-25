using System.Collections.Generic;

namespace JaEngine.Text
{

    /// <summary>
    /// 日本語組版で扱うテキストを表す。
    /// </summary>
    public class JText
    {
        public List<JLine> Lines { get; } = new List<JLine>();

        public JText(string rawText, JTextConfig config)
        {

        }
    }
}