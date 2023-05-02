namespace JaEngine.Text
{
    /// <summary>
    /// 文字クラスを定義する。あらゆる文字はこのクラスのいずれかに分類される。
    /// </summary>
    public enum CharacterClass
    {
        OpeningBrackets, // 初め括弧類
        ClosingBrackets, // 終わり括弧類
        Hyphens, // ハイフン類
        DividingPunctuationMarks, // 区切り約物
        MiddleDots, // 中点類
        FullStops, // 句点類
        Commas, // 読点類
        EmDash, // EMダッシュ
        ThreeDotLeader, // 三点リーダー
        TwoDotLeader, // 二点リーダー
        VerticalKanaRepeatUpperMarks, // 縦書きくの字点上
        VerticalKanaRepeatLowerMarks, // 縦書きくの字点下
        IterationMarks, // 繰返し記号
        ProlongedSoundMarks, // 長音記号
        SmallKana, // 小書きの仮名
        PreFixedAbbreviations, // 前置省略記号
        PostFixedAbbreviations, // 後置省略記号
        FullWidthIdeographicSpace, // 和字間隔
        Hiragana, // 平仮名
        Katakana, // 片仮名
        MathSymbols, // 等号類
        MathOperators, // 演算記号
        IdeographicCharacters, // 漢字等
        EuropeanNumerals, // アラビア数字
        WesternWordSpace, // 欧文間隔
        WesternCharacters, // 欧文用文字
    }
}