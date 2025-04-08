using UnityEngine;

public class NPCANIMATOR_ChangePlayerState : MonoBehaviour
{
    public PlayerManager Player;

    private void PlayerState_Game()
    {
        Player._PlayerState = PlayerManager.PlayerState.Game;
    }

    private void PlayerState_Cutscene()
    {
        Player._PlayerState = PlayerManager.PlayerState.Cutscene;
    }
}
