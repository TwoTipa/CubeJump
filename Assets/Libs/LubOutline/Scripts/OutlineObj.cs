using UnityEngine;

namespace LubOutline
{
    [ExecuteInEditMode]
    public class OutlineObj : MonoBehaviour
    {
        public Renderer Render { get; private set; }
        public int SubMeshCount { get; private set; }

        [SerializeField] private bool useScaleShader;
        
        [SerializeField] private Color color = Color.white;
        [SerializeField] public float outlineWidth = 1f;

        public void OnEnable()
        {
            Init();
        }

        private void OnValidate()
        {
            if (!Application.isEditor) return;
            Init();
        }

        public void Init()
        {
            Render = GetComponent<Renderer>();
            if (TryGetComponent(out MeshFilter filter))
            {
                SubMeshCount = filter.sharedMesh.subMeshCount;
            } else if (TryGetComponent(out SkinnedMeshRenderer renderer))
            {
                SubMeshCount = renderer.sharedMesh.subMeshCount;
            }
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            Render.GetPropertyBlock(block);
            block.SetInt("_UseScale", useScaleShader ? 1 : 0);
            block.SetColor("_ColorOut", color);
            block.SetFloat("_OutWidth", outlineWidth);
            Render.SetPropertyBlock(block);
        }

        private void OnWillRenderObject()
        {
            if (!enabled) return;
            OutlineSystem.Add(this, Camera.current);
        }
    }
}