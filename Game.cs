using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    [SerializeField] private Animator _playerTakeDamageAnimator;
    [SerializeField] private GameScenario _tutorScenario;
    [SerializeField] private GameObject _underGroundLvl;
    public UIStartNewGame UIStartNewGame;
    private GameTile tile;
    [SerializeField] private GameObject _tutorLvl;
    public GameBoard board;
    [SerializeField] private GameObject[] _maps;

    [SerializeField] private Vector2Int size;

    [SerializeField] private Camera camera;

    [SerializeField] private GameTileContentFactory contentFactory;
    [SerializeField] private WarFactory warFactory;

    private GameScenario scenario;

    private const float prepareTime = 10;

    private bool scenarioInProcess;

    private GameScenario.State activeScenario;

    private Ray TouchRay => camera.ScreenPointToRay(Input.mousePosition);

    private GameBehaviourCollections enemies = new GameBehaviourCollections();
    private GameBehaviourCollections nonEnemies = new GameBehaviourCollections();
    public List<GameObject> boosters = new List<GameObject>();
    public static bool Pause = false;
    public static bool FirstPass = true;

    [SerializeField] private EnemyFactory EnemyFactory;

    public bool isGameTime = false;
    public bool pressedStartGameBtn = false;
    private bool playerSelectLevel = false;

    private float gameStartTimer = 0;
    public int currentWave = 0;

    [SerializeField] private UnityEngine.UI.Image _selectedTile;
    [SerializeField] public GameOver gameOver;

    public byte currentLevel = 0;
    public bool isUnderGround = false;

    public LevelButtons[] levels; // Available levels

    [SerializeField] private ParticleSystem _explosionEffect;

    private float[] _collectorCoinsPerSecondLevel;

    private float[] _pistolTargetingRangeLevel;
    private float[] _pistolShootsPerSecondLevel;
    private float[] _pistolDamageLevel;

    private float[] _laserTargetingRangeLevel;
    private float[] _laserShootsPerSecondLevel;
    private float[] _laserDamageLevel;

    private float[] _mortarTargetingRangeLevel;
    private float[] _mortarShootsPerSecondLevel;
    private float[] _mortarDamageLevel;

    private float[] _hammerTargetingRangeLevel;
    private float[] _hammerShootsPerSecondLevel;
    private float[] _hammerDamageLevel;

    private Action _createLevel;

    private const int TimerUpgradeValue = 99;
    private const int TargetingRangeUpgradeValue = 13;
    private const float DamageUpgradeValue = 1.15f;

    public int[] towerSalePrice;
    private int[] towerImprovePrice;

    public readonly int towerBuildPrice = 25;
    private void OnEnable()
    {
        Instance = this;
    }
    private void EnableRandomMap()
    {
        if (_maps.Length <= 0) return;
        int id = UnityEngine.Random.Range(0, _maps.Length);
        for(int i = 0; i < _maps.Length; i++)
        {
            _maps[i].SetActive(false);
        }
        _maps[id].SetActive(true);
    }
    private void ClearBoosters()
    {
        foreach(GameObject el in boosters)
        {
            Destroy(el);
        }
        boosters.Clear();
    }
    private void Awake()
    {
        board.InitMap(size, contentFactory);

        towerSalePrice = new int[5] { 12, 17, 26, 41, 72 };
        towerImprovePrice = new int[4] { 35, 52, 83, 142 };

        _collectorCoinsPerSecondLevel = new float[5] { 0.4f, 1f, 1.8f, 2.5f, 3.5f };

        _pistolTargetingRangeLevel = new float[5] { 1.6f, 1.62f, 1.65f, 1.69f, 1.75f };
        _pistolShootsPerSecondLevel = new float[5] { 2f, 3f, 4f, 4.5f, 5f };
        _pistolDamageLevel = new float[5] { 2.25f, 3.31f, 4.25f, 5f, 6f };

        _laserTargetingRangeLevel = new float[5] { 1.4f, 1.43f, 1.47f, 1.52f, 1.6f };
        _laserShootsPerSecondLevel = new float[5] { 0.49f, 0.56f, 0.67f, 0.72f, 0.79f };
        _laserDamageLevel = new float[5] { 21f, 37f, 71f, 94f, 119 };

        _mortarTargetingRangeLevel = new float[5] { 3f, 3.3f, 3.6f, 4f, 4.5f };
        _mortarShootsPerSecondLevel = new float[5] { 0.25f, 0.25f, 0.25f, 0.25f, 0.25f };
        _mortarDamageLevel = new float[5] { 14f, 24f, 49f, 65f, 75f };

        _hammerTargetingRangeLevel = new float[5] { 1.48f, 1.485f, 1.49f, 1.495f, 1.5f };
        _hammerShootsPerSecondLevel = new float[5] { 0.2f, 0.2f, 0.2f, 0.2f, 0.21f };
        _hammerDamageLevel = new float[5] { 23f, 46f, 91f, 98f, 109f };

        FirstPass = PlayerPrefs.GetInt(ProfileKeys.FirstPassForTutor, -1) == 1 ? false : true;
        _tutorLvl.SetActive(false);
    }
    private void Start()
    {
        OpenNextLevel();
    }
    public void GamePause(bool pause)
    {
        Pause = pause;
    }
    public int GetTowerUpgradePrice(int currentTowerLevel, bool isPriceText = false)
    {
        int forPriceText = (isPriceText && currentTowerLevel == 4) ? 1 : 0;
        int price = towerImprovePrice[currentTowerLevel - forPriceText];
        return price;
    }
    public float GetCollectorUpgradeUpgrade(Tower tower)
    {
        float upgrade = _collectorCoinsPerSecondLevel[tower.currentTowerLevel];

        return upgrade;
    }
    /// <summary>
    /// 1) Shots per second /// 2) Targetting range /// 3) Tower damage
    /// </summary>
    /// <param name="tower"></param>
    /// <returns></returns>
    public float[] GetTowerUpgrades(TowerType tower, int currentTowerLevel, bool isText = false)
    {
        float[] upgrades = new float[3];
        float towerLvl = ProfileAssistant.main.userProfile.TowerStages[tower] != 0 ? ProfileAssistant.main.userProfile.TowerStages[tower] + 1 : 0;

        towerLvl = isText == true ? towerLvl + 1 : towerLvl;

        if (tower == TowerType.Pistol)
        {
            upgrades[0] = (float)Math.Round(_pistolShootsPerSecondLevel[currentTowerLevel] + ((towerLvl) / TimerUpgradeValue), 2);
            upgrades[1] = (float)Math.Round(_pistolTargetingRangeLevel[currentTowerLevel] + ((towerLvl) / TargetingRangeUpgradeValue), 2);
            upgrades[2] = (float)Math.Round(_pistolDamageLevel[currentTowerLevel] + (towerLvl / 10) , 2);
        }
        else if (tower == TowerType.Laser)
        {
            upgrades[0] = (float)Math.Round(_laserShootsPerSecondLevel[currentTowerLevel] + ((towerLvl) / TimerUpgradeValue), 2);
            upgrades[1] = (float)Math.Round(_laserTargetingRangeLevel[currentTowerLevel] + ((towerLvl) / TargetingRangeUpgradeValue), 2);
            upgrades[2] = (float)Math.Round(_laserDamageLevel[currentTowerLevel] + (towerLvl * DamageUpgradeValue), 2);
        }
        else if (tower == TowerType.Mortar)
        {
            upgrades[0] = (float)Math.Round(_mortarShootsPerSecondLevel[currentTowerLevel] + ((towerLvl) / TimerUpgradeValue), 2);
            upgrades[1] = (float)Math.Round(_mortarTargetingRangeLevel[currentTowerLevel] + ((towerLvl) / TargetingRangeUpgradeValue), 2);
            upgrades[2] = (float)Math.Round(_mortarDamageLevel[currentTowerLevel] + (towerLvl * DamageUpgradeValue), 2);
        }
        else if (tower == TowerType.Hammer)
        {
            upgrades[0] = (float)Math.Round(_hammerShootsPerSecondLevel[currentTowerLevel] + ((towerLvl) / TimerUpgradeValue), 2);
            upgrades[1] = (float)Math.Round(_hammerTargetingRangeLevel[currentTowerLevel] + ((towerLvl) / TargetingRangeUpgradeValue), 2);
            upgrades[2] = (float)Math.Round(_hammerDamageLevel[currentTowerLevel] + (towerLvl * DamageUpgradeValue), 2);
        }

        return upgrades;
    }

    public void ExplosionEffectMina(Vector3 position)
    {
        _explosionEffect.transform.position = new Vector3(position.x, position.y + 1, position.z);
        _explosionEffect.Play();
    }
    public void GoToMainMenuFromGameTime()
    {
        UI_SongsController.main.Play(SongType.EnableSong);
        if (scenarioInProcess)
        {
            ProfileAssistant.main.userProfile.MinusLife();
        }

        scenarioInProcess = false;
        isGameTime = false;
        pressedStartGameBtn = false;
        playerSelectLevel = false;
        CleaerGameObjects();
    }
    private void FinishGame(bool win)
    {
        scenarioInProcess = false;
        gameOver.FinishGame(win);
        BackGroundSongs.main.Stop();
        UI_SongsController.main.Play(win ? SongType.YouWin : SongType.YouLose, false);
        CleaerGameObjects();
        CheckLife();
    }
    private void OpenNextLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (ProfileAssistant.main.userProfile.currentAvailableLvl - 1 >= i)
            {
                levels[i]._levelButton.image.sprite = GameSettings.main.availableLvl;
                levels[i].lvlAvailable = true;
            }
            else
            {
                levels[i]._levelButton.image.sprite = GameSettings.main.lockLvl;
                levels[i].lvlAvailable = false;
            }
        }

        _underGroundLvl.SetActive(ProfileAssistant.main.userProfile.currentAvailableLvl >= 5);
        if (FirstPass != true)
        {
            if (ProfileAssistant.main.userProfile.LevelStages[currentLevel] >= 1 && ProfileAssistant.main.userProfile.currentAvailableLvl == currentLevel)
                ProfileAssistant.main.userProfile.currentAvailableLvl++;
            levels[ProfileAssistant.main.userProfile.currentAvailableLvl - 1]._levelButton.image.sprite = GameSettings.main.activeLvl;
        }
    }
    public void CheckLife()
    {
        if (ProfileAssistant.main.userProfile.GameResources[ResourcesType.Life] <= 0)
            Boosts.main.addLifeButton.gameObject.SetActive(true);
        else
            Boosts.main.addLifeButton.gameObject.SetActive(false);
    }
    public void StartGame(byte levelId, GameScenario scenario, Action createLevel)
    {
        Pause = false;
        if (ProfileAssistant.main.userProfile.GameResources[ResourcesType.Life] > 0)
        {
            if (scenario == null) scenario = _tutorScenario;
            ProfileAssistant.main.killedEnemyCount = 0;
            EnableRandomMap();
            UIStartNewGame.SetActivePanel(true);
            currentLevel = levelId;
            this.scenario = scenario;
            _createLevel = createLevel;

            isGameTime = false;
            pressedStartGameBtn = true;
            playerSelectLevel = true;

            BeginNewGame(createLevel);

            GameTimeUI.main.CoinsPerSeconde = 0;
        }
        else
        {
            Boosts.main.shopPopup.ShowBuyBoostPanel(ResourcesType.Life);
        }
    }
    public void PlayAgainCurrentLevel()
    {
        if (ProfileAssistant.main.userProfile.GameResources[ResourcesType.Life] > 0)
        {
            isGameTime = false;
            pressedStartGameBtn = true;
            playerSelectLevel = true;

            BeginNewGame(_createLevel);
            GameTimeUI.main.CoinsPerSeconde = 0;
            GameTimeUI.main.DisableUpgradePanelAndEnableBuildTowerPanel();

            gameOver.DisabeleGameOverPanels();
        }
        else
        {
            Boosts.main.shopPopup.ShowBuyBoostPanel(ResourcesType.Life);
        }
    }
    private void Update()
    {
        if (Pause != true)
        {
            if (isGameTime == false) StartGameTimer();

            if (Input.GetMouseButtonDown(0))
            {
                SelectTileForTower();
            }

            if (GameTimeUI.main.gameObject.activeInHierarchy)
            {
                GameTimeUI.main.CoinsText(ProfileAssistant.main.currentCoins);
                GameTimeUI.main.EnemyWaveText(scenario.waveCount, (int)gameStartTimer);
            }

            if (isGameTime)
            {
                if (scenarioInProcess)
                {
                    if (ProfileAssistant.main.currentPlayerHelth <= 0)
                    {
                        playerSelectLevel = false;
                        ProfileAssistant.main.userProfile.MinusLife();
                        FinishGame(false);
                    }
                    if (!activeScenario.Progress() && enemies.IsEmpty && ProfileAssistant.main.currentPlayerHelth > 0)
                    {
                        playerSelectLevel = false;
                        activeScenario.Progress();
                        FinishGame(true);
                        OpenNextLevel();
                    }
                    if (Application.isEditor)
                    {
                        if (Input.GetKeyDown(KeyCode.F1))
                        {
                            playerSelectLevel = false;
                            activeScenario.Progress();
                            FinishGame(true);
                            OpenNextLevel();
                        }
                    }
                }
            }
            enemies.GameUpdate();
            Physics.SyncTransforms();
            board.GameUpdate();
            nonEnemies.GameUpdate();
        }
    }
    public void SpawnGidraChilds(EnemyType type, GameTile tile)
    {
        Enemy enemy = Instance.EnemyFactory.Get(type);
        enemy.Spawn(tile);
        Instance.enemies.Add(enemy);
    }
    public static void SpawnEnemy(EnemyFactory factory, EnemyType type)
    {
        try
        {
            GameTile spawnPoint = Instance.board.GetSpawnPoint(UnityEngine.Random.Range(0, Instance.board.spawnPointCount));
            Enemy enemy = factory.Get(type);
            enemy.Spawn(spawnPoint);
            Instance.enemies.Add(enemy);
        }
        catch 
        {
            Debug.Log("Îøèáêà");
        }
    }
    private void SelectTileForTower()
    {
        if (GameTimeUI.main)
        {
            GameTile tile = board.GetTile(TouchRay);
            if (tile != null)
            {
                if (tile.Content.type == GameTileContentType.TowerPlace)
                {
                    if (TutorialMaster.main) TutorialMaster.main.ShowWhatWeNeedToBuildAnimation();
                    UI_SongsController.main.Play(SongType.EnableSong);
                    _selectedTile.gameObject.SetActive(true);
                    _selectedTile.rectTransform.localScale = new Vector2(0.5f, 0.5f);
                    _selectedTile.transform.position = tile.transform.position;

                    Vector2 rectPos = _selectedTile.rectTransform.anchoredPosition;
                    float positionX = rectPos.x;
                    float positionY = rectPos.y;

                    _selectedTile.rectTransform.anchoredPosition3D = new Vector3(positionX, positionY + 65, 0);
                    GameTimeUI.main.DisableUpgradePanelAndEnableBuildTowerPanel();
                    this.tile = tile;
                }
                else if (tile.Content.type != GameTileContentType.Tower)
                {
                    GameTimeUI.main.DisableUpgradePanelAndEnableBuildTowerPanel();
                    _selectedTile.gameObject.SetActive(false);
                }
                else if (tile.Content.type == GameTileContentType.Tower)
                {
                    _selectedTile.gameObject.SetActive(false);
                }
            }
            else
            {
                _selectedTile.gameObject.SetActive(false);
            }
        }
    }
    public void DisableTowerPlace()
    {
        if (tile != null) tile = null;
    }
    public void BuildTower(TowerType towerType)
    {
        if (tile != null)
        {
            if (ProfileAssistant.main.currentCoins >= towerBuildPrice)
            {
                if (TutorialMaster.main) TutorialMaster.main.TowerWasBuild();
                UI_SongsController.main.Play(SongType.PurchaseSong);
                board.ToggleTower(tile, towerType);
                ProfileAssistant.main.currentCoins -= towerBuildPrice;
                _selectedTile.gameObject.SetActive(false);
                tile = null;
            }
            else
            {
                UI_SongsController.main.Play(SongType.NeedMoreGemsSong);
                _selectedTile.gameObject.SetActive(true);
            }
        }
    }
    public void DestroyTower(GameTile tower)
    {
        board.DestroyTower(tower);
    }

    public static Shell SpawnShell()
    {
        Shell shell = Instance.warFactory.Shell;
        Instance.nonEnemies.Add(shell);

        return shell;
    }

    public static Explosion SpawnExplosion()
    {
        Explosion explosion = Instance.warFactory.Explosion;
        Instance.nonEnemies.Add(explosion);

        return explosion;
    }

    public static Bullet SpawnBullet()
    {
        Bullet bullet = Instance.warFactory.Bullet;
        Instance.nonEnemies.Add(bullet);

        return bullet;
    }

    private void BeginNewGame(Action createLevel)
    {
        FirebaseEventTracker.main.LevelStart();
        GameTimeUI.main.DisableUpgradePanelAndEnableBuildTowerPanel(true);
        currentWave = 0;
        ProfileAssistant.main.userProfile.underGroundWavesCount = 0;
        scenarioInProcess = false;
        if (gameStartTimer != prepareTime)
        {
            gameStartTimer = prepareTime;
        }

        enemies.Clear();
        nonEnemies.Clear();
        ClearBoosters();
        board.Clear();

        ProfileAssistant.main.currentPlayerHelth = ProfileAssistant.main.StartingPlayerHealth;
        ProfileAssistant.main.currentCoins = ProfileAssistant.main.startCoins;
        SelectBoost.main.StartGameWithBoosts();

        createLevel.Invoke();
    }
    public void EnemyReachedDestination()
    {
        UI_SongsController.main.Play(SongType.PlayerTakeDamage);
        ProfileAssistant.main.currentPlayerHelth--;
        _playerTakeDamageAnimator.Play("show");
        CameraShake.Shake(1f, 1f, CameraShake.ShakeMode.OnlyX);
        CameraShake.Shake(1f, 1f, CameraShake.ShakeMode.OnlyY);

        GameTimeUI.main.PlayerHealthText(ProfileAssistant.main.currentPlayerHelth);
    }

    public void CleaerGameObjects()
    {
        enemies.Clear();
        nonEnemies.Clear();
        ClearBoosters();
        board.Clear();
    }
    private void StartGameTimer()
    {
        if (playerSelectLevel)
        {
            if (gameStartTimer <= 0)
            {
                activeScenario = scenario.Begin();
                scenarioInProcess = true;
                isGameTime = true;
            }
            else
            {
                if (gameStartTimer > 0)
                    gameStartTimer -= Time.deltaTime;
            }
        }
    }
}
