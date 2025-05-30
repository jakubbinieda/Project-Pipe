using UnityEngine;
using UnityEngine.UI;

namespace ProjectPipe
{
    public class UIStatBar : MonoBehaviour
    {
        protected Slider _slider;

        protected virtual void Awake()
        {
            _slider = GetComponent<Slider>();
        }
        
        protected virtual void Start()
        {
        }

        public virtual void SetStat(int newValue)
        {
            _slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            _slider.maxValue = maxValue;
            _slider.value = maxValue;
        }
    }
}
