using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;

    private void Start()
    {
        StartBattle();
        battleSystem.OnBattleOver += EndBattle;
    }

    public void StartBattle()
    {
        battleSystem.StartBattle();
    }

    void EndBattle(bool won)
    {
        
    }
}