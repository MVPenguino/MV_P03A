using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PokemonInBattle : MonoBehaviour
{
    [SerializeField] PokemonBaseStats baseStats;
    [SerializeField] int level;
    [SerializeField] bool isPlayerPokemon;

    public bool IsPlayerUnit
    {
        get { return IsPlayerUnit;  }
    }

    public Pokemon pokemon { get; set; }

    Image image;
    Vector3 originalPosition;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPosition = image.transform.localPosition;
        originalColor = image.color;
    }
    public void Setup()
    {
        pokemon = new Pokemon(baseStats, level);
        if (isPlayerPokemon)
        {
            image.sprite = pokemon.baseStats.BackSprite;
        }
        else
        {
            image.sprite = pokemon.baseStats.FrontSprite;
        }

        image.color = originalColor;
        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        if (isPlayerPokemon)
        {
            image.transform.localPosition = new Vector3(-500f, originalPosition.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPosition.y);
        }

        image.transform.DOLocalMoveX(originalPosition.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerPokemon)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPosition.x + 50f, 0.25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPosition.x - 50f, 0.25f));
        }

        sequence.Append(image.transform.DOLocalMoveX(originalPosition.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0, .1f));
        sequence.Append(image.DOFade(1, .1f));
        sequence.Append(image.DOFade(0, .1f));
        sequence.Append(image.DOFade(1, .1f));
        sequence.Append(image.DOFade(0, .1f));
        sequence.Append(image.DOFade(1, .1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPosition.y - 150f, .5f));
        sequence.Join(image.DOFade(0f, 0.25f));
    }
}