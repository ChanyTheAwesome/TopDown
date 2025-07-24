using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;
    private RangeWeaponHandler rangeWeaponHandler;

    private float currentDuration;
    private Vector2 direction;
    private bool isReady;
    private Transform pivot;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteRenderer;

    public bool fxOnDestroy = true;

    ProjectileManager projectileManager;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0);
    }

    private void Update()
    {
        if (!isReady) return;//준비되지 않으면 update를 돌지 않는다.

        currentDuration += Time.deltaTime;

        if (currentDuration > rangeWeaponHandler.Duration)
        {
            DestroyProjectile(transform.position, false);//최대 사거리 느낌. 최대 사거리가 넘어가면 파괴한다.
        }
        _rigidbody.velocity = direction * rangeWeaponHandler.Speed;//그렇지 않다면 계속 정해진 방향과 정해진 속도로 나아간다.
    }

    private void OnTriggerEnter2D(Collider2D collision)//뭔가 맞았다!
    {
        if(levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))//Level레이어는 Level-Grid-Collision에 붙어있다.
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * 0.2f, fxOnDestroy);
        }
        else if(rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer)))
        {//타겟과 같다면,
            ResourceController resourceController = collision.GetComponent<ResourceController>();
            if(resourceController != null)
            {
                resourceController.ChangeHealth(-rangeWeaponHandler.Power);//무기의 파워만큼 체력을 깎고
                if (rangeWeaponHandler.IsOnKnockback)//넉백이 되는 무기라면
                {
                    BaseController controller = collision.GetComponent<BaseController>();
                    if(controller != null)
                    {
                        controller.ApplyKnockback(transform, rangeWeaponHandler.KnockbackPower, rangeWeaponHandler.KnockbackTime);//넉백을 준다.
                    }
                }
            }
            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);//이후 투사체 파괴
        }
    }

    public void Init(Vector2 direction, RangeWeaponHandler weaponHandler, ProjectileManager projectileManager)
    {//이 Init은 ProjectileManager에서 총알을 쐈을 때 호출된다.
        this.projectileManager = projectileManager;
        rangeWeaponHandler = weaponHandler;
        this.direction = direction;
        currentDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        spriteRenderer.color = weaponHandler.ProjectileColor;

        transform.right = this.direction;

        if (direction.x < 0)
        {
            pivot.localRotation = Quaternion.Euler(180, 0, 0);//방향이 왼쪽이라면 180도 틀어주고
        }
        else
        {
            pivot.localRotation = Quaternion.Euler(0, 0, 0);//아니라면 그대로 둔다.
        }
        isReady = true;//준비 됐다.
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            projectileManager.CreateImpactParticlesAtPosition(position, rangeWeaponHandler);//projeectileManager의 파티클 생성 메서드로 보낸다.
        }
        Destroy(this.gameObject);
    }
}
