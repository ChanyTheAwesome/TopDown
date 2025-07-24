using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Coroutine waveRoutine;

    [SerializeField] private List<GameObject> enemyPrefabs;

    [SerializeField] List<Rect> spawnAreas;
    [SerializeField] private Color gizmoColor = new Color(1, 0, 0, 0.3f);
    private List<EnemyController> activeEnemies = new List<EnemyController>();

    private bool enemySpawnComplete;

    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1.0f;

    GameManager gameManager;
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
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
    private void SpawnRandomEnemy()
    {
        if (enemyPrefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning("Somethings wrong...");
            return;
        }

        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];//�������� ������ ��������
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
        activeEnemies.Remove(enemy);//���� ������ ����Ʈ���� ���ְ�
        if (enemySpawnComplete && activeEnemies.Count == 0)//�� ���׿���?
        {
            gameManager.EndOfWave();//���̺� �����ּ���~ == ���� ���̺� �������ּ���~
        }
    }
}
