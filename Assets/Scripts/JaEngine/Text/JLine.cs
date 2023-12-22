using System.Collections.Generic;
using System;

namespace JaEngine.Text
{
    public class JLine
    {
        public List<UnBreakableGroup> Body { get; }
        private float _currentWidth;
        private readonly JTextConfig _config;
        private const float Epsilon = 0.001f;
        private JChar _lastChar;

        public JLine(JTextConfig config)
        {
            Body = new List<UnBreakableGroup>();
            _config = config;
            _currentWidth = 0.0f;
        }

        /// <summary>
        /// 指定した分割禁止グループを、単純追加できるかを返す。
        /// </summary>
        /// <param name="unBreakableGroup"></param>
        /// <returns></returns>
        public bool CanAddProhibitedGroup(UnBreakableGroup unBreakableGroup)
        {
            double groupWidth = CalculateGroupWidth(unBreakableGroup);
            return _currentWidth + groupWidth <= _config.LineLength + Epsilon;
        }
        
        /// <summary>
        /// 分割グループをこの行に追加する。
        /// </summary>
        /// <param name="unBreakableGroup"></param>
        public void AddProhibitedGroup(UnBreakableGroup unBreakableGroup)
        {
            Body.Add(unBreakableGroup);
            _currentWidth += CalculateGroupWidth(unBreakableGroup);
        }

        /// <summary>
        /// 溢れが発生する行に対する追加の場合の、調整タイプを返す。
        /// </summary>
        /// <param name="unBreakableGroup"></param>
        /// <returns></returns>
        public AdjustType GetAdjustType(UnBreakableGroup unBreakableGroup)
        {
            //単純追加できないなら、追い込み処理か追い出し処理かを比較し、判定する。
            var oikomiAmount = GetOikomiAmount(unBreakableGroup);
            var oidashiAmount = GetOidashiAmount(unBreakableGroup);
            
            // oikomiAmountが負の値を返した場合、追い込み処理は不可能なので追い出し処理とする。
            if (oikomiAmount < 0)
            {
                return AdjustType.Oidashi;
            }

            // oikomiとoidashiの量を比較し、小さい方を採用する。
            return oidashiAmount < oikomiAmount ? AdjustType.Oidashi : AdjustType.Oikomi;
        }
        
        /// <summary>
        /// 現在の行の文字幅を、設定された最大行幅に合わせる。
        /// </summary>
        public void AdjustLineWidth()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定した分割禁止グループを追い込み処理で追加する場合の、調整量を算出する。
        /// </summary>
        /// <param name="addingGroup"></param>
        /// <returns></returns>
        private float GetOikomiAmount(UnBreakableGroup addingGroup)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 指定した分割禁止グループを追い出し処理で追加する場合の、調整量を算出する。
        /// </summary>
        /// <param name="addingGroup"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private float GetOidashiAmount(UnBreakableGroup addingGroup)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定された分割グループの長さを算出する。
        /// </summary>
        /// <param name="unBreakableGroup"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private float CalculateGroupWidth(UnBreakableGroup unBreakableGroup)
        {
            // TODO: Implement CalculateGroupWidth.
            throw new NotImplementedException();
        }
    }
}