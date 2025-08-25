using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager instance;
    public static ProjectileManager Instance { get { return instance; } }

    [SerializeField] private GameObject[] projectilePrefabs;

    [SerializeField] private ParticleSystem impactParticleSystem;

    ObjectPoolManager objectPoolManager;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        objectPoolManager = ObjectPoolManager.Instance;
    }

    public void ShootBullet(RangeWeaponHandler rangeWeaponHandler, Vector2 startPosition, Vector2 direction)
    {
        // GameObject origin = projectilePrefabs[rangeWeaponHandler.BulletIndex];
        // GameObject obj = Instantiate(origin,startPosition,Quaternion.identity);
        GameObject obj = objectPoolManager.GetObject(rangeWeaponHandler.BulletIndex, startPosition, Quaternion.identity);

        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(direction, rangeWeaponHandler, this);
    }

    public void CreateImpactParticlesAtPosition(Vector3 position, RangeWeaponHandler weaponHandler)
    {
        impactParticleSystem.transform.position = position;//�ε��� ���� ������
        ParticleSystem.EmissionModule em = impactParticleSystem.emission;
        em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(weaponHandler.BulletSize * 5)));//�ȿ� �� �Ű������� �ð���, ��ƼŬ ����. 
        //����Ե� �Ѿ��� ũ�Ⱑ Ŀ������ ��ƼŬ�� ������ ������ ���̴�! Ceil�� �־��� ������ �۰ų� ���� �ִ� ������ ����ش�.

        ParticleSystem.MainModule mainModule = impactParticleSystem.main;//��ƼŬ �ý����� �������� ������

        mainModule.startSpeedMultiplier = weaponHandler.BulletSize * 10f;//���� ���ǵ带 �����ϰ�
        impactParticleSystem.Play();//��ƼŬ�� ����Ѵ�.
    }
}
