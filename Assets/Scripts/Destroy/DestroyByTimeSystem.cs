using Unity.Entities;

namespace SpaceArcade.Destroy
{
    public partial class DestroyByTimeSystem : SystemBase
    {
        BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;
        
        protected override void OnCreate()
        {
            m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            RequireForUpdate(GetEntityQuery(new EntityQueryDesc
            {
                None = new ComponentType[] { typeof(DestroyTag) },
                All = new ComponentType[] { typeof(DestroyByTime) }
            }));
        }

        protected override void OnUpdate()
        {
            var dt = Time.DeltaTime;
            var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer();
            
            Entities
                .WithBurst()
                .ForEach((Entity entity, ref DestroyByTime destroyByTimeComponent) =>
                {
                    destroyByTimeComponent.DestroyTimer -= dt;
                    if (destroyByTimeComponent.DestroyTimer < 0)
                        commandBuffer.AddComponent(entity, new DestroyTag { Type = DestroyType.Time });
                })
                .Schedule();
            
            m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}