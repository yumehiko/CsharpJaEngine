using TMPro;
using UnityEngine;

namespace JaEngine.Bridge.Unity
{
    public class UnityCharMetricsProvider : ICharMetricsProvider
    {
        private readonly TMP_FontAsset _fontAsset;

        public UnityCharMetricsProvider(TMP_FontAsset fontAsset)
        {
            _fontAsset = fontAsset;
        }

        public float GetWidth(char character, float size)
        {
            uint unicode = character;
            _fontAsset.characterLookupTable.TryGetValue(unicode, out var tmpCharacter);
            if (tmpCharacter == null) return 0;

            _fontAsset.glyphLookupTable.TryGetValue(tmpCharacter.glyphIndex, out var tmpGlyph);
            if (tmpGlyph == null) return 0;

            var width = tmpGlyph.metrics.horizontalAdvance * (size / _fontAsset.faceInfo.pointSize);
            return width;
        }
    }
}