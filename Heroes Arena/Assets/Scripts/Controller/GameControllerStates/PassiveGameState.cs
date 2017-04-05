namespace HeroesArena
{
    public class PassiveGameState : BaseGameState
    {
        public override void Enter()
        {
            base.Enter();
            GameStateLabel.text = "State: Opponent's Turn.";
            RefreshPlayerLabels();
        }
    }
}