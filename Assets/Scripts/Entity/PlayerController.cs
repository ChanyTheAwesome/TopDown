using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private Camera camera;

    private GameManager gameManager;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        camera = Camera.main;
    }

    protected override void HandleAction()
    {
        

        

        

        
    }

    public void UseItem(ItemData item)
    {
        foreach (StatEntry modifier in item.statModifiers)
        {
            statHandler.ModifyStat(modifier.statType, modifier.baseValue, !item.isTemporary, item.duration);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ItemHandler>(out ItemHandler handler))
        {
            if (handler.ItemData == null)
            {
                return;
            }

            UseItem(handler.ItemData);
            Destroy(handler.gameObject);
        }
    }
    public override void Death()
    {
        base.Death();
        gameManager.GameOver();
    }

    void OnMove(InputValue inputValue)
    {
        movementDirection = inputValue.Get<Vector2>();
        movementDirection = movementDirection.normalized;//InputValue�� ���͸� �����ش�.
    }

    void OnLook(InputValue inputValue)
    {
        Vector2 mousePosition = inputValue.Get<Vector2>();//���콺�� ��ġ ��ǥ�� �ް�
        Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);//ī�޶��� ���� ��ǥ�� �޾�

        lookDirection = (worldPos - (Vector2)transform.position);//������
        if (lookDirection.magnitude < 0.9f)
        {
            lookDirection = Vector2.zero;
        }
        else
        {
            lookDirection = lookDirection.normalized;//�̰����� �ٶ󺸴� ������ ���͸� �����Ѵ�.
        }
    }

    void OnFire(InputValue inputValue)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;//��� ĵ������ �׸� �Ϳ� ���콺�� �ø� �ڿ� ���콺�� ������ �����ع����ڴٴ� ��
        }
        isAttacking = inputValue.isPressed;//isAttacking�� ���콺�� ���ȴ��� �����ش�.
    }
}
