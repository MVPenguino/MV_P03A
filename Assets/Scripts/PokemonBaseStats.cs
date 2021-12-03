using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create New Pokemon")]
public class PokemonBaseStats : ScriptableObject
{
    [SerializeField] string pokemonName;

    [TextArea]
    [SerializeField] string description;

    [Header("Visuals")]
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [Header("Category")]
    [SerializeField] PokemonType typePrimary;
    [SerializeField] PokemonType typeSecondary;

    [Header("Stats")]
    [SerializeField] int maxHealth;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    [SerializeField] int expYield;
    [SerializeField] GrowthRate growthRate;

    [Header("Move List")]
    [SerializeField] List<LearnableMoves> learnableMoves;

    public int GetExpForLevel(int level)
    {
        if (growthRate == GrowthRate.Fast)
        {
            return 4 * (level * level * level) / 5;
        }
        else if (growthRate == GrowthRate.MediumFast)
        {
            return level * level * level;
        }

        return -1;
    }

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }

    public Sprite BackSprite
    {
        get { return backSprite; }
    }

    public PokemonType TypePrimary
    {
        get { return typePrimary;  }
    }

    public PokemonType TypeSecondary
    {
        get { return typeSecondary; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public int SpAttack
    {
        get { return spAttack; }
    }

    public int SpDefense
    {
        get { return spDefense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public int ExpYield => expYield;

    public GrowthRate GrowthRate => growthRate;

    public List<LearnableMoves> LearnableMoves
    {
        get { return learnableMoves; }
    }
}

[System.Serializable]
public class LearnableMoves
{
    [SerializeField] MoveBaseAttacks moveBaseAttacks;
    [SerializeField] int level;

    public MoveBaseAttacks Base
    {
        get { return moveBaseAttacks; }
    }

    public int Level
    {
        get { return level; }
    }
}

public enum PokemonType
{
    None,
    Normal,
    Bug,
    Dark,
    Dragon,
    Electric,
    Fairy,
    Fire,
    Fighting,
    Flying,
    Grass,
    Ground,
    Ghost,
    Ice,
    Poison,
    Psychic,
    Rock,
    Steel,
    Water
}

public enum GrowthRate
{
    Fast, MediumFast
}

public enum Stat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed
}

public class TypeChart
{
    static float[][] chart =
    {
        //                      NOR   BUG   DAR   DRA   ELE   FAI   FIR   FIG   FLY   GRA   GRO   GHO   ICE   POI   PSY   ROC   STE   WAT
        /* NOR */ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f },
        /* BUG */ new float[] { 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 2.0f, 2.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f },
        /* DAR */ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
        /* DRA */ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
        /* ELE */ new float[] { 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f },
        /* FAI */ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
        /* FIR */ new float[] { 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 0.5f, 2.0f, 0.5f },
        /* FIG */ new float[] { 2.0f, 0.5f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 0.0f, 2.0f, 0.5f, 0.5f, 2.0f, 2.0f, 1.0f },
        /* FLY */ new float[] { 1.0f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f },
        /* GRA */ new float[] { 1.0f, 0.5f, 1.0f, 0.5f, 1.0f, 1.0f, 0.5f, 1.0f, 0.5f, 0.5f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 2.0f, 0.5f, 2.0f },
        /* GRO */ new float[] { 1.0f, 0.5f, 1.0f, 1.0f, 2.0f, 1.0f, 2.0f, 1.0f, 0.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 2.0f, 2.0f, 1.0f },
        /* GHO */ new float[] { 0.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f },
        /* ICE */ new float[] { 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 2.0f, 2.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f },
        /* POI */ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 0.5f, 1.0f, 0.5f, 1.0f, 0.5f, 0.0f, 1.0f },
        /* PSY */ new float[] { 1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 1.0f },
        /* ROC */ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
        /* STE */ new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f },
        /* WAT */ new float[] { 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f },
    };

    public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        if (attackType == PokemonType.None || defenseType == PokemonType.None)
        {
            return 1;
        }

        int row = (int)attackType - 1;
        int column = (int)defenseType - 1;

        return chart[row][column];
    }
}