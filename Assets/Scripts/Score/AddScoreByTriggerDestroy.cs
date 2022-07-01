using Unity.Entities;

[GenerateAuthoringComponent]
public struct AddScoreByTriggerDestroy : IComponentData
{
    public int Score;
}
