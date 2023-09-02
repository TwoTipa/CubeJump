using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BonusTimer
    {
        private float _timer;
        private Image _image;
        private GameObject _visualPresenter;
        private TextMeshProUGUI _text;
        
        public BonusTimer(float delay, Image image, GameObject visualPresenter)
        {
            _image = image;
            _timer = delay;
            _visualPresenter = visualPresenter;
            _text = _visualPresenter.GetComponentInChildren<TextMeshProUGUI>();
        }

        public GameObject GetVisualPresenter()
        {
            return _visualPresenter;
        }
        
        public bool Tick()
        {
            _timer -= Time.deltaTime;
            
            if (_timer <= 0)
            {
                return true;
            }

            _text.text = Math.Round(_timer)+"s";
            return false;
        }

        public Image GetImage()
        {
            return _image;
        }
        
        public override string ToString()
        {
            var timeSpan = TimeSpan.FromSeconds(_timer);
            return timeSpan.Seconds.ToString();
        }
    }
}