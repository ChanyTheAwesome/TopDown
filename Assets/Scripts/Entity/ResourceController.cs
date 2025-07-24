using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f;
    private BaseController baseController;
    private StatHandler statHandler;
    private AnimationHandler animationHandler;

    private float timeSinceLastChange = float.MaxValue;

    public float CurrentHealth { get; private set; }
    public float Maxhealth => statHandler.Health;

    public AudioClip damageClip;

    private Action<float, float> OnChangeHealth;

    private void Awake()
    {
        baseController = GetComponent<BaseController>();
        statHandler = GetComponent<StatHandler>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    private void Start()
    {
        CurrentHealth = statHandler.Health;//체력을 최대로 설정해주고
    }
    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay)//마지막 체력변화시간보다 무적 시간이 길다면
        {
            timeSinceLastChange += Time.deltaTime;//시간을 더해보고
            if (timeSinceLastChange >= healthChangeDelay)
            {
                animationHandler.InvincibilityEnd();//끝났다면 무적시간을 끝낸다.
            }
        }//왜이렇게 했냐면, 단순히 if-else로 했다면 InvincibilityEnd가 계속 호출됐을 것이다. 한번만 호출하도록 보장했다.
    }
    public bool ChangeHealth(float change)
    {
        if (change == 0 || timeSinceLastChange < healthChangeDelay)
        {
            return false;//변화값이 없거나, 무적시간동안엔 false를 보낸다.
        }

        timeSinceLastChange = 0.0f;//지나간 시간을 0초로 설정하고
        CurrentHealth += change;//변화를 준 다음,
        CurrentHealth = (CurrentHealth > Maxhealth) ? Maxhealth : CurrentHealth;//변화값이 최대체력보다 높은지 체크,
        CurrentHealth = (CurrentHealth < 0) ? 0: CurrentHealth;//0보다 낮은지 체크한다.

        OnChangeHealth?.Invoke(CurrentHealth, Maxhealth);//OnChangeHealth에 등록된 작동이 있다면 실행한다.
        if(change < 0)
        {
            animationHandler.Damage();//맞았다는 애니메이션과
            if (damageClip != null)
            {
                SoundManager.PlayClip(damageClip);//소리를 출력하고
            }
        }

        if (CurrentHealth <= 0.0f)
        {
            Death();//체력이 0이되면 사망 메서드를 호출한다.
        }

        return true;
    }
    private void Death()
    {
        baseController.Death();
    }

    public void AddHealthChangeEvent(Action<float, float> action)
    {
        OnChangeHealth += action;//OnChangeHealth액션에 이벤트를 추가한다.
    }

    public void RemoveHealthChangeEvent(Action<float, float> action)
    {
        OnChangeHealth -= action;//OnChangeHealth액션에 이벤트를 제거한다.
    }
}
