using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour//단순히 스탯을 가지고 있다.
{
    [Range(1, 100)][SerializeField] private int health = 10;//SerializeField를 통해 인스펙터에서 볼 수 있게 하고, 범위를 설정할 수 있도록 했다. 슬라이더가 생긴다!
    public int Health { get => health; set => health = Mathf.Clamp(value, 0, 100); }//Clamp는 최소/최대값 안에서만 놀 수 있게 하는 녀석이다.

    [Range(1.0f, 20.0f)][SerializeField] private float speed = 3.0f;
    public float Speed { get => speed; set => speed = Mathf.Clamp(value, 0, 20); }
}
