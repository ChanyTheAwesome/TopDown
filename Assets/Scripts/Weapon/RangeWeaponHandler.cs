using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHandler : WeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int bulletIndex;

    public int BulletIndex { get { return bulletIndex; } }
    [SerializeField] private float bulletSize = 1.0f;
    public float BulletSize { get { return bulletSize; } }
    [SerializeField] private float duration;
    public float Duration { get { return duration; } }
    [SerializeField] private float spread;
    public float Spread { get { return spread; } }
    [SerializeField] private int numberofProjectilesPerShot;
    public int NumberofProjectilesPerShot { get { return numberofProjectilesPerShot; } }
    [SerializeField] private float multipleProjectileAngle;
    public float MultipleProjectileAngle { get { return multipleProjectileAngle; } }
    [SerializeField] private Color projectileColor;
    public Color ProjectileColor { get { return projectileColor; } }

    private ProjectileManager projectileManager;

    private StatHandler statHandler;


    protected override void Start()
    {
        base.Start();
        projectileManager = ProjectileManager.Instance;
        statHandler = GetComponentInParent<StatHandler>();
    }


    public override void Attack()
    {
        base.Attack();

        float projectileAngleSpace = multipleProjectileAngle;
        int numberOfProjectilePerShot = numberofProjectilesPerShot + (int)statHandler.GetStat(StatType.ProjectileCount); ;

        float minAlge = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace;

        for (int i = 0; i < numberOfProjectilePerShot; i++)
        {
            float angle = minAlge + projectileAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;
            CreateProjectile(Controller.LookDirection, angle);
        }
    }
    private void CreateProjectile(Vector2 _lookDirection, float angle)
    {//총알 발사, projectileManager에게 있는 ShootBullet을 호출한다.
        projectileManager.ShootBullet(this, projectileSpawnPosition.position, RotateVector2(_lookDirection, angle));
    }//보내주는 값은 자신과, spawnPosition, 보고있는 방향에서 angle만큼 값을 보정한 vector를 보낸다.
    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
