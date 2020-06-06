using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SwitchVideos : MonoBehaviour
{
    #region PrivateVariables

    private int _activeEntity;

    #endregion
    
    #region SerializableVariables

    [SerializeField] private List<GameObject> texts;

    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private List<VideoClip> videoClips;

    #endregion

    #region MonoBehaviour

    private void OnEnable()
    {
        _activeEntity = 0;
        videoPlayer.clip = videoClips[_activeEntity];

        foreach (var text in texts)
        {
            text.SetActive(false);
        }
        
        texts[0].SetActive(true);
    }

    #endregion
    
    #region PublicMethods

    public void PreviousEntity()
    {
        texts[_activeEntity].SetActive(false);
        
        if (_activeEntity == 0)
        {
            _activeEntity = videoClips.Count - 1;
        }
        else
        {
            _activeEntity -= 1;
        }
        
        texts[_activeEntity].SetActive(true);
        videoPlayer.clip = videoClips[_activeEntity];
    }

    public void NextEntity()
    {
        texts[_activeEntity].SetActive(false);
        
        if (_activeEntity == videoClips.Count - 1)
        {
            _activeEntity = 0;
        }
        else
        {
            _activeEntity += 1;
        }
        
        texts[_activeEntity].SetActive(true);
        videoPlayer.clip = videoClips[_activeEntity];
    }

    #endregion
}