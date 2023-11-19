/// <summary>
/// The Spawner class is responsible for spawning minions.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject minionPrefab;
    public float minionScale = 0.9f;
    public GameObject ground;
    public int minionCount = 3;
    public float minionScaleTime = 2f;
    private GameManager gameManager;

    /// <summary>
    /// Start is called before the first frame update. It spawns minions.
    /// </summary>
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SpawnMinions();
    }

    /// <summary>
    /// SpawnMinions is called when the game starts. It spawns minions.
    /// </summary>
    void SpawnMinions()
    {
        Vector3 groundSize = ground.GetComponent<Renderer>().bounds.size;
        Vector3 groundPosition = ground.transform.position;

        for (int i = 0; i < minionCount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(groundPosition.x - (groundSize.x - 10) / 2, groundPosition.x + (groundSize.x + 10) / 2),
                groundPosition.y,
                Random.Range(groundPosition.z - (groundSize.z - 10) / 2, groundPosition.z + (groundSize.z + 10) / 2)
            );

            GameObject minion = Instantiate(minionPrefab, randomPosition, Quaternion.identity);
            minion.transform.localScale = new Vector3(0, 0, 0);
            StartCoroutine(ScaleOverTime(minion, minionScale, minionScaleTime));
        }
        gameManager.minionLevelReady = true;
    }

    /// <summary>
    /// ScaleOverTime is a coroutine that scales a GameObject over time.
    /// </summary>
    IEnumerator ScaleOverTime(GameObject obj, float targetScale, float duration)
    {
        Vector3 originalScale = obj.transform.localScale;
        Vector3 targetScaleVec = new Vector3(targetScale, targetScale, targetScale);

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            obj.transform.localScale = Vector3.Lerp(originalScale, targetScaleVec, t);
            yield return null;
        }

        obj.transform.localScale = targetScaleVec;
    }
}
