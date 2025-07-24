using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Attack Info")]
    [SerializeField] private float delay = 1.0f;
    public float Delay { get => delay; set => delay = value; }
    [SerializeField] private float weaponSize = 1.0f;
    public float WeaponSize { get => weaponSize; set => weaponSize = value; }

    [SerializeField] private float power = 1.0f;
    public float Power { get => power; set => power = value; }

    [SerializeField] private float speed = 1.0f;
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] private float attackRange = 10.0f;
    public float AttackRange { get => attackRange; set => attackRange = value; }

    public LayerMask target;

    [Header("Knock Back Info")]
    [SerializeField] private bool isOnKnockback = false;
    public bool IsOnKnockback { get => isOnKnockback; set => isOnKnockback = value; }
    [SerializeField] private float knockbackPower = 0.1f;
    public float KnockbackPower { get => knockbackPower; set => knockbackPower = value; }
    [SerializeField] private float knockbackTime = 0.5f;
    public float KnockbackTime { get => knockbackTime; set => knockbackTime = value; }
    private static readonly int IsAttack = Animator.StringToHash("IsAttack");

    public BaseController Controller { get; private set; }
    private Animator animator;
    private SpriteRenderer weaponRenderer;

    public AudioClip attackSoundClip;

    protected virtual void Awake()
    {
        Controller = GetComponentInParent<BaseController>();
        animator = GetComponentInChildren<Animator>();
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();

        animator.speed = 1.0f / delay;//속도는 1.0f에 delay를 나눈 값만큼으로 결정된다. 즉 delay가 적으면 적을수록 속도는 빨라진다.
        transform.localScale = Vector3.one * weaponSize;
    }

    protected virtual void Start()
    {

    }

    public virtual void Attack()
    {
        AttackAnimation();//애니메이션을 호출하고

        if(attackSoundClip != null)
        {
            SoundManager.PlayClip(attackSoundClip);//소리를 출력한다.
        }
    }

    public void AttackAnimation()
    {
        animator.SetTrigger(IsAttack);//공격시 트리거 활성화
    }

    public virtual void Rotate(bool isLeft)
    {
        weaponRenderer.flipY = isLeft;//왼쪽볼까요? 오른쪽 볼까요?
    }
}
