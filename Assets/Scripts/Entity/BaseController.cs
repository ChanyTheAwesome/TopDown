using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;

    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection {  get { return movementDirection; }}

    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; }}

    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    protected AnimationHandler animationhandler;

    protected StatHandler statHandler;

    [SerializeField] public WeaponHandler WeaponPrefab;

    protected WeaponHandler weaponHandler;
    protected bool isAttacking;
    private float timeSincelastAttack = float.MaxValue;
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationhandler = GetComponent<AnimationHandler>();
        statHandler = GetComponent<StatHandler>();
        if(WeaponPrefab != null)
        {
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
        }
        else
        {
            weaponHandler = GetComponentInChildren<WeaponHandler>(); // WeaponPrefab이 null이라면 찾아본다.
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
        Rotate(lookDirection);
        HandleAttackDelay();
    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime; // knockbackDuration을 매 프레임마다 빼준다.
        }
    }

    protected virtual void HandleAction()
    {

    }

    private void Movement(Vector2 direction)
    {
        direction = direction * statHandler.Speed; // 받아온 direction에 speed만큼 이동, 적은 EnemyController의 handleAction에, Player는 OnMove에 지정되어있음.
        if(knockbackDuration > 0.0f)
        {
            direction *= 0.2f;
            direction += knockback;
        }//넉백 중이라면 넉백을 하도록 함, 이동의 전체 크기를 0.2만큼 낮추고, knockback 벡터를 direction에 더함
        _rigidbody.velocity = direction; // 물리 연산을 하는 rigidbody의 velocity에 direction을 넣어줌
        animationhandler.Move(direction); // 이동 애니메이션 키세요 라는 뜻
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //각도 구하기, 뒤의 것은 라디안 값에서 우리가 잘 아는 각도로 변환해주는 것.
        bool isLeft = Mathf.Abs(rotZ) > 90; 
        characterRenderer.flipX = isLeft;//90도 넘나요? 예-> 왼쪽 보셈 아니오-> 오른쪽 보셈

        if (weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);//무기도 같이 돌려주세요
        }
        weaponHandler?.Rotate(isLeft); //좌우 반대도 해주세요
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power; //각 무기의 위치 - 맞은 놈의 위치의 벡터를 power만큼 knockback시킨다.
    }

    private void HandleAttackDelay()
    {
        if(weaponHandler == null)
        {
            return;
        }
        if (timeSincelastAttack <= weaponHandler.Delay)
        {
            timeSincelastAttack += Time.deltaTime;
        }
        if(isAttacking && timeSincelastAttack > weaponHandler.Delay)
        {
            timeSincelastAttack = 0.0f;
            Attack();
        }
    }
    protected virtual void Attack()
    {
        if(lookDirection != Vector2.zero)
        {
            weaponHandler?.Attack();//그냥 뭔가를 보고있지도 않다면 공격하지 마세요, 즉 lookDirection이 시작하기 전에는 공격하지 않는 코드?
        }
    }

    public virtual void Death()
    {
        _rigidbody.velocity = Vector3.zero; //이동을 멈추고

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color; //알파값을 0.3으로 다 정하겠습니다~
        }

        foreach(Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false; //컴포넌트도 다 꺼버리겠습니다~
        }

        Destroy(gameObject, 2.0f); //2초 뒤에 삭제해버리겠습니다~
    }
}
