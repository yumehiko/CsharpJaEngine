using System.Collections.Generic;
using JaEngine.Text;
using System;
using JaEngine.Bridge;

namespace JaEngine.Text
{
    public class JTextConfig
    {
        /// <summary>
        /// フォントの基本サイズ。
        /// </summary>
        public float FontSize { get; }

        /// <summary>
        /// 行長。文字の長さ単位。
        /// </summary>
        public int LineLength { get; }
        
        /// <summary>
        /// 文字幅を取得するためのプロバイダ。
        /// </summary>
        public ICharMetricsProvider CharMetricsProvider { get; }
        
        public JTextConfig(float fontSize, int lineLength, ICharMetricsProvider charMetricsProvider)
        {
            FontSize = fontSize;
            LineLength = lineLength;
            CharMetricsProvider = charMetricsProvider;
        }

        /// <summary>
        /// 与えられた2つの文字間の、基本のアキ量を返す。
        /// 例えば、前の文字が句点の場合、0.5fを返す。
        /// </summary>
        /// <param name="previousChar"></param>
        /// <param name="nextChar"></param>
        /// <returns></returns>
        public float GetDefaultAkiByPair(JChar previousChar, JChar nextChar)
        {
            // TODO: 最終的には、ベースアキ量ルールセットを読み込み、それを順に解決するという仕組みにする。
            
            // 判定用のローカル関数
            bool IsWesternOrNumerals(CharacterClass cClass) =>
                cClass is CharacterClass.WesternCharacters or CharacterClass.EuropeanNumerals;

            bool IsNotWesternOrNumerals(CharacterClass cClass) => !IsWesternOrNumerals(cClass);

            // 次の文字が約物系のアキ量
            var nextCharAki = nextChar.CharacterClass switch
            {
                CharacterClass.MiddleDots => 0.25f,
                CharacterClass.ClosingBrackets or CharacterClass.Commas or CharacterClass.FullStops => 0.0f,
                CharacterClass.OpeningBrackets => 0.5f,
                _ => 0.0f
            };

            if (nextCharAki != 0.0f)
            {
                return nextCharAki;
            }

            // 前の文字が約物系のアキ量
            var previousCharAki = previousChar.CharacterClass switch
            {
                CharacterClass.MiddleDots => 0.25f,
                CharacterClass.ClosingBrackets or CharacterClass.Commas or CharacterClass.FullStops => 0.5f,
                CharacterClass.OpeningBrackets => 0.0f,
                _ => 0.0f
            };

            if (previousCharAki != 0.0f)
            {
                return previousCharAki;
            }

            // 前の文字が英数字であり、かつ次の文字が英数字または後置省略記号でなければ、その後アキは0.25f。
            if (IsWesternOrNumerals(previousChar.CharacterClass)
                && IsNotWesternOrNumerals(nextChar.CharacterClass)
                && nextChar.CharacterClass != CharacterClass.PostFixedAbbreviations)
            {
                return 0.25f;
            }

            // 次の文字が英数字であり、かつ前の文字が英数字または前置省略記号でなければ、その前アキは0.25f。
            if (IsWesternOrNumerals(nextChar.CharacterClass)
                && IsNotWesternOrNumerals(previousChar.CharacterClass)
                && previousChar.CharacterClass != CharacterClass.PreFixedAbbreviations)
            {
                return 0.25f;
            }

            return 0.0f;
        }

    }
}