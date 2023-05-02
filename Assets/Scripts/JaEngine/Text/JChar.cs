using System;

namespace JaEngine.Text
{

    /// <summary>
    /// 日本語組版における、1文字を表す。
    /// </summary>
    public class JChar
    {
        public char Value { get; }
        public CharacterClass CharacterClass { get; }
        
        /// <summary>
        /// ルビなどの理由で、前の文字との分割禁止。
        /// </summary>
        public bool UnLineBreakable { get; set; }
        public float MaeAki { get; private set; }
        public float AtoAki { get; private set; }

        public JChar(char value)
        {
            Value = value;
            
            // 文字からタイプを判別し、設定する。
            CharacterClass = GetCharacterClass(value);
            
            // タイプに応じたベタ組み時の前アキ量を設定する。
            MaeAki = CharacterClass switch
            {
                CharacterClass.OpeningBrackets => -0.5f,
                CharacterClass.MiddleDots => -0.25f,
                _ => 0f
            };
            
            // タイプに応じたベタ組時の後アキ量を設定する。
            AtoAki = CharacterClass switch
            {
                CharacterClass.ClosingBrackets => -0.5f,
                CharacterClass.Commas => -0.5f,
                CharacterClass.FullStops => -0.5f,
                CharacterClass.MiddleDots => -0.25f,
                _ => 0f
            };
        }
        
        /// <summary>
        /// 指定したUnicodeの文字が、日本語組版においてどのような文字種に分類されるかを返す。
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static CharacterClass GetCharacterClass(char c)
        {
            throw new System.NotImplementedException();
        }
    }
}