using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

namespace Bonuses
{
    public class CameraRotateBonus : Bonus
    {
        private float _angle;
        private Transform model;
        
        public override void Init(BonusSetting setting)
        {
            base.Init(setting);
            _angle = (Random.Range(0, 2) == 1 ? -1 : 1) * 90;
            model = transform.GetChild(0).transform;
            model.rotation = UnityEngine.Quaternion.Euler(0,_angle,90);
        }
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.transform.TryGetComponent(out Player.Player player))
            {
                return;
            }

            CameraController.Instance.Rotate( _angle);
            Destroy(gameObject);

        }
    }
}