using UnityEngine;
using UnityEngine.Video;

public class UIScreenActivator : MonoBehaviour
{
    public enum ActionType { StartGame, NextLevel, PlayVideo, CustomEvent }

    public ActionType action = ActionType.StartGame;
    public GameManager gameManager; // optional: assign if you want direct calls
    public string levelName = "Level2"; // used for NextLevel
    public VideoPlayer videoPlayer; // optional: play video when activated

    // Public method to call when the screen is activated by remote/raycast
    public void Activate()
    {
        Debug.Log("UIScreenActivator activated: " + gameObject.name + " action=" + action);

        switch (action)
        {
            case ActionType.StartGame:
                if (gameManager != null)
                {
                    gameManager.TekanStartGame();
                }
                else
                {
                    GameEvents.RaiseStartGame();
                }
                break;
            case ActionType.NextLevel:
                if (gameManager != null)
                {
                    // hide any UI and call PindahKeLevel2 or load levelName
                    gameManager.PindahKeLevel2();
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
                }
                break;
            case ActionType.PlayVideo:
                if (videoPlayer != null)
                {
                    videoPlayer.Play();
                }
                break;
            case ActionType.CustomEvent:
                // raise a general tutorial completed event as an example
                GameEvents.RaiseTutorialCompleted();
                break;
        }
    }
}
