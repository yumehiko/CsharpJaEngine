using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;

namespace JaEngine.Text
{
    public class JText
    {
        public List<JLine> Lines { get; }

        public JText(string rawText, JTextConfig config)
        {
            try
            {
                Lines = GenerateJLines(rawText, config);
            }
            catch (ArgumentException e)
            {
                Debug.LogError(e.Message);
                throw;
            }
        }
        
        /// <summary>
        /// 与えられた文字列から、行を生成する。
        /// </summary>
        /// <param name="rawText"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private List<JLine> GenerateJLines(string rawText, JTextConfig config)
        {
            if (rawText.Length == 0)
            {
                throw new ArgumentException("rawText is empty.");
            }
            var result = new List<JLine>();
            var rawTextIndex = 0;
            // 最初の行を作成。
            var currentLine = new JLine(config);

            // すべての文字が処理されるまで繰り返す。
            while (rawTextIndex < rawText.Length)
            {
                // 分割禁止グループを作成する。
                var prohibitedGroup = CreateProhibitedGroup(rawText, ref rawTextIndex);
                //単純追加できるか確認し、できるなら単純追加する。
                if (currentLine.CanAddProhibitedGroup(prohibitedGroup))
                {
                    currentLine.AddProhibitedGroup(prohibitedGroup);
                    continue;
                }

                // 単純追加できない場合、調整タイプを取得する。
                var adjustType = currentLine.GetAdjustType(prohibitedGroup);

                switch (adjustType)
                {
                    // 追い込みの場合、追い込み追加し、新たな行を定義する。
                    case AdjustType.Oikomi:
                        currentLine.AddProhibitedGroup(prohibitedGroup);
                        currentLine.AdjustLineWidth();
                        result.Add(currentLine);
                        currentLine = new JLine(config);
                        continue;
                    // 追い出しの場合、現在の行は確定し、新たな行を定義し、そこにグループを追加する。
                    case AdjustType.Oidashi:
                        currentLine.AdjustLineWidth();
                        result.Add(currentLine);
                        currentLine = new JLine(config);
                        currentLine.AddProhibitedGroup(prohibitedGroup);
                        continue;
                    default:
                        throw new ArgumentException("未定義のAdjustTypeです。");
                }
            }

            return result;
        }

        /// <summary>
        /// 指定した文字列の先頭から分割禁止グループを作成し、返す。
        /// </summary>
        /// <param name="rawText"></param>
        /// <param name="rawTextIndex"></param>
        /// <returns></returns>
        private List<JChar> CreateProhibitedGroup(string rawText, ref int rawTextIndex)
        {
            var prohibitedGroup = new List<JChar>();
            JChar previousChar = null;

            while (rawTextIndex < rawText.Length)
            {
                var currentChar = rawText[rawTextIndex];
                var jChar = new JChar(currentChar);

                if (previousChar != null && !IsProhibitedPair(previousChar, jChar))
                {
                    break;
                }

                prohibitedGroup.Add(jChar);
                rawTextIndex++;
                previousChar = jChar;
            }

            return prohibitedGroup;
        }

    
        /// <summary>
        /// 指定した文字が、その前の文字と分割禁止ペアであるかを返す。
        /// </summary>
        /// <returns></returns>
        private bool IsProhibitedPair(JChar previousChar, JChar currentChar)
        {
            // その文字が行頭禁則なら、分割禁止。
            if (currentChar.CharacterClass 
                is CharacterClass.ClosingBrackets 
                or CharacterClass.MiddleDots
                or CharacterClass.FullStops 
                or CharacterClass.Commas
                or CharacterClass.VerticalKanaRepeatLowerMarks
                or CharacterClass.FullWidthIdeographicSpace)
            {
                return true;
            }

            // 前の文字が行末禁則なら、分割禁止。
            if (previousChar.CharacterClass 
                is CharacterClass.OpeningBrackets
                or CharacterClass.VerticalKanaRepeatUpperMarks)
            {
                return true;
            }
            
            // 行頭・行末禁則でないなら、その文字が分割禁止かどうかを単純チェックする。
            if (IsAllowedPair(currentChar))
            {
                return false;
            }

            // TODO:その他の分割禁止組み合わせを調べる
            return previousChar.CharacterClass switch
            {
                // 数字の後に数字・アルファベット・後置省略記号が続く場合は分割禁止
                CharacterClass.EuropeanNumerals when 
                    currentChar.CharacterClass 
                        is CharacterClass.EuropeanNumerals 
                        or CharacterClass.WesternCharacters 
                        or CharacterClass.PostFixedAbbreviations => true,
                // アルファベットの後に数字・アルファベットが続く場合は分割禁止
                CharacterClass.WesternCharacters when 
                    currentChar.CharacterClass 
                        is CharacterClass.EuropeanNumerals 
                        or CharacterClass.WesternCharacters => true,
                // 前置省略記号の後の文字が数字の場合、分割禁止
                CharacterClass.PreFixedAbbreviations when 
                    currentChar.CharacterClass 
                        is CharacterClass.EuropeanNumerals => true,
                // 分離禁止文字：EmDash同士の組み合わせは分割禁止
                CharacterClass.EmDash when 
                    currentChar.CharacterClass 
                        is CharacterClass.EmDash => true,
                // 分離禁止文字：三点リーダー同士の組み合わせは分割禁止
                CharacterClass.ThreeDotLeader when 
                    currentChar.CharacterClass 
                        is CharacterClass.ThreeDotLeader => true,
                // 分離禁止文字：二点リーダー同士の組み合わせは分割禁止
                CharacterClass.TwoDotLeader when 
                    currentChar.CharacterClass 
                        is CharacterClass.TwoDotLeader => true,
                // それ以外は分割許可
                _ => false
            };
        }
        
        private bool IsAllowedPair(JChar jChar)
        {
            if (jChar.UnLineBreakable)
            {
                return false;
            }

            var isAllowedClass = jChar.CharacterClass
                is CharacterClass.Hiragana
                or CharacterClass.Katakana
                or CharacterClass.IdeographicCharacters;

            if (isAllowedClass)
            {
                return true;
            }
            
            // 他の文字クラスに対する処理を追加する場合、ここに記述します。

            // 最後に、デフォルトの判断を返します。
            return false;
        }

    }
}
