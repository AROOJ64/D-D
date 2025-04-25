using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CMTimeline : MonoBehaviour
{
    public PlayableDirector playableDirector;  

    void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.extrapolationMode = DirectorWrapMode.Hold;
            playableDirector.Pause();
        }
    }

    public void PlayTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
        }
    }
}
