using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public PlayerManager _PlayerManager;

    public Vector2 ReEnterPosition;

    public Vector2 ReEnterDirection;

    public bool IsPlayableLevel = true;

    private int PreviousLevelIndex;

    public INPUTPOSITION inputPOS;

    public CameraManager _CameraManager;

    private void Start()
    {
        if (PlayerPrefs.GetFloat("HasSceneTransition") == 0f)
        {
            LoadSceneTransitionPlayerPosition();
        }
        _CameraManager = Camera.main.gameObject.GetComponent<CameraManager>();
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void SaveIntValue(string IntName, int IntValue)
    {
        PlayerPrefs.SetInt(IntName, IntValue);
        PlayerPrefs.Save();
        Debug.Log("Game data saved! | " + IntName + " : " + IntValue);
    }

    public void SceneTransitionPlayerPosition(Vector3 PlayerPos)
    {
        PlayerPrefs.SetInt("VISITED" + SceneManager.GetActiveScene().name, 1);
        PlayerPrefs.SetInt("HasSceneTransition", 1);
        PlayerPrefs.SetInt("PreviousScene", SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadSceneTransitionPlayerPosition()
    {
        MonoBehaviour.print(PlayerPrefs.GetInt("PreviousScene"));
        if (PlayerPrefs.GetInt("PreviousScene") == inputPOS.SceneNameIndex[PreviousLevelIndex])
        {
            MonoBehaviour.print("Last Level was " + PlayerPrefs.GetInt("PreviousScene"));
            _PlayerManager.transform.position = inputPOS.StartPosition[PreviousLevelIndex];
            _PlayerManager._PMove.RotatePlayerAnim(inputPOS.StartDirection[PreviousLevelIndex]);
            _CameraManager.transform.position = inputPOS.StartCameraPosition[PreviousLevelIndex];
        }
        else if (PreviousLevelIndex != inputPOS.SceneNameIndex.Length)
        {
            PreviousLevelIndex++;
            LoadSceneTransitionPlayerPosition();
        }
        else
        {
            Debug.Log("No Scene Position!");
        }
    }

    private void LoadIntValue(string IntName, int IntValue)
    {
        if (PlayerPrefs.HasKey(IntName))
        {
            PlayerPrefs.GetInt(IntName);
            Debug.Log("Game data loaded!");
        }
        else
        {
            Debug.LogError(IntName + " Does not exist!");
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ResetData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
