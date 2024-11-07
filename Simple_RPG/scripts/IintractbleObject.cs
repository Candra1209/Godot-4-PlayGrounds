using Godot;

namespace Playground_C.Simple_RPG.scripts;

public interface IIntractbleObject
{
    void Interact();
    void Interact(bool newState);
    void play_animation(){}
    
    void stop_animation(){}

    void show_Highlight();
    
    void ShowIndicator(){}
    
    void HideIndicator(){}

    void hide_Highlight();
    void play_effect(){}
    void IsEnterDetectArea();
    void IsExitDetectArea();

}