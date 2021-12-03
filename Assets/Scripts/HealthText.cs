using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    [SerializeField] Text healthText;

    public void SetHealthData(Pokemon pokemon)
    {
        healthText.text = (2 * pokemon.MaxHealth) + "/" + (2 * pokemon.MaxHealth);
    }

    public IEnumerator UpdateHealthText(Pokemon pokemon)
    {
        yield return healthText.text = (2 * pokemon.currentHealth) + "/" + (2 * pokemon.MaxHealth);
    }
}