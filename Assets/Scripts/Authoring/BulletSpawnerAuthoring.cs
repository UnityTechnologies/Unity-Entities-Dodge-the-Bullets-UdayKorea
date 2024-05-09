using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

// Bullet Spawner 게임 오브젝트를 엔티티로 변환하는 스크립트
public class BulletSpawnerAuthoring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 1000;
    public int countPerEachSpawn = 3;
    public float spawnPositionRadius = 20f;
    public float timeBetSpawn = 3f;
    public Vector2 bulletSpeedMinMax = new(1,3);
    
    private class Baker : Baker<BulletSpawnerAuthoring> {
        public override void Bake(BulletSpawnerAuthoring authoring)
        {
            // 총알 생성기는 움직일 필요가 없으므로 TransformUsageFlags.None 플래그를 사용
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new BulletSpawner
            {
                BulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                PoolSize = authoring.poolSize,
                CountPerEachSpawn = authoring.countPerEachSpawn,
                SpawnPositionRadius = authoring.spawnPositionRadius,
                TimeBetSpawn = authoring.timeBetSpawn,
                BulletSpeedMinMax = authoring.bulletSpeedMinMax
            });
        }
    }
}