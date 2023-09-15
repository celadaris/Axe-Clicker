using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseFilm;
    [SerializeField] GameObject pauseTxt;
    [SerializeField] GameObject menuBtn;
    [SerializeField] GameObject quitBtn;

    [SerializeField] Image pauseImg;
    [SerializeField] Sprite pausePic;
    [SerializeField] Sprite playPic;

    public static bool gamePaused { get; private set; }

    private void Start()
    {
        gamePaused = false;
        Time.timeScale = 1.0f;
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pauseImg.sprite = playPic;

        pauseFilm.SetActive(true);
        pauseTxt.SetActive(true);
        menuBtn.SetActive(true);
        quitBtn.SetActive(true);

        gamePaused = true;
    }

    private void UnPauseGame()
    {
        Time.timeScale = 1;
        pauseImg.sprite = pausePic;

        pauseFilm.SetActive(false);
        pauseTxt.SetActive(false);
        menuBtn.SetActive(false);
        quitBtn.SetActive(false);

        gamePaused = false;
    }

    public void PauseMenu()
    {
        if (!gamePaused)
        {
            PauseGame();
        }
        else
        {
            UnPauseGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }
}
