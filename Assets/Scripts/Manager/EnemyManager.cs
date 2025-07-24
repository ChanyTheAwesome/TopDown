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
            gameManager.EndOfWave();//EndOfWave는 다음 웨이브의 시작을 호출한다.
            return;
        }
        if (waveRoutine != null)
        {
            StopCoroutine(waveRoutine);//이미 웨이브 중이라면, waveRoutine에 등록된 코루틴을 중지한다.
        }
        waveRoutine = StartCoroutine(SpawnWave(waveCount));//waveRoutine에 SpawnWave(waveCount)라는 코루틴을 등록한다.
    }

    public void StopWave()
    {
        StopAllCoroutines();//멈춰!!!
    }

    private IEnumerator SpawnWave(int waveCount)
    {
        enemySpawnComplete = false;//다 만들었나요? 아니요.
        yield return new WaitForSeconds(timeBetweenWaves);//웨이브 사이의 시간만큼 기다리고

        for (int i = 0; i < waveCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);//스폰 사이의 시간만큼 기다린 다음
            SpawnRandomEnemy();//생성한다.
        }
        enemySpawnComplete = true;//다 만들었나요? 네.
    }
    private void SpawnRandomEnemy()
    {
        if (enemyPrefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning("Somethings wrong...");
            return;
        }

        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];//무작위로 선정된 프리팹을
        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];//리스트에 들어있는 상자를 무작위로 선정한 다음

        Vector2 randomPosition = new Vector2(Random.Range(randomArea.xMin, randomArea.xMax), Random.Range(randomArea.yMin, randomArea.yMax));//그 상자 안에서 또 무작위로 위치를 선정해

        GameObject spawnEnemy = Instantiate(randomPrefab, new Vector3(randomPosition.x, randomPosition.y), Quaternion.identity);//소환한다.
        EnemyController enemyController = spawnEnemy.GetComponent<EnemyController>();
        enemyController.Init(this, gameManager.Player.transform);//여기서 Init의 플레이어는 타겟을 지칭한다.

        activeEnemies.Add(enemyController);//activeEnemies라는 리스트에 enemyController를 넣는다.
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
    }//요건 보기 편하게 스폰 위치 그려주기.

    public void RemoveEnemyOnDeath(EnemyController enemy)
    {
        activeEnemies.Remove(enemy);//적이 죽으면 리스트에서 빼주고
        if (enemySpawnComplete && activeEnemies.Count == 0)//엥 다죽였네?
        {
            gameManager.EndOfWave();//웨이브 끝내주세요~ == 다음 웨이브 시작해주세요~
        }
    }
}
