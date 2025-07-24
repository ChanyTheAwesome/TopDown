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
        exitButton.onClick.AddListener(OnClickExitButton);//��ư�� �����Ѵ�.
    }

    public void OnClickReStartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//ActiveScene�� �ε����� ������ �װ� ȣ���Ѵ�.
        //������ �ϳ��ۿ� ������, SampleScene�� �ٽ� �θ��� �Ͱ� �����ϴ�.
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