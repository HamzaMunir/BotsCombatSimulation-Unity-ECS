using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Authoring
{
    public class HealthComponent : IComponentData, IEnableableComponent
    {
        public GameObject HealthUI;
    }

    public class HealthComponentTransform : ICleanupComponentData
    {
        public Transform Transform;
    }

    public class HealthUIComponent : IComponentData
    {
        public HealthDisplayUIView HealthDisplay;
    }

    public struct HealthUIEntity : IComponentData
    {
        public Entity Value;
    }
}