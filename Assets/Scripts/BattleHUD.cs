using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Healthbar healthBar;
    [SerializeField] GameObject expBar;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.baseStats.Name;
        levelText.text = "" + pokemon.level;
        healthBar.SetHealth((float) pokemon.currentHealth / pokemon.MaxHealth);
        SetExp();
    }

    public void SetExp()
    {
        if (expBar == null) return;

        float normalizedExp = GetNormalizedExp();
        expBar.transform.localScale = new Vector3(normalizedExp, 1, 1);
    }

    public IEnumerator SetExpSmooth()
    {
        if (expBar == null) yield break;

        float normalizedExp = GetNormalizedExp();
        yield return expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();
    }

    float GetNormalizedExp()
    {
        int currentLevelExp = _pokemon.baseStats.GetExpForLevel(_pokemon.level);
        int nextLevelExp = _pokemon.baseStats.GetExpForLevel(_pokemon.level + 1);

        float normalizedExp = (float)(_pokemon.Exp - currentLevelExp) / (nextLevelExp - currentLevelExp);
        return Mathf.Clamp01(normalizedExp);
    }

    public IEnumerator UpdateHealth()
    {
        yield return healthBar.SmoothSlider((float)_pokemon.currentHealth / _pokemon.MaxHealth);
    }
}