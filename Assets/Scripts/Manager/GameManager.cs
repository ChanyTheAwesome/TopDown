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
        _playerResourceController.RemoveHealthChangeEvent(uiManager.ChangePlayerHP);//���⼭ ResourceController�� �־��� ��������Ʈ�� �̺�Ʈ�� �־��ش�. Ȥ�� �𸣴� �ϴ� ��������
        _playerResourceController.AddHealthChangeEvent(uiManager.ChangePlayerHP);//�־��ִ� ���̴�.

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
            StartGame();//�ƴ϶�� ���� ���� �����Ѵ�.
        }
        else
        {
            isFirstLoading = false;//ù �ε��̶�� �׳� �Ұ� �� �ϰ�
        }
        //����۽� HomeUI�� �ѹ� �� �����°� �����ϴ� ���̴�.
    }
    public void StartGame()
    {
        uiManager.SetPlayGame();//UI��� ���� gameȭ������ �����ϰ�
        //StartNextWave();//���̺긦 �����Ѵ�.

        LoadOrStartNewStage();
    }
    void StartNextWave()
    {
        currentWaveIndex += 1;//���̺��� ���� 1 �÷��ְ�
        enemyManager.StartWave(1 + currentWaveIndex / 5);//5���帶�� 1������ �� ����� �Ѵ�.
        uiManager.ChangeWave(currentWaveIndex);//���̺� ������ ����� �����Ѵ�.
    }
    public void EndOfWave()
    {
        //StartNextWave();//���� ���̺� �ּ���~
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
            Debug.Log("�������� ������ �����ϴ�.");
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
