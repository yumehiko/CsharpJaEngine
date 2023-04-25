using System.Collections.Generic;

namespace JaEngine.Text
{
    /// <summary>
    /// 日本語組版における、1行を表す。
    /// </summary>
    public class JLine
    {
        public List<JChar> Chars { get; } = new List<JChar>();
    }
}