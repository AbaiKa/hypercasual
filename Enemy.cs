using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : GameBehavior
{
    [SerializeField] protected GameObject model;
    [SerializeField] protected UnityEngine.UI.Image healtBar;
    [SerializeField] protected int rewardForDie = 3;
    public EnemyFactory originFactory { get; set; }

    protected GameTile tileFrom, tileTo;
    private Vector3 positionFrom, positionTo;
    private float progress, progressFactory;

    private Direction direction;
    private DirectionChange directionChange;
    private float directionAngleFrom, directionAngleTo;
    private float positionOffset;
    private float speed;
    private float startSpeed;

    public float Scale { get; private set; }
    public float Health { get; private set; }


    protected float startHealth;
    protected float onePercent;
    public abstract SuperEnemies enemySuperKnowlege { get; }

    [HideInInspector] public bool isShield = false;
    [HideInInspector] public bool isGidra = false;
    [HideInInspector] public bool isHealer = false;
    [HideInInspector] public bool isLeader = false;

    [SerializeField] private Animator animator;

    public void InitializeScale(float scale, float position, float speed, float health)
    {
        model.transform.localScale = new Vector3(scale, scale, scale);
        positionOffset = position;
        this.speed = speed;
        startSpeed = speed;
        Scale = scale;
        Health = health;
        startHealth = health;
        onePercent = startHealth / 100;
    }
    public void Spawn(GameTile tile)
    {
        transform.localPosition = tile.transform.localPosition;
        tileFrom = tile;
        tileTo = tile.NextTile;
        progress = 0;
        PrepareIntro();
    }

    private void PrepareIntro()
    {
        positionFrom = tileFrom.transform.localPosition;
        positionTo = tileFrom.ExitPoint;
        direction = tileFrom.PathDirection;
        directionChange = DirectionChange.None;
        directionAngleFrom = directionAngleTo = direction.GetAngle();
        transform.localRotation = direction.GetRotation();
        progressFactory = speed;
        model.transform.localPosition = new Vector3(positionOffset, 0);
    }
    /// <summary>
    /// 1 это 100%
    /// 0,5 это 50%
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        this.speed = startSpeed * speed;
        HandleNextState();
    }
    private float CalculateHealthBar(float currentHealth)
    {
        return (currentHealth / onePercent) / 100;
    }
    protected void HealthBarFillAmount(float health)
    {
        healtBar.fillAmount = CalculateHealthBar(health);
    }
    public override bool GameUpdate()
    {
        //if (Game.Pause != true)
        {
            if (Health <= 0)
            {
                DestroyEnemy();
                return false;
            }

            healtBar.transform.rotation = Quaternion.identity;

            progress += Time.deltaTime * progressFactory;

            while (progress >= 1)
            {
                tileFrom = tileTo;
                tileTo = tileTo.NextTile;
                if (tileTo == null)
                {
                    Game.Instance.EnemyReachedDestination();
                    DestroyEnemy();
                    return false;
                }

                progress = (progress - 1) / progressFactory;
                PrepareNextState();
                progress *= progressFactory;
            }
            transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);

            if (directionChange != DirectionChange.None)
            {
                float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, progress);
                transform.localRotation = Quaternion.Euler(0, angle, 0);
            }
        }
        return true;
    }
    public void HealEnemyHealth(float healPlus)
    {
        if (Health < startHealth)
        {
            Health += healPlus;

            HealthBarFillAmount(Health);
        }
    }
    public virtual void TakeDamage(float damage)
    {
        EnemySongs.main.Play();
        Health -= damage;
        HealthBarFillAmount(Health);
    }
    public virtual void TakeDamageWithBonus(float damage)
    {
        Health -= damage;

        HealthBarFillAmount(Health);
    }
    private void PrepareNextState()
    {
        positionFrom = positionTo;
        positionTo = tileTo.ExitPoint;
        directionChange = direction.GetDirectionChange(tileFrom.PathDirection);
        direction = tileFrom.PathDirection;
        directionAngleFrom = directionAngleTo;

        HandleNextState();
    }
    private void HandleNextState()
    {
        switch (directionChange)
        {
            case DirectionChange.None: PrepareForward(); break;
            case DirectionChange.TurnRight: PrepareTurnRight(); break;
            case DirectionChange.TurnLeft: PrepareTurnLeft(); break;
            default: PrepareTurnAround(); break;
        }
    }
    private void PrepareForward()
    {
        transform.localRotation = direction.GetRotation();
        directionAngleTo = direction.GetAngle();
        model.transform.localPosition = new Vector3(positionOffset, 0);
        progressFactory = speed;
    }

    private void PrepareTurnRight()
    {
        directionAngleTo = directionAngleFrom + 90;
        model.transform.localPosition = new Vector3(positionOffset, 0);
        progressFactory = speed;
    }

    private void PrepareTurnLeft()
    {
        directionAngleTo = directionAngleFrom - 90;
        model.transform.localPosition = new Vector3(positionOffset, 0);
        progressFactory = speed;
    }

    private void PrepareTurnAround()
    {
        directionAngleTo = directionAngleFrom + 180;
        model.transform.localPosition = new Vector3(positionOffset, 0);
        progressFactory = speed;
    }
    public virtual void DestroyEnemy()
    {
        ProfileAssistant.main.currentCoins += rewardForDie;
        ProfileAssistant.main.userProfile.bossWinsCount++;
        GoldNotificationAssistant.main.ShowGoldNotification(model.transform.position, rewardForDie);
        Recycle();
    }
    public override void Recycle()
    {
        ProfileAssistant.main.killedEnemyCount++;
        if (Health <= 0)
        {
            animator.Play("Die");
        }
        else
        {
            RecycleEnemyAfterAnimation();
        }
    }

    public void RecycleEnemyAfterAnimation()
    {
        StartCoroutine(SizeDownAndTurnOff());
    }

    private IEnumerator SizeDownAndTurnOff()
    {
        float enemySize = gameObject.transform.localScale.x;
        for (float size = enemySize; size > 0f; size -= enemySize / 10)
        {
            gameObject.transform.localScale = new Vector3(size, size, size);
            yield return new WaitForSeconds(.02f);
        }

        originFactory.Reclaim(this);
    }
}

public enum SuperEnemies
{
    Usuall,
    Gidra,
    Shield,
    Healer,
    Leader,
    Bird,
    Empty,
    Fat
}