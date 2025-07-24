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

    public override void Death()
    {
        base.Death();
        gameManager.GameOver();
    }

    void OnMove(InputValue inputValue)
    {
        movementDirection = inputValue.Get<Vector2>();
        movementDirection = movementDirection.normalized;//InputValue의 벡터를 정해준다.
    }

    void OnLook(InputValue inputValue)
    {
        Vector2 mousePosition = inputValue.Get<Vector2>();//마우스의 위치 좌표를 받고
        Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition);//카메라의 월드 좌표를 받아

        lookDirection = (worldPos - (Vector2)transform.position);//빼본다
        if (lookDirection.magnitude < 0.9f)
        {
            lookDirection = Vector2.zero;
        }
        else
        {
            lookDirection = lookDirection.normalized;//이것으로 바라보는 방향의 벡터를 지정한다.
        }
    }

    void OnFire(InputValue inputValue)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;//요건 캔버스가 그린 것에 마우스를 올린 뒤에 마우스를 누르면 리턴해버리겠다는 뜻
        }
        isAttacking = inputValue.isPressed;//isAttacking에 마우스가 눌렸는지 보내준다.
    }
}
