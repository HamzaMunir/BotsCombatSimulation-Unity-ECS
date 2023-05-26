using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Authoring
{
    public class BulletPrefabAuthoring : MonoBehaviour
    {
        public GameObject BulletPrefab;
        
        public class BulletPrefabBaker : Baker<BulletPrefabAuthoring>
        {
            public override void Bake(BulletPrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new BulletPrefabComponent()
                {
                    Bullet = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    public struct BulletPrefabComponent : IComponentData
    {
        public Entity Bullet;
    }
}