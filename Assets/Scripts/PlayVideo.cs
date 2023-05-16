using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    // [SerializeField]
    //VideoPlayer myvideoPlayer;
    [SerializeField]
    public VideoPlayer videoPlayer;
    public int timeToStop;

    // Use this for initialization
    void Start()
    {
        videoPlayer.gameObject.SetActive(true);
        //videoPlayer.SetActive(true);
      //  Destroy(videoPlayer, timeToStop);

        videoPlayer.loopPointReached += whenVideoEnds;
    }



 /*   // Update is called once per frame
    void OnTriggerEnter(Collider player)
    {

        if (player.gameObject.tag == "Player")
        {
            videoPlayer.gameObject.SetActive(true);
        //    Destroy(videoPlayer, timeToStop);
        }
    }
*/
    void whenVideoEnds(VideoPlayer vp) {
        Debug.Log("Yeahh video is finished");
    }
}