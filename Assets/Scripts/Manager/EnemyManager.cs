using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Coroutine waveRoutine;

    [SerializeField] private List<GameObject> enemyPrefabs;
    private Dictionary<string, GameObject> enemyPrefabDict;

    [SerializeField] List<Rect> spawnAreas;
    [SerializeField] private Color gizmoColor = new Color(1, 0, 0, 0.3f);
    private List<EnemyController> activeEnemies = new List<EnemyController>();

    private bool enemySpawnComplete;

    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1.0f;
    [SerializeField] private List<GameObject> itemPrefabs;
    GameManager gameManager;
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        enemyPrefabDict = new Dictionary<string, GameObject>();
        foreach (GameObject prefab in enemyPrefabs)
        {
            enemyPrefabDict[prefab.name] = prefab;
        }
    }
    public void StartWave(int waveCount)
    {
        if (waveCount <= 0)
        {
            gameManager.EndOfWave();//EndOfWave�� ���� ���̺��� ������ ȣ���Ѵ�.
            return;
        }
        if (waveRoutine != null)
        {
            StopCoroutine(waveRoutine);//�̹� ���̺� ���̶��, waveRoutine�� ��ϵ� �ڷ�ƾ�� �����Ѵ�.
        }
        waveRoutine = StartCoroutine(SpawnWave(waveCount));//waveRoutine�� SpawnWave(waveCount)��� �ڷ�ƾ�� ����Ѵ�.
    }

    public void StopWave()
    {
        StopAllCoroutines();//����!!!
    }

    private IEnumerator SpawnWave(int waveCount)
    {
        enemySpawnComplete = false;//�� ���������? �ƴϿ�.
        yield return new WaitForSeconds(timeBetweenWaves);//���̺� ������ �ð���ŭ ��ٸ���

        for (int i = 0; i < waveCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);//���� ������ �ð���ŭ ��ٸ� ����
            SpawnRandomEnemy();//�����Ѵ�.
        }
        enemySpawnComplete = true;//�� ���������? ��.
    }
    private void SpawnRandomEnemy(string prefabName = null)
    {
        if (enemyPrefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning("Somethings wrong...");
            return;
        }
        GameObject randomPrefab;

        if (prefabName == null)
        {
            randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];//�������� ������ ��������
        }
        else
        {
            randomPrefab = enemyPrefabDict[prefabName];
        }

        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];//����Ʈ�� ����ִ� ���ڸ� �������� ������ ����

        Vector2 randomPosition = new Vector2(Random.Range(randomArea.xMin, randomArea.xMax), Random.Range(randomArea.yMin, randomArea.yMax));//�� ���� �ȿ��� �� �������� ��ġ�� ������

        GameObject spawnEnemy = Instantiate(randomPrefab, new Vector3(randomPosition.x, randomPosition.y), Quaternion.identity);//��ȯ�Ѵ�.
        EnemyController enemyController = spawnEnemy.GetComponent<EnemyController>();
        enemyController.Init(this, gameManager.Player.transform);//���⼭ Init�� �÷��̾�� Ÿ���� ��Ī�Ѵ�.

        activeEnemies.Add(enemyController);//activeEnemies��� ����Ʈ�� enemyController�� �ִ´�.
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null)
        {
            return;
        }

        Gizmos.color = gizmoColor;

        foreach (var area in spawnAreas)
        {
            Vector3 center = new Vector3(area.x + area.width / 2, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, area.height);
            Gizmos.DrawCube(center, size);
        }
    }//��� ���� ���ϰ� ���� ��ġ �׷��ֱ�.

    public void RemoveEnemyOnDeath(EnemyController enemy)
    {
        activeEnemies.Remove(enemy);

        CreateRandomItem(enemy.transform.position);

        if (enemySpawnComplete && activeEnemies.Count == 0)
            gameManager.EndOfWave();
    }
    public void CreateRandomItem(Vector3 position)
    {
        GameObject item = Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Count)], position, Quaternion.identity);
    }
    public void StartStage(StageInstance stageInstance)
    {
        if (waveRoutine != null)
            StopCoroutine(waveRoutine);

        waveRoutine = StartCoroutine(SpawnStart(stageInstance));
    }

    private IEnumerator SpawnStart(StageInstance stageInstance)
    {
        enemySpawnComplete = false;
        yield return new WaitForSeconds(timeBetweenWaves);

        WaveData waveData = stageInstance.currentStageInfo.waves[stageInstance.currentWave];

        for (int i = 0; i < waveData.monsters.Length; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);

            MonsterSpawnData monsterSpawnData = waveData.monsters[i];
            for (int j = 0; j < monsterSpawnData.spawnCount; j++)
            {
                SpawnRandomEnemy(monsterSpawnData.monsterType);
            }
        }

        if (waveData.hasBoss)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);

            gameManager.MainCameraShake();
            SpawnRandomEnemy(waveData.bossType);
        }

        enemySpawnComplete = true;
    }
}
