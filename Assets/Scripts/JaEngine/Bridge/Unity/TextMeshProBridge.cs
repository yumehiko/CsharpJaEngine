using UnityEngine;
using TMPro;
using JaEngine.Text;
using Cysharp.Threading.Tasks;
using System.Text;

namespace JaEngine.Bridge.Unity
{
    public class TextMeshProBridge : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        
        private void Start()
        {
            // Disable automatic word wrapping
            tmpText.enableWordWrapping = false;
        }
        
        public void RenderText(JText jText, TMP_FontAsset fontAsset)
        {
            tmpText.font = fontAsset;
            tmpText.text = ExtractText(jText);

            // Update mesh after setting the text
            tmpText.ForceMeshUpdate();

            // Update vertex positions based on akikumi values
            UpdateVertexPositions(jText);
        }
        
        public async UniTask RenderTextWithDelay(JText jText, TMP_FontAsset fontAsset, float delayPerCharacter)
        {
            tmpText.font = fontAsset;
            tmpText.text = "";
            int charIndex = 0;

            foreach (var jLine in jText.Lines)
            {
                foreach (var jChar in jLine.JChars)
                {
                    tmpText.text += jChar.Value;
                    tmpText.ForceMeshUpdate();
                    UpdateVertexPosition(jChar, charIndex);
                    charIndex++;

                    // ディレイ
                    await UniTask.Delay((int)(delayPerCharacter * 1000));
                }
                tmpText.text += System.Environment.NewLine;
            }
        }

        private string ExtractText(JText jText)
        {
            var stringBuilder = new StringBuilder();

            foreach (var jLine in jText.Lines)
            {
                foreach (var jChar in jLine.JChars)
                {
                    stringBuilder.Append(jChar.Value);
                }
                stringBuilder.Append(System.Environment.NewLine);
            }

            return stringBuilder.ToString();
        }

        private void UpdateVertexPositions(JText jText)
        {
            var textInfo = tmpText.textInfo;
            var charIndex = 0;

            foreach (var line in jText.Lines)
            {
                foreach (var jChar in line.JChars)
                {
                    UpdateVertexPosition(jChar, charIndex);
                    charIndex++;
                }
            }

            UpdateMeshes(textInfo);
        }
        
        private void UpdateVertexPosition(JChar jChar, int targetCharIndex)
        {
            var textInfo = tmpText.textInfo;
            var charInfo = textInfo.characterInfo[targetCharIndex];

            if (!charInfo.isVisible)
            {
                return;
            }

            var materialIndex = charInfo.materialReferenceIndex;
            var vertexIndex = charInfo.vertexIndex;
            var vertices = textInfo.meshInfo[materialIndex].vertices;
            var maeAki = jChar.MaeAki;

            for (int j = 0; j < 4; j++)
            {
                vertices[vertexIndex + j].x += maeAki;
            }

            // Update mesh with the modified vertex positions
            for (var i = 0; i < textInfo.meshInfo.Length; i++)
            {
                var meshInfo = textInfo.meshInfo[i];
                if (meshInfo.mesh == null) continue;

                meshInfo.mesh.vertices = meshInfo.vertices;
                tmpText.UpdateGeometry(meshInfo.mesh, i);
            }
        }

        private void UpdateMeshes(TMP_TextInfo textInfo)
        {
            for (var i = 0; i < textInfo.meshInfo.Length; i++)
            {
                var meshInfo = textInfo.meshInfo[i];
                if (meshInfo.mesh == null) continue;

                meshInfo.mesh.vertices = meshInfo.vertices;
                tmpText.UpdateGeometry(meshInfo.mesh, i);
            }
        }
    }
}
