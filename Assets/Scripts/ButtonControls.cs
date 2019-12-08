using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumeGame()
    {
        GameManager.Instance.DeactivatePromotionMenu();
        GameManager.Instance.currentBoardStatus = GameManager.Status.FreeMove;
        GameManager.Instance.NextPlayer();
    }

    public void GetPiece(int piece)
    {
        GameManager.Instance.SwapPiece(piece);
        Debug.Log("Fired");
        ResumeGame();
    }
}
