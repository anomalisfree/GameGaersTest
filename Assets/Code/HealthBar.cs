using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image image = null;
        [SerializeField] private Text text = null;

        private float _currentLife;

        public void Initialize(float life)
        {
            _currentLife = life;

            gameObject.SetActive(true);
            image.fillAmount = 1;
        }

        public void SetPose(Vector3 pivot)
        {
            if (Camera.main != null)
                GetComponent<RectTransform>().localPosition = Camera.main.WorldToScreenPoint(pivot);
        }

        public void SetAmount(float life, float maxLife)
        {
            if (life <= 0)
            {
                gameObject.SetActive(false);
                return;
            }

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

            gameObject.SetActive(true);
            image.fillAmount = life / maxLife;
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