using SpaceArcade.Destroy;
using Unity.Entities;

public partial class AddScoreByTriggerDestroySystem : SystemBase
{
    private BeginInitializationEntityCommandBufferSystem m_CommandBufferSystem;

    protected override void OnCreate()
    {
        RequireForUpdate(GetEntityQuery(ComponentType.ReadOnly<AddScoreByTriggerDestroy>(), typeof(DestroyTag)));
        m_CommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = m_CommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

        Entities
            //.WithBurst()
            .WithoutBurst()
            .ForEach((Entity entity, int entityInQueryIndex, in DestroyTag destroyTag, in AddScoreByTriggerDestroy addScore) =>
            {
                if (destroyTag.Type == DestroyType.Trigger)
                {
                    var gameDataEntity = GetEntityQuery(typeof(GameData)).GetSingletonEntity();
                    var gameData = GetComponentDataFromEntity<GameData>()[gameDataEntity];
                    gameData.Score += addScore.Score;
                    SetComponent(gameDataEntity, gameData);
                }
            })
            .Run();
        m_CommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
