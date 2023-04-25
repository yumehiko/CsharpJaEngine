namespace JaEngine.Text
{

    /// <summary>
    /// 日本語組版における、1文字を表す。
    /// </summary>
    public class JChar
    {
        public char Character { get; }
        public JCharType Type { get; }
        public float MaeAki { get; }
        public float AtoAki { get; }

        public JChar(char character, JCharType type)
        {
            Character = character;
            Type = type;
        }
    }
}