using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BonusTimerPrefab : MonoBehaviour
    {
        [SerializeField] private Image img;

        public GameObject Initialize(Image image)
        {
            img.sprite = image.sprite;
            return gameObject;
        }
    }
}