using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, Busy, Complete }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] GameController gameController;

    [Header("Classes")]
    [SerializeField] PokemonInBattle playerPokemon;
    [SerializeField] PokemonInBattle enemyPokemon;
    [SerializeField] BattleHUD playerHUD;
    [SerializeField] BattleHUD enemyHUD;
    [SerializeField] HealthText playerHealthText;
    [SerializeField] Healthbar enemyHealthBar;
    [SerializeField] Dialogue dialogue;

    [Header("GameObjects")]
    [SerializeField] GameObject playerOptions;
    [SerializeField] GameObject dialogueBox;

    [Header("Action Selection Arrows")]
    [SerializeField] Image arrowSelectFight;
    [SerializeField] Image arrowSelectBag;
    [SerializeField] Image arrowSelectPokemon;
    [SerializeField] Image arrowSelectRun;

    [Header("Move Selection Arrows")]
    [SerializeField] Image arrowSelectMove1;
    [SerializeField] Image arrowSelectMove2;
    [SerializeField] Image arrowSelectMove3;
    [SerializeField] Image arrowSelectMove4;

    [Header("Music")]
    public AudioSource audioSourceGameMusic;
    public AudioClip audioClipGameMusic;
    public AudioClip audioClipWinMusic;

    [Header("SFX")]
    [SerializeField] AudioClip arrowSelectSFX = null;
    [SerializeField] AudioClip normalHitSFX = null;
    [SerializeField] AudioClip ineffectiveHitSFX = null;
    [SerializeField] AudioClip superEffectiveHitSFX = null;
    [SerializeField] AudioClip pokemonFaintSFX = null;

    [Header("MoveSFX")]
    [SerializeField] AudioClip growlSFX = null;
    [SerializeField] AudioClip tackleSFX = null;
    [SerializeField] AudioClip vineWhipSFX = null;
    [SerializeField] AudioClip growthSFX = null;
    [SerializeField] AudioClip scratchSFX = null;
    [SerializeField] AudioClip emberSFX = null;
    [SerializeField] AudioClip thunderShockSFX = null;
    [SerializeField] AudioClip tailWhipSFX = null;
    [SerializeField] AudioClip waterGunSFX = null;
    [SerializeField] AudioClip withdrawSFX = null;

    public event Action<bool> OnBattleOver;

    BattleState state;
    bool loadingOptionRematch = false;
    bool loadingOptionNoRematch = false;
    bool actionInProgress = false;
    int currentAction;
    int currentMove;

    private void Awake()
    {
        playerOptions.SetActive(false);

        arrowSelectFight.enabled = true;
        arrowSelectBag.enabled = false;
        arrowSelectPokemon.enabled = false;
        arrowSelectRun.enabled = false;

        arrowSelectMove1.enabled = false;
        arrowSelectMove2.enabled = false;
        arrowSelectMove3.enabled = false;
        arrowSelectMove4.enabled = false;
    }

    public void StartBattle()
    {
        StartCoroutine(SetupBattle());

        ChangeToGameMusic(audioClipGameMusic);
        audioSourceGameMusic.Play();
    }

    public IEnumerator SetupBattle()
    {
        playerPokemon.Setup();
        enemyPokemon.Setup();
        playerHUD.SetData(playerPokemon.pokemon);
        enemyHUD.SetData(enemyPokemon.pokemon);
        enemyHealthBar.ResetHealth();
        playerHealthText.SetHealthData(playerPokemon.pokemon);

        dialogue.SetMoveNames(playerPokemon.pokemon.Moves);

        yield return dialogue.TypeDialogue($"Oh! A wild {enemyPokemon.pokemon.baseStats.Name} appeared!");

        ActionSelection();
    }

    public void ChangeToWinMusic(AudioClip winMusic)
    {
        audioSourceGameMusic.clip = winMusic;
        audioSourceGameMusic.Play();
    }

    public void ChangeToGameMusic(AudioClip gameMusic)
    {
        audioSourceGameMusic.clip = gameMusic;
        audioSourceGameMusic.Play();
    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;

        StartCoroutine(dialogue.TypeDialogue($"What will \n{playerPokemon.pokemon.baseStats.Name} do?"));

        playerOptions.SetActive(true);
        dialogue.EnableActionSelector(true);
    }

    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogue.EnableActionSelector(false);
        dialogue.EnableDialogueText(false);
        dialogueBox.SetActive(false);
        dialogue.EnableMoveSelector(true);
    }

    IEnumerator PlayerRun()
    {
        yield return dialogue.TypeDialogue("You can't run from this battle!");

        yield return new WaitForSeconds(1f);
    }

    IEnumerator PlayerBag()
    {
        yield return dialogue.TypeDialogue("Your bag is empty!");

        yield return new WaitForSeconds(1f);
    }

    IEnumerator PlayerPokemon()
    {
        yield return dialogue.TypeDialogue("You have no other Pokemon!");

        yield return new WaitForSeconds(1f);
    }

    IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;

        var move = playerPokemon.pokemon.Moves[currentMove];

        if (move.PP > 0)
        {
            move.PP--;

            yield return new WaitForSeconds(1f);
            yield return dialogue.TypeDialogue($"{playerPokemon.pokemon.baseStats.Name} used {move.Base.Name}!");

            if (move.Base.Name == "Growl")
            {
                AudioSource.PlayClipAtPoint(growlSFX, transform.position);
            }
            if (move.Base.Name == "Tackle")
            {
                AudioSource.PlayClipAtPoint(tackleSFX, transform.position);
            }
            if (move.Base.Name == "Vine Whip")
            {
                AudioSource.PlayClipAtPoint(vineWhipSFX, transform.position);
            }
            if (move.Base.Name == "Growth")
            {
                AudioSource.PlayClipAtPoint(growthSFX, transform.position);
            }
            if (move.Base.Name == "Scratch")
            {
                AudioSource.PlayClipAtPoint(scratchSFX, transform.position);
            }
            if (move.Base.Name == "Ember")
            {
                AudioSource.PlayClipAtPoint(emberSFX, transform.position);
            }
            if (move.Base.Name == "Thunder Shock")
            {
                AudioSource.PlayClipAtPoint(thunderShockSFX, transform.position);
            }
            if (move.Base.Name == "Tail Whip")
            {
                AudioSource.PlayClipAtPoint(tailWhipSFX, transform.position);
            }
            if (move.Base.Name == "Water Gun")
            {
                AudioSource.PlayClipAtPoint(waterGunSFX, transform.position);
            }
            if (move.Base.Name == "Withdraw")
            {
                AudioSource.PlayClipAtPoint(withdrawSFX, transform.position);
            }

            playerPokemon.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);

            enemyPokemon.PlayHitAnimation();

            if (move.Base.Category == MoveCategory.Status)
            {
                var effects = move.Base.Effects;
                if (effects.Boosts != null)
                {
                    if (move.Base.Target == MoveTarget.Self)
                    {
                        playerPokemon.pokemon.ApplyBoosts(move.Base.Effects.Boosts);
                    }
                    else
                    {
                        enemyPokemon.pokemon.ApplyBoosts(move.Base.Effects.Boosts);
                    }

                    yield return ShowStatusChanges(playerPokemon.pokemon);
                    yield return ShowStatusChanges(enemyPokemon.pokemon);
                }
            }

            var damageDetails = enemyPokemon.pokemon.TakeDamage(move, playerPokemon.pokemon);
            yield return enemyHUD.UpdateHealth();
            if (damageDetails.typeEffectiveness > 1f)
            {
                AudioSource.PlayClipAtPoint(superEffectiveHitSFX, transform.position);
            }
            if (damageDetails.typeEffectiveness < 1f)
            {
               AudioSource.PlayClipAtPoint(ineffectiveHitSFX, transform.position);
            }
            else if (damageDetails.typeEffectiveness == 1)
            {
                AudioSource.PlayClipAtPoint(normalHitSFX, transform.position);
            }
                yield return ShowDamageDetails(damageDetails);

            if (enemyPokemon.pokemon.currentHealth <= 0)
            {
                yield return dialogue.TypeDialogue($"The wild {enemyPokemon.pokemon.baseStats.Name} fainted.");
                enemyPokemon.PlayFaintAnimation();
                AudioSource.PlayClipAtPoint(pokemonFaintSFX, transform.position);

                yield return new WaitForSeconds(1f);
                audioSourceGameMusic.Stop();
                if (audioClipWinMusic != null)
                {
                    ChangeToWinMusic(audioClipWinMusic);
                }

                // exp gain
                int expYield = playerPokemon.pokemon.baseStats.ExpYield;
                int enemyLevel = enemyPokemon.pokemon.level;

                int expGain = Mathf.FloorToInt((expYield * enemyLevel) / 7);
                playerPokemon.pokemon.Exp += expGain;
                yield return dialogue.TypeDialogue($"{playerPokemon.pokemon.baseStats.Name} gained {expGain} exp.");
                yield return playerHUD.SetExpSmooth();

                // check level up
                while (playerPokemon.pokemon.CheckForLevelUp())
                {
                    playerHUD.SetLevel();
                    yield return dialogue.TypeDialogue($"{playerPokemon.pokemon.baseStats.Name} is now Level {playerPokemon.pokemon.level}!");

                    yield return playerHUD.SetExpSmooth(true);
                }

                yield return new WaitForSeconds(2f);
                OnBattleOver(true);
                state = BattleState.Complete;

                yield return new WaitForSeconds(1f);
                yield return dialogue.TypeDialogue("Want to continue playing? \nPress Y for a rematch and N to quit the game.");
            }
            else
            {
                StartCoroutine(EnemyMove());
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            yield return dialogue.TypeDialogue($"{playerPokemon.pokemon.baseStats.Name} cannot perform this move anymore.");

            MoveSelection();
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.PerformMove;

        var move = enemyPokemon.pokemon.GetRandomMove();
        move.PP--;
        yield return dialogue.TypeDialogue($"The wild {enemyPokemon.pokemon.baseStats.Name} used {move.Base.Name}!");

        if (move.Base.Name == "Growl")
        {
            AudioSource.PlayClipAtPoint(growlSFX, transform.position);
        }
        if (move.Base.Name == "Tackle")
        {
            AudioSource.PlayClipAtPoint(tackleSFX, transform.position);
        }
        if (move.Base.Name == "Vine Whip")
        {
            AudioSource.PlayClipAtPoint(vineWhipSFX, transform.position);
        }
        if (move.Base.Name == "Growth")
        {
            AudioSource.PlayClipAtPoint(growthSFX, transform.position);
        }
        if (move.Base.Name == "Scratch")
        {
            AudioSource.PlayClipAtPoint(scratchSFX, transform.position);
        }
        if (move.Base.Name == "Ember")
        {
            AudioSource.PlayClipAtPoint(emberSFX, transform.position);
        }
        if (move.Base.Name == "Thunder Shock")
        {
            AudioSource.PlayClipAtPoint(thunderShockSFX, transform.position);
        }
        if (move.Base.Name == "Tail Whip")
        {
            AudioSource.PlayClipAtPoint(tailWhipSFX, transform.position);
        }
        if (move.Base.Name == "Water Gun")
        {
            AudioSource.PlayClipAtPoint(waterGunSFX, transform.position);
        }
        if (move.Base.Name == "Withdraw")
        {
            AudioSource.PlayClipAtPoint(withdrawSFX, transform.position);
        }

        enemyPokemon.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        playerPokemon.PlayHitAnimation();

        if (move.Base.Category == MoveCategory.Status)
        {
            var effects = move.Base.Effects;
            if (effects.Boosts != null)
            {
                if (move.Base.Target == MoveTarget.Self)
                {
                    enemyPokemon.pokemon.ApplyBoosts(move.Base.Effects.Boosts);
                }
                else
                {
                    playerPokemon.pokemon.ApplyBoosts(move.Base.Effects.Boosts);
                }

                yield return ShowStatusChanges(enemyPokemon.pokemon);
                yield return ShowStatusChanges(playerPokemon.pokemon);
            }
        }

        var damageDetails = playerPokemon.pokemon.TakeDamage(move, enemyPokemon.pokemon);
        yield return playerHUD.UpdateHealth();
        if (damageDetails.typeEffectiveness > 1f)
        {
            AudioSource.PlayClipAtPoint(superEffectiveHitSFX, transform.position);
        }
        if (damageDetails.typeEffectiveness < 1f)
        {
            AudioSource.PlayClipAtPoint(ineffectiveHitSFX, transform.position);
        }
        else if (damageDetails.typeEffectiveness == 1)
        {
            AudioSource.PlayClipAtPoint(normalHitSFX, transform.position);
        }
        yield return ShowDamageDetails(damageDetails);
        yield return playerHealthText.UpdateHealthText(playerPokemon.pokemon);

        if (playerPokemon.pokemon.currentHealth <= 0)
        {
            yield return dialogue.TypeDialogue($"{playerPokemon.pokemon.baseStats.Name} fainted");
            playerPokemon.PlayFaintAnimation();
            AudioSource.PlayClipAtPoint(pokemonFaintSFX, transform.position);

            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
            state = BattleState.Complete;

            yield return new WaitForSeconds(1f);
            yield return dialogue.TypeDialogue("Want to continue playing? \nPress Y for a rematch and N to quit the game.");
        }
        else
        {
            ActionSelection();
        }
    }

    IEnumerator ShowStatusChanges(Pokemon pokemon)
    {
        while (pokemon.StatusChanges.Count > 0)
        {
            var message = pokemon.StatusChanges.Dequeue();
            yield return dialogue.TypeDialogue(message);
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.critical > 1f)
        {
            yield return dialogue.TypeDialogue("A critical hit!");
        }
        if (damageDetails.typeEffectiveness > 1f)
        {
            yield return dialogue.TypeDialogue("It's super effective!");
        }
        else if (damageDetails.typeEffectiveness < 1f)
        {
            yield return dialogue.TypeDialogue("It's not very effective...");
        }
    }

    private void Update()
    {
        if (state == BattleState.ActionSelection)
        {
            StartCoroutine(HandleActionSelection());
        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.Complete)
        {
            if (loadingOptionRematch == false && loadingOptionNoRematch == false)
            {
                StartCoroutine(HandleRematch());
            }
        }
    }

    IEnumerator HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentAction;

            AudioSource.PlayClipAtPoint(arrowSelectSFX, transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentAction;

            AudioSource.PlayClipAtPoint(arrowSelectSFX, transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentAction += 2;

            AudioSource.PlayClipAtPoint(arrowSelectSFX, transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentAction -= 2;

            AudioSource.PlayClipAtPoint(arrowSelectSFX, transform.position);
        }

        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogue.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // fight
            if (currentAction == 0)
            {
                MoveSelection();
            }
            // bag
            else if (currentAction == 1 && actionInProgress == false)
            {
                StartCoroutine(PlayerBag());

                actionInProgress = true;

                yield return new WaitForSeconds(2f);

                actionInProgress = false;
            }
            // pokemon
            if (currentAction == 2 && actionInProgress == false)
            {
                StartCoroutine(PlayerPokemon());

                actionInProgress = true;

                yield return new WaitForSeconds(2f);

                actionInProgress = false;
            }
            // run
            if (currentAction == 3 && actionInProgress == false)
            {
                StartCoroutine(PlayerRun());

                actionInProgress = true;

                yield return new WaitForSeconds(2f);

                actionInProgress = false;
            }

            AudioSource.PlayClipAtPoint(arrowSelectSFX, transform.position);
        }

        yield return new WaitForSeconds(1f);
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentMove;

            AudioSource.PlayClipAtPoint(arrowSelectSFX, transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentMove;

            AudioSource.PlayClipAtPoint(arrowSelectSFX, transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMove += 2;

            AudioSource.PlayClipAtPoint(arrowSelectSFX, transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMove -= 2;

            AudioSource.PlayClipAtPoint(arrowSelectSFX, transform.position);
        }

        currentMove = Mathf.Clamp(currentMove, 0, playerPokemon.pokemon.Moves.Count - 1);

        dialogue.UpdateMoveSelection(currentMove, playerPokemon.pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioSource.PlayClipAtPoint(arrowSelectSFX, transform.position);

            dialogue.EnableMoveSelector(false);
            dialogueBox.SetActive(true);
            dialogue.EnableDialogueText(true);
            StartCoroutine(PlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            dialogue.EnableMoveSelector(false);
            dialogueBox.SetActive(true);
            dialogue.EnableDialogueText(true);
            ActionSelection();
        }
    }

    IEnumerator HandleRematch()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            loadingOptionRematch = true;

            yield return new WaitForSeconds(1f);

            gameController.StartBattle();

            yield return new WaitForSeconds(3f);

            loadingOptionRematch = false;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            loadingOptionNoRematch = true;

            yield return new WaitForSeconds(1f);

            yield return dialogue.TypeDialogue("Quitting the game...");

            yield return new WaitForSeconds(1f);

            loadingOptionNoRematch = false;

            Debug.Log("Exit game.");
            Application.Quit();
        }
    }
}