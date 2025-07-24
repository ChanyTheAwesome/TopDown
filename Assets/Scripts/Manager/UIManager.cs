using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum UIState
{
    Home,
    Game,
    GameOver
}
public class UIManager : MonoBehaviour
{
    HomeUI homeUI;
    GameUI gameUI;
    GameOverUI gameOverUI;
    public UIState currentState;

    private void Awake()
    {
        homeUI = GetComponentInChildren<HomeUI>(true);
        homeUI.Init(this);
        gameUI = GetComponentInChildren<GameUI>(true);
        gameUI.Init(this);
        gameOverUI = GetComponentInChildren<GameOverUI>(true);
        gameOverUI.Init(this);

        ChangeState(UIState.Home);
    }

    public void SetPlayGame()
    {
        ChangeState(UIState.Game);
    }

    public void SetGameOver()
    {
        ChangeState(UIState.GameOver);
    }

    public void ChangeWave(int waveIndex)
    {
        gameUI.UpdateWaveText(waveIndex);
    }

    public void ChangePlayerHP(float currentHP, float maxHP)
    {
        gameUI.UpdateHPSlider(currentHP / maxHP);//HPSlider를 재조정한다. 플레이어의 HP가 변경될 때 마다 호출된다.(델리게이트로 작동함)
    }

    public void ChangeState(UIState state)//받아온 state로
    {
        currentState = state;
        homeUI.SetActive(currentState);
        gameUI.SetActive(currentState);
        gameOverUI.SetActive(currentState);
        //어떤 UI를 켤지 정한다.
    }
}
