using ECSBattleSimulation.Authoring;
using ECSBattleSimulation.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECSBattleSimulation.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct UpdateHealthSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.Enabled = false;
        }
        
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            foreach(var (healthComponent,entity) in SystemAPI.Query<HealthComponent>().WithEntityAccess())
            {
                GameObject go = GameObject.Instantiate(healthComponent.HealthUI);
                go.AddComponent<EntityGameObject>().AssignEntity(entity, state.World);

                ecb.AddComponent(entity, new HealthComponentTransform() { Transform = go.transform });
                ecb.AddComponent(entity, new HealthUIComponent() {  HealthDisplay = go.GetComponent<HealthDisplayUIView>() });
                ecb.AddComponent(entity, new HealthUIEntity() { Value = entity });
                ecb.RemoveComponent<HealthComponent>(entity);
            }
   
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>();
            var healthLookup = SystemAPI.GetComponentLookup<CharacterHealthPoints>();
            foreach (var (goTransform,healthComponent, teamComponent, entity ) in SystemAPI.Query<HealthComponentTransform, HealthUIComponent, RefRO<TeamComponent>>().WithEntityAccess())
            {
                if (transformLookup.HasComponent(entity))
                {
                    goTransform.Transform.position = transformLookup.GetRefRO(entity).ValueRO.Position + new float3(0, 4 , 0);
                }

                if (healthLookup.HasComponent(entity))
                {
                    healthComponent.HealthDisplay.SetHealth(healthLookup.GetRefRO(entity).ValueRO.CurrentHealth);
                    
                }
            }
            
            foreach(var (goTransform,entity) in SystemAPI.Query<HealthComponentTransform>().WithNone<HealthUIComponent>().WithEntityAccess())
            {
                if (goTransform.Transform != null)
                {
                    GameObject.Destroy(goTransform.Transform.gameObject);
                }
                ecb.RemoveComponent<HealthComponentTransform>(entity);
            }
        }
    }
}