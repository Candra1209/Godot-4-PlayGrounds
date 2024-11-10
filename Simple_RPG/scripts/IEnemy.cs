namespace Playground_C.Simple_RPG.scripts;

public interface IEnemy
{
    int Health { get; set; }
    int Defender { get; set; }
    int Power { get; set; }

    void Hit();
}