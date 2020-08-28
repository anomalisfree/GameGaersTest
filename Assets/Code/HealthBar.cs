using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Text text;

        private float _currentLife;

        public void SetPose(Vector3 pivot)
        {
            if (Camera.main != null)
                GetComponent<RectTransform>().localPosition = Camera.main.WorldToScreenPoint(pivot);
        }

        public void SetAmount(float life, float maxLife, bool isStarting)
        {
            if (life <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                image.fillAmount = life / maxLife;

                if (!isStarting)
                    if (life > _currentLife)
                    {
                        text.color = Color.green;
                        text.text = $"+{(int) (life - _currentLife)}";
                        CreateNewLabel();
                    }
                    else if (life < _currentLife)
                    {
                        text.color = Color.red;
                        text.text = $"-{(int) (_currentLife - life)}";
                        CreateNewLabel();
                    }
            }

            _currentLife = life;
        }

        private void CreateNewLabel()
        {
            var label = Instantiate(text.gameObject, transform);
            label.SetActive(true);
            Destroy(label, 1);
        }
    }
}