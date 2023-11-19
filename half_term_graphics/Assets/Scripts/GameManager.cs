/// <summary>
/// The GameManager class manages the game's state, including player and enemy actions,
/// tracking bullets, and handling the start and end of levels and the game.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int defeatedMinions = 0;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI playerBulletCountText;
    public TextMeshProUGUI minionBulletCountText;
    public TextMeshProUGUI bossBulletCountText;
    public TextMeshProUGUI totalBulletCountText;
    public TextMeshProUGUI minionsLeft;
    public TextMeshProUGUI bossLeft;
    public Image shootingPoint;
    public GameObject bossPrefab;
    public GameObject ground;
    public GameObject bossHealthBar;
    public bool playerDead = false;
    public bool minionLevelReady = false;
    public bool bossDead = false;
    public float bossScaleTime = 1f;
    public bool bossLevelReady = false;

    private bool bossLevelStarted = false;
    private int totalMinions;
    private Spawner spawner;
    private int playerBulletCount = 0;
    private int minionBulletCount = 0;
    private int bossBulletCount = 0;

    /// <summary>
    /// Start is called before the first frame update. It initializes game state and prepares
    /// the game for the first level.
    /// </summary>
    private void Start()
    {
        playerDead = false;
        bossDead = false;
        bossLevelReady = false;
        defeatedMinions = 0;
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();

        totalMinions = spawner.minionCount;
        shootingPoint.enabled = false;
        bossHealthBar.SetActive(false);

        StartCoroutine(StartGame());
    }

    /// <summary>
    /// Update is called once per frame and is responsible for updating game state.
    /// </summary>
    private void Update()
    {
        countEnemies();
        countBullets();
    }

    /// <summary>
    /// Counts the enemies currently active in the game and updates the UI.
    /// </summary>
    private void countEnemies()
    {
        GameObject[] minions = GameObject.FindGameObjectsWithTag("Minion");
        minionsLeft.text = "Minions Left: " + minions.Length;

        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        bossLeft.text = "Bosses Left: " + bosses.Length;
    }

    /// <summary>
    /// Counts the bullets currently active in the game and updates the UI.
    /// </summary>
    private void countBullets()
    {
        playerBulletCount = GameObject.FindGameObjectsWithTag("PlayerBullet").Length;
        minionBulletCount = GameObject.FindGameObjectsWithTag("MinionBullet").Length;
        bossBulletCount = GameObject.FindGameObjectsWithTag("BossBullet").Length;

        int totalBullets = playerBulletCount + minionBulletCount + bossBulletCount;

        playerBulletCountText.text = "Player Bullets: " + playerBulletCount;
        minionBulletCountText.text = "Minion Bullets: " + minionBulletCount;
        bossBulletCountText.text = "Boss Bullets: " + bossBulletCount;
        totalBulletCountText.text = "Total Bullets: " + totalBullets;
    }

    /// <summary>
    /// Coroutine to start the game. Displays the starting level text and enables the shooting point.
    /// </summary>
    IEnumerator StartGame()
    {
        levelText.text = "LEVEL 1";
        yield return new WaitForSeconds(spawner.minionScaleTime);
        levelText.text = "";
        shootingPoint.enabled = true;
    }

    /// <summary>
    /// Called when a minion is defeated. Checks if it's time to start the boss level.
    /// </summary>
    public void MinionDefeated()
    {
        defeatedMinions++;
        if (defeatedMinions >= totalMinions && !bossLevelStarted)
        {
            bossLevelStarted = true;
            StartCoroutine(StartBossLevel());
        }
    }

    /// <summary>
    /// Coroutine to start the boss level. Displays the boss level text and initiates the boss spawn.
    /// </summary>
    IEnumerator StartBossLevel()
    {
        bossHealthBar.SetActive(true);
        levelText.text = "Boss Level";
        yield return new WaitForSeconds(5);
        levelText.text = "";

        SpawnBoss();
    }

    /// <summary>
    /// Spawns the boss on the game field and scales it over time.
    /// </summary>
    void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab, ground.transform.position, Quaternion.identity);
        boss.transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(ScaleOverTime(boss, 0.3f, bossScaleTime));
        StartCoroutine(ShowStartText());
    }

    /// <summary>
    /// Coroutine to display the start text for the boss level.
    /// </summary>
    IEnumerator ShowStartText()
    {
        yield return new WaitForSeconds(1);
        levelText.text = "START";
        yield return new WaitForSeconds(1);
        levelText.text = "";
        bossLevelReady = true;
    }

    /// <summary>
    /// Scales an object over time to a target scale.
    /// </summary>
    /// <param name="obj">The object to scale.</param>
    /// <param name="targetScale">The target scale size.</param>
    /// <param name="duration">Duration over which to perform the scaling.</param>
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

    /// <summary>
    /// Called when the player is defeated. Sets the game state to player dead and ends the game.
    /// </summary>
    public void PlayerDefeated()
    {
        playerDead = true;
        levelText.text = "YOU LOSE!";
        StartCoroutine(FinishGame());
    }

    /// <summary>
    /// Called when the boss is defeated. Sets the game state to boss dead and ends the game.
    /// </summary>
    public void BossDefeated()
    {
        bossDead = true;
        levelText.text = "YOU WIN!";
        StartCoroutine(FinishGame());
    }

    /// <summary>
    /// Coroutine to finish the game. Waits for a duration before stopping the game.
    /// </summary>
    IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(10);
        EditorApplication.isPlaying = false;
    }
}
