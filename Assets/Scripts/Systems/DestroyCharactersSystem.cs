using ECSBattleSimulation.Authoring;
using ECSBattleSimulation.Components;
using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Systems
{
    public partial class DestroyCharactersSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Enabled = false;
        }

        protected override void OnUpdate()
        {
            Enabled = false;

            var ecbSingleton = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged).AsParallelWriter();

            //Destroy all entities having team component on them i.e. character entities
            //Destroy all health ui components

            Entities.WithAll<HealthComponentTransform>().ForEach(
                (Entity entity, int entityInQueryIndex, in TeamComponent team, in HealthUIEntity healthUIEntity) =>
                {
                    ecb.DestroyEntity(entityInQueryIndex, entity);
                    ecb.DestroyEntity(healthUIEntity.Value.Index, healthUIEntity.Value);
                    
                }).ScheduleParallel();


            EntityManager.WorldUnmanaged.GetExistingSystemState<TeamSpawnSystem>().Enabled = true;
        }
    }
}