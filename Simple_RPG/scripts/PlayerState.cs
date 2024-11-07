using Godot;

namespace Playground_C.Simple_RPG.scripts;

public interface IPlayerState
{
    void EnterState(Player player);
    void MoveState(Player player, Vector2 direction, float deltaTime);
    void PlayAnimation(AnimationPlayer animationPlayer, Vector2 direction);

    string getAnimationDepan();
    string getAnimationBelakang();
    string getAnimationSamping();
}

public class RunState : IPlayerState
{
    private float _baseSpeed = 10000;

    public void EnterState(Player player)
    {
        Godot.GD.Print("memasuki state berlari");
    }

    public void MoveState(Player player, Vector2 direction, float deltaTime)
    {
        player.Velocity = direction * deltaTime * _baseSpeed;
    }

    public void PlayAnimation(AnimationPlayer animationPlayer, Vector2 direction)
    {
       
    }

    public string getAnimationDepan()
    {
        return "dash_depan";
    }

    public string getAnimationBelakang()
    {
        return "dash_belakang";
    }

    public string getAnimationSamping()
    {
        return "dash_samping";
    }
}

public class WalkState : IPlayerState
{
    private float _baseSpeed = 4000;
    
    public void EnterState(Player player)
    {
        Godot.GD.Print("memasuki state_berjalan");
    }

    public void MoveState(Player player, Vector2 direction, float deltaTime)
    {
        player.Velocity = direction * deltaTime * _baseSpeed;
    }

    public void PlayAnimation(AnimationPlayer animationPlayer, Vector2 direction)
    {
        throw new System.NotImplementedException();
    }

    public string getAnimationDepan()
    {
        return "walk_depan";
    }

    public string getAnimationBelakang()
    {
        return "walk_belakang";
    }

    public string getAnimationSamping()
    {
        return "walk_samping";
    }
}