namespace JaEngine.Bridge
{
    /// <summary>
    /// ある文字の幅を取得するためのインターフェース。
    /// </summary>
    public interface ICharMetricsProvider
    {
        /// <summary>
        /// 指定した文字の幅を取得する。
        /// </summary>
        /// <param name="character"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        float GetWidth(char character, float size);
    }
}