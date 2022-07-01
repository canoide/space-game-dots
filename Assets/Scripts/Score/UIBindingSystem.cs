using Unity.Entities;

namespace SpaceArcade.Score
{
    public partial class UIBindingSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .ForEach((ScorePanel currencyBinding) =>
                {
                    var gameDataEntity = GetEntityQuery(typeof(GameData)).GetSingletonEntity();
                    var gameData = GetComponentDataFromEntity<GameData>()[gameDataEntity];
                    if (currencyBinding.SetScoreValue != gameData.Score)
                    {
                        currencyBinding.SetScoreValue = gameData.Score;
                        currencyBinding.Text.SetText($"Score: {gameData.Score}");
                    }
                }).Run();
        }
    }
}