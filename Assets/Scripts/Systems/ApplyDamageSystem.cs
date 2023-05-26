using ECSBattleSimulation.Authoring;
using ECSBattleSimulation.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECSBattleSimulation.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(CharacterAttackSystem))]
    public partial class ApplyDamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged).AsParallelWriter();
            
            var transformLookup = GetComponentLookup<LocalTransform>(true);
            var healthLookup = GetComponentLookup<CharacterHealthPoints>(true);
            
            Entities.WithReadOnly(healthLookup).WithReadOnly(transformLookup).ForEach((Entity entity,
                int entityInQueryIndex,
                in BulletProperties bulletProperties) =>
            {
                if (!transformLookup.HasComponent(bulletProperties.TargetEntity))
                {
                    ecb.DestroyEntity(entityInQueryIndex, entity);
                    return;
                }
                var targetTransform = transformLookup.GetRefRO(bulletProperties.TargetEntity);
                var bulletTransform = transformLookup.GetRefRO(entity);
                var distance = math.distance(targetTransform.ValueRO.Position, bulletTransform.ValueRO.Position);
                
                //Check if bullet has reached its target
                if (distance < 0.1f)
                {
                    var targetHealth = healthLookup.GetRefRO(bulletProperties.TargetEntity);
                    ecb.AddComponent(entityInQueryIndex, bulletProperties.TargetEntity, new CharacterHealthPoints()
                    {
                        HealthPoints = targetHealth.ValueRO.HealthPoints,
                        CurrentHealth = targetHealth.ValueRO.CurrentHealth - bulletProperties.Damage
                    });
                    ecb.DestroyEntity(entityInQueryIndex, entity);
                }
                else
                {
                    //translate the bullet to the target
                    float3 direction =
                        math.normalize(targetTransform.ValueRO.Position - bulletTransform.ValueRO.Position);
                    var nextPosition = bulletTransform.ValueRO.Position + direction * 10f * deltaTime;
                    ecb.AddComponent(entityInQueryIndex, entity, new LocalTransform()
                    {
                        Position = nextPosition,
                        Rotation = quaternion.LookRotation(direction, math.up()),
                        Scale = 0.25f
                    });
                }
            }).ScheduleParallel();
        }
    }
}