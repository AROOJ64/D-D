using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public enum Scene
    { 
        Map,
        DiceRoll,
        CharacterSheet,
        SceneManager,
        Showcase
    };
    public void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
    public void MapScene()
    {
        Load(Scene.Map);
    }
    public void DiceRollScene()
    {
        Load(Scene.DiceRoll);
    }
    public void CharacterSheetScene()
    {
        Load(Scene.CharacterSheet);
    }
    public void SceneManagerScene()
    {
        Load(Scene.SceneManager);
    }
    public void ShowcaseScene()
    {
        Load(Scene.Showcase);
    }
}
