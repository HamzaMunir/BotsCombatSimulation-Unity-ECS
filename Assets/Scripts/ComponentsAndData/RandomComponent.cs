using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace ECSBattleSimulation.Components
{
    public struct RandomComponent : IComponentData
    {
        public Random Value;
    }
}