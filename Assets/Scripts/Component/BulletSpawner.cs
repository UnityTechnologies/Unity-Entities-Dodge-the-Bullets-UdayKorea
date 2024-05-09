using Unity.Entities;
using Unity.Mathematics;

// 총알 스폰에 필요한 정보를 제공하는 컴포넌트
public struct BulletSpawner : IComponentData
{
    public Entity BulletPrefab; // 총알 프리팹
    public int PoolSize;
    public int CountPerEachSpawn; // 스폰할 총알 개수
    public float SpawnPositionRadius; // 총알이 스폰될 반지름
    public double TimeBetSpawn; // 총알 스폰 간격
    public double NextSpawnTime; // 다음 총알 스폰 시간
    public float2 BulletSpeedMinMax; // 총알 속도 최소 최대값
}