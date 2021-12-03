using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Healthbar healthBar;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.baseStats.Name;
        levelText.text = "" + pokemon.level;
        healthBar.SetHealth((float) pokemon.currentHealth / pokemon.MaxHealth);
    }

    public IEnumerator UpdateHealth()
    {
        yield return healthBar.SmoothSlider((float)_pokemon.currentHealth / _pokemon.MaxHealth);
    }
}