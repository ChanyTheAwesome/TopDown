using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController Player { get; private set; }

    private ResourceController _playerResourceController;

    [SerializeField] private int currentWaveIndex = 0;
    private EnemyManager enemyManager;

    private UIManager uiManager;
    public static bool isFirstLoading = true;
    private void Awake()
    {
        Instance = this;

        Player = FindObjectOfType<PlayerController>();
        Player.Init(this);

        uiManager = FindObjectOfType<UIManager>();

        enemyManager = GetComponentInChildren<EnemyManager>();
        enemyManager.Init(this);

        _playerResourceController = Player.GetComponent<ResourceController>();
        _playerResourceController.RemoveHealthChangeEvent(uiManager.ChangePlayerHP);//여기서 ResourceController에 있었던 델리게이트에 이벤트를 넣어준다. 혹시 모르니 일단 빼본다음
        _playerResourceController.AddHealthChangeEvent(uiManager.ChangePlayerHP);//넣어주는 것이다.
    }

    private void Start()
    {
        if (!isFirstLoading)
        {
            StartGame();//아니라면 냅다 게임 시작한다.
        }
        else
        {
            isFirstLoading = false;//첫 로딩이라면 그냥 할거 다 하고
        }
        //재시작시 HomeUI가 한번 더 나오는걸 방지하는 것이다.
    }
    public void StartGame()
    {
        uiManager.SetPlayGame();//UI띄울 것을 game화면으로 설정하고
        StartNextWave();//웨이브를 시작한다.
    }
    void StartNextWave()
    {
        currentWaveIndex += 1;//웨이브의 수를 1 늘려주고
        enemyManager.StartWave(1 + currentWaveIndex / 5);//5라운드마다 1마리씩 더 생기게 한다.
        uiManager.ChangeWave(currentWaveIndex);//웨이브 숫자의 출력을 변경한다.
    }
    public void EndOfWave()
    {
        StartNextWave();//다음 웨이브 주세요~
    }

    public void GameOver()
    {
        enemyManager.StopWave();//웨이브를 멈추고
        uiManager.SetGameOver();//UI띄울 것을 GameOver로 설정한다.
    }
}
