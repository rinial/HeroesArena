namespace HeroesArena
{
    public class LoadGameState : BaseGameState
    {
        public override void Enter()
        {
            base.Enter();
            GameStateLabel.text = "State: Waiting For Players";
            LocalPlayerLabel.text = "Player: -";
        }

        public override void Exit()
        {
            base.Exit();
            RefreshPlayerLabels();
        }
    }
}