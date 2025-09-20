using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoTransitionManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public VideoClip backgroundLoop;
    public VideoClip transitionClip;
    public string sceneToLoad = "GameScene";
    public Button button;

    private bool playingTransition = false;

    void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        PlayVideo(backgroundLoop, true);
    }

    public void OnStartGamePressed()
    {
        button.gameObject.SetActive(false);
        PlayVideo(transitionClip, false);
        playingTransition = true;


        // When done, call LoadNextScene
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void PlayVideo(VideoClip clip, bool loop)
    {
        videoPlayer.Stop();
        videoPlayer.clip = clip;
        videoPlayer.isLooping = loop;
        videoPlayer.Play();
        rawImage.texture = videoPlayer.targetTexture;
    }
    
    private void OnVideoFinished(VideoPlayer vp)
    {
        if (playingTransition)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
