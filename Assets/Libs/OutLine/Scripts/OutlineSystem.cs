using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace LubOutline
{
    [RequireComponent(typeof(Camera)), ExecuteInEditMode]
    public class OutlineSystem : MonoBehaviour
    {
        private static Material clearPass;
        private static Material writePass;

        private static Dictionary<Camera, CommandBuffer> _buffers = new Dictionary<Camera, CommandBuffer>();
        private static Dictionary<Camera, HashSet<OutlineObj>> _outlines = new Dictionary<Camera, HashSet<OutlineObj>>();
        private static Dictionary<Camera, bool> _inited = new Dictionary<Camera, bool>();

        public static void Add(OutlineObj o, Camera cam)
        {
            if (_outlines.TryGetValue(cam, out HashSet<OutlineObj> objects))
            {
                objects.Remove(o);
                objects.Add(o);
            }
            else
            {
                HashSet<OutlineObj> set = new HashSet<OutlineObj>();
                set.Add(o);
                _outlines.Add(cam, set);
            }
            UpdateRender(cam);
        }

        public static void Remove(OutlineObj o, Camera cam)
        {
            if (_outlines.TryGetValue(cam, out HashSet<OutlineObj> objects))
            {
                objects.Remove(o);
            }
            UpdateRender(cam);
        }

        private static bool isDestroy;

        private void Awake()
        {
            isDestroy = false;
        }

        private void OnDestroy()
        {
            isDestroy = true;
        }
        
        private void OnDisable()
        {
            foreach (var val in _buffers)
            {
                Clear(val.Key);
            }
        }

        private void OnEnable()
        {
            clearPass = new Material(Shader.Find("Custom/ClearPass"));
            writePass = new Material(Shader.Find("Custom/ObjOutline"));
        }

        private static void Clear(Camera cam)
        {
            if (_inited.TryGetValue(cam, out bool init) && init)
            {
                if (_buffers.TryGetValue(cam, out CommandBuffer buffer))
                {
                    if (cam == null)
                    {
                        _inited.Remove(cam);
                        _buffers.Remove(cam);
                        return;
                    }
                    cam.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, buffer);
                }
            }
        }

        private void Update()
        {
            _outlines.Clear();

            #if UNITY_EDITOR
            try
            {
                foreach (var buff in _buffers)
                {
                    buff.Key.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, buff.Value);
                }
            }
            catch { }

            #else
            foreach (var buff in _buffers)
            {
                buff.Key.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, buff.Value);
            }
            #endif
            
            _buffers.Clear();
        }

        private static void UpdateRender(Camera cam)
        {
            if (!Application.isEditor && isDestroy) return;
            
            Clear(cam);
            
            CommandBuffer outlineBuffer = new CommandBuffer();
            outlineBuffer.name = "Outline Buffer";
            
            if (!_outlines.TryGetValue(cam, out HashSet<OutlineObj> outlines)) return;

            IEnumerable<OutlineObj> ordered = outlines.OrderBy(o => Vector3.Distance(o.transform.position, cam.transform.position)).Reverse();

            foreach (OutlineObj o in ordered)
            {
                Renderer r = o.Render;
                if (!r) continue;
                for (int i = 0; i < o.SubMeshCount; i++)
                {
                    outlineBuffer.DrawRenderer(r, writePass, i);
                }
                
                for (int i = 0; i < o.SubMeshCount; i++)
                {
                    outlineBuffer.DrawRenderer(r, clearPass, i);
                }
            }

            cam.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, outlineBuffer);
            if (_buffers.ContainsKey(cam)) _buffers.Remove(cam); 
            _buffers.Add(cam, outlineBuffer);
            if (_inited.ContainsKey(cam)) _inited.Remove(cam); 
            _inited.Add(cam, true);
        }
    }
}