using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    public PokemonBaseStats baseStats { get; set; }
    public int level { get; set; }

    public int Exp { get; set; }
    public int currentHealth { get; set; }

    public List<Moves> Moves { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }

    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();

    public Pokemon(PokemonBaseStats pBase, int pLevel)
    {
        baseStats = pBase;
        level = pLevel;

        Moves = new List<Moves>();
        foreach (var move in baseStats.LearnableMoves)
        {
            if (move.Level <= level)
            {
                Moves.Add(new Moves(move.Base));
            }
            if (Moves.Count >= 4)
            {
                break;
            }
        }

        Exp = baseStats.GetExpForLevel(level);

        CalculateStats();

        currentHealth = MaxHealth;

        ResetStatBoost();
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((baseStats.Attack * level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((baseStats.Defense * level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((baseStats.SpAttack * level) / 100f) + 5);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt((baseStats.SpDefense * level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((baseStats.Speed * level) / 100f) + 5);

        MaxHealth = Mathf.FloorToInt((baseStats.MaxHealth * level) / 100f) + 10;
    }

    void ResetStatBoost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            { Stat.Attack, 0 },
            { Stat.Defense, 0 },
            { Stat.SpAttack, 0 },
            { Stat.SpDefense, 0 },
            { Stat.Speed, 0 },
        };
    }

    int GetStat(Stat stat)
    {
        int statValue = Stats[stat];

        int boost = StatBoosts[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
        {
            statValue = Mathf.FloorToInt(statValue * boostValues[boost]);
        }
        else
        {
            statValue = Mathf.FloorToInt(statValue / boostValues[-boost]);
        }

        return statValue;
    }

    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach (var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

            if (boost > 0)
            {
                StatusChanges.Enqueue($"{baseStats.name}'s {stat} rose!");
            }
            else
            {
                StatusChanges.Enqueue($"{baseStats.name}'s {stat} fell!");
            }

            Debug.Log($"{stat} has been boosted to {StatBoosts[stat]}");
        }
    }

    public bool CheckForLevelUp()
    {
        if (Exp > baseStats.GetExpForLevel(level + 1))
        {
            ++level;
            return true;
        }

        return false;
    }

    public int MaxHealth { get; private set; }

    public int Attack
    {
        get { return GetStat(Stat.Attack); }
    }

    public int Defense
    {
        get { return GetStat(Stat.Defense); }
    }

    public int SpAttack
    {
        get { return GetStat(Stat.SpAttack); }
    }

    public int SpDefense
    {
        get { return GetStat(Stat.SpDefense); }
    }

    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }

    public DamageDetails TakeDamage(Moves move, Pokemon attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
        {
            critical = 2f;
        }

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.baseStats.TypePrimary) * TypeChart.GetEffectiveness(move.Base.Type, this.baseStats.TypeSecondary);

        var damageDetails = new DamageDetails()
        {
            typeEffectiveness = type,
            critical = critical,
            fainted = false
        };

        float attack = (move.Base.Category == MoveCategory.Special) ? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.Category == MoveCategory.Special) ? SpDefense : Defense;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            damageDetails.fainted = true;
        }

        return damageDetails;
    }

    public Moves GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails
{
    public bool fainted { get; set; }
    public float critical { get; set; }
    public float typeEffectiveness { get; set; }
}