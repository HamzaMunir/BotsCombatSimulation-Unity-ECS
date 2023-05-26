using ECSBattleSimulation.Authoring;
using ECSBattleSimulation.Components;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Systems
{
    [BurstCompile]
    public partial class CharacterDeathSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnUpdate()
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged).AsParallelWriter();
            
            //Destroy all entities having health points less than or equal to 0 on them
            Entities.WithAll<HealthComponentTransform>().ForEach((Entity entity, int entityInQueryIndex, in CharacterHealthPoints health, in HealthUIEntity healthUIEntity) =>
            {
                if (health.CurrentHealth <= 0)
                {
                    ecb.DestroyEntity(entityInQueryIndex, entity);
                    ecb.DestroyEntity(healthUIEntity.Value.Index, healthUIEntity.Value);
                }
            
            }).ScheduleParallel();
            
        }
    }
}