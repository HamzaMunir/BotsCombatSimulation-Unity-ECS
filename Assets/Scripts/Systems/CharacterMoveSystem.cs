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
    [UpdateAfter(typeof(TargetLockSystem))]
    public partial class CharacterMoveSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(EntityManager.WorldUnmanaged).AsParallelWriter();
            EntityQuery entitiesQuery = EntityManager.CreateEntityQuery(typeof(TeamComponent));
            NativeArray<Entity> entities = entitiesQuery.ToEntityArray(Allocator.TempJob);
            var transformLookup = GetComponentLookup<LocalTransform>(true);
            
            Entities.WithReadOnly(entities).WithReadOnly(transformLookup).ForEach((Entity entity, int entityInQueryIndex, ref CharacterTarget target, in CharacterMovementSpeed speed, in CharacterAttackRange attackRange) =>
            {
                if (!entities.Contains(target.Target))
                {
                    return;
                }
                var targetTransform = transformLookup.GetRefRO(target.Target);
                var entityTransform = transformLookup.GetRefRO(entity);
                float3 direction = math.normalize(targetTransform.ValueRO.Position - entityTransform.ValueRO.Position);
                if (math.distance(targetTransform.ValueRO.Position, entityTransform.ValueRO.Position) <= attackRange.AttackRange)
                {
                    return;
                }
                var moveSpeed = speed.MovementSpeed > 0 ? speed.MovementSpeed : 2f;
                var newPosition = entityTransform.ValueRO.Position + direction * moveSpeed * deltaTime;
                ecb.SetComponent(entityInQueryIndex, entity, new LocalTransform()
                {
                    Position = newPosition,
                    Rotation = quaternion.LookRotation(direction, math.up()),
                    Scale = 1f
                });
                
            }).ScheduleParallel();
        }
    }
    
}