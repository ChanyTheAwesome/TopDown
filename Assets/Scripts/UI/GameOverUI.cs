using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : BaseUI
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        restartButton.onClick.AddListener(OnClickReStartButton);
        exitButton.onClick.AddListener(OnClickExitButton);//버튼을 설정한다.
    }

    public void OnClickReStartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//ActiveScene의 인덱스를 가져와 그걸 호출한다.
        //지금은 하나밖에 없으니, SampleScene을 다시 부르는 것과 동일하다.
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }

    protected override UIState GetUIState()
    {
        return UIState.GameOver;
    }
}