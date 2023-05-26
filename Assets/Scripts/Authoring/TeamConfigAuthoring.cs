using ECSBattleSimulation.Components;
using ECSBattleSimulation.Scriptable;
using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Authoring
{
    public class TeamConfigAuthoring : MonoBehaviour
    {
        public TeamSpawnData FriendlyTeamConfig;
        public TeamSpawnData EnemyTeamConfig;

        public class TeamConfigBaker : Baker<TeamConfigAuthoring>
        {
            public override void Bake(TeamConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,new SpawnerConfig());
                AddComponent(entity, new FriendlyTeamConfigComponent());
                AddComponent(entity, new EnemyTeamConfigComponent());
            }
        }
    }

    public struct SpawnerConfig : IComponentData
    {
        
    }
}