using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        public void SetPose(Vector3 pivot)
        {
            if (Camera.main != null)
                GetComponent<RectTransform>().localPosition = Camera.main.WorldToScreenPoint(pivot);
        }

        public void SetAmount(float amount)
        {
            image.fillAmount = amount;
        }
    }
}