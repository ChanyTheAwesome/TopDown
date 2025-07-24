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
        _playerResourceController.RemoveHealthChangeEvent(uiManager.ChangePlayerHP);//���⼭ ResourceController�� �־��� ��������Ʈ�� �̺�Ʈ�� �־��ش�. Ȥ�� �𸣴� �ϴ� ��������
        _playerResourceController.AddHealthChangeEvent(uiManager.ChangePlayerHP);//�־��ִ� ���̴�.
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
        StartNextWave();//���̺긦 �����Ѵ�.
    }
    void StartNextWave()
    {
        currentWaveIndex += 1;//���̺��� ���� 1 �÷��ְ�
        enemyManager.StartWave(1 + currentWaveIndex / 5);//5���帶�� 1������ �� ����� �Ѵ�.
        uiManager.ChangeWave(currentWaveIndex);//���̺� ������ ����� �����Ѵ�.
    }
    public void EndOfWave()
    {
        StartNextWave();//���� ���̺� �ּ���~
    }

    public void GameOver()
    {
        enemyManager.StopWave();//���̺긦 ���߰�
        uiManager.SetGameOver();//UI��� ���� GameOver�� �����Ѵ�.
    }
}
