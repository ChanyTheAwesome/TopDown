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

    protected override void Start()
    {
        base.Start();
        projectileManager = ProjectileManager.Instance;
    }

    public override void Attack()
    {
        base.Attack();//애니메이션 출력하고, 소리를 내는 것이다.
        float projectileAngleSpace = multipleProjectileAngle;
        int numberOfProjectilePerShot = NumberofProjectilesPerShot;
        float minAngle = -(numberOfProjectilePerShot/2.0f) * projectileAngleSpace;//최소 각도를 설정한다.

        for(int i = 0; i<numberOfProjectilePerShot; i++)
        {
            float angle = minAngle + projectileAngleSpace * i;//최소 각도에 정해진 각도 간격을 더하고
            float randomSpread = Random.Range(-spread, spread);//무작위 값을 또 정하고
            angle += randomSpread;//더해
            CreateProjectile(Controller.LookDirection, angle);//생성한다.
        }//이를 투사체 갯수만큼 반복한다.
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
