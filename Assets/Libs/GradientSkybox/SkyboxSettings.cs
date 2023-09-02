using UnityEngine;

namespace Libs.GradientSkybox
{
    [ExecuteAlways]
    public class SkyboxSettings : MonoBehaviour
    {
        [ColorUsage(false)] public Color TopCollor;
        [ColorUsage(false)] public Color BottomCollor;
        [SerializeField] public float exponent;

        private void OnValidate()
        {
            UpdateState();
        }

        private void OnEnable()
        {
            UpdateState();
        }

        private void LateUpdate()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            Shader.SetGlobalColor("TopColorSkybox", TopCollor);
            Shader.SetGlobalColor("BottomColorSkybox", BottomCollor);
            Shader.SetGlobalVector("UpSkybox", transform.up);
            Shader.SetGlobalFloat("ExpSkybox", exponent);
        }
    }
}