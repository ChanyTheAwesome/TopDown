using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController Player { get; private set; }

    private ResourceController _playerResourceController;

    [SerializeField] private int currentStageIndex = 0;
    [SerializeField] private int currentWaveIndex = 0;
    private EnemyManager enemyManager;

    private UIManager uiManager;
    public static bool isFirstLoading = true;

    private CameraShake cameraShake;
    private StageInstance currentStageInstance;
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

        cameraShake = FindObjectOfType<CameraShake>();
        MainCameraShake();
    }

    public void MainCameraShake()
    {
        cameraShake.ShakeCamera(1, 1, 1);
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
        //StartNextWave();//웨이브를 시작한다.

        LoadOrStartNewStage();
    }
    void StartNextWave()
    {
        currentWaveIndex += 1;//웨이브의 수를 1 늘려주고
        enemyManager.StartWave(1 + currentWaveIndex / 5);//5라운드마다 1마리씩 더 생기게 한다.
        uiManager.ChangeWave(currentWaveIndex);//웨이브 숫자의 출력을 변경한다.
    }
    public void EndOfWave()
    {
        //StartNextWave();//다음 웨이브 주세요~
        StartNextWaveInStage();
    }

    public void GameOver()
    {
        enemyManager.StopWave();
        uiManager.SetGameOver();
        StageSaveManager.ClearSavedStage();
    }
    private void LoadOrStartNewStage()
    {
        StageInstance savedInstance = StageSaveManager.LoadStageInstance();

        if (savedInstance != null)
        {
            currentStageInstance = savedInstance;
        }
        else
        {
            currentStageInstance = new StageInstance(0, 0);
        }

        StartStage(currentStageInstance);
    }

    public void StartStage(StageInstance stageInstance)
    {
        currentStageIndex = stageInstance.stageKey;
        currentWaveIndex = stageInstance.currentWave;

        StageInfo stageInfo = GetStageInfo(stageInstance.stageKey);

        if (stageInfo == null)
        {
            Debug.Log("스테이지 정보가 없습니다.");
            StageSaveManager.ClearSavedStage();
            currentStageInstance = null;
            return;
        }

        stageInstance.SetStageInfo(stageInfo);

        uiManager.ChangeWave(currentStageIndex + 1);
        enemyManager.StartStage(currentStageInstance);
        StageSaveManager.SaveStageInstance(currentStageInstance);
    }


    public void StartNextWaveInStage()
    {
        if (currentStageInstance.CheckEndOfWave())
        {
            currentStageInstance.currentWave++;
            StartStage(currentStageInstance);
        }
        else
        {
            CompleteStage();
        }
    }

    public void CompleteStage()
    {
        StageSaveManager.ClearSavedStage();

        if (currentStageInstance == null)
            return;

        currentStageInstance.stageKey += 1;
        currentStageInstance.currentWave = 0;

        StartStage(currentStageInstance);
    }

    private StageInfo GetStageInfo(int stageKey)
    {
        foreach(StageInfo stage in StageData.stages)
        {
            if (stage.stageKey == stageKey)
            {
                return stage;
            }
        }
        return null;
    }
}
