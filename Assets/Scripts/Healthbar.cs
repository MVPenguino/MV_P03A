using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void ResetHealth()
    {
        slider.value = 1;

        fill.color = gradient.Evaluate(slider.value);
    }

    public void SetHealth(float healthNormalized)
    {
        slider.value = healthNormalized;
    }

    public IEnumerator SmoothSlider(float newHealth)
    {
        float _currentHealth = slider.value;
        float changeAmount = _currentHealth - newHealth;

        while (_currentHealth - newHealth > Mathf.Epsilon)
        {
            _currentHealth -= changeAmount * Time.deltaTime;
            slider.value = _currentHealth;

            yield return null;
        }

        slider.value = newHealth;

        fill.color = gradient.Evaluate(slider.value);
    }
}