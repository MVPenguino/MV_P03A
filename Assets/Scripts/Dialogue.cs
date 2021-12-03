using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Color highlightColor;

    [SerializeField] Text dialogueText;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveInfo;

    [SerializeField] List<Text> actionText;
    [SerializeField] List<Text> moveText;
    [SerializeField] List<Image> actionArrow;
    [SerializeField] List<Image> moveArrow;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    public void SetDialogue(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    public IEnumerator TypeDialogue(string dialogue)
    {
        dialogueText.text = "";
        foreach (var letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
    }

    public void EnableDialogueText(bool enabled)
    {
        dialogueText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveInfo.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionText.Count; ++i)
        {
            if (i == selectedAction)
            {
                actionText[i].color = highlightColor;
                actionArrow[i].enabled = true;
            }
            else
            {
                actionText[i].color = new Color32(61, 60, 58, 255);
                actionArrow[i].enabled = false;
            }
        }
    }

    public void UpdateMoveSelection(int selectedMove, Moves move)
    {
        for (int i = 0; i < moveText.Count; ++i)
        {
            if (i == selectedMove)
            {
                moveText[i].color = highlightColor;
                moveArrow[i].enabled = true;
            }
            else
            {
                moveText[i].color = new Color32(61, 60, 58, 255);
                moveArrow[i].enabled = false;
            }
        }

        ppText.text = $"{move.PP} / {move.Base.PP}";
        typeText.text = "Type/ " + move.Base.Type.ToString();
    }

    public void SetMoveNames(List <Moves> moves)
    {
        for (int i = 0; i < moveText.Count; ++i)
        {
            if (i < moves.Count)
            {
                moveText[i].text = moves[i].Base.Name;
            }
            else
            {
                moveText[i].text = "-";
            }
        }
    }
}