using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Authoring
{
    public class MatchConfigPrefabAuthoring : MonoBehaviour
    {
        public GameObject MatchConfigPrefab;
        
        public class MatchConfigPrefabBaker : Baker<MatchConfigPrefabAuthoring>
        {
            public override void Bake(MatchConfigPrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MatchConfigPrefabComponent()
                {
                    MatchConfig = GetEntity(authoring.MatchConfigPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    public struct MatchConfigPrefabComponent : IComponentData
    {
        public Entity MatchConfig;
    }
    
}