namespace JaEngine.Rule
{
    public abstract class AkikumiRule
    {
        /// <summary>
        /// このルールを適用する。
        /// </summary>
        public abstract void ApplyRule(string text);
    }
}