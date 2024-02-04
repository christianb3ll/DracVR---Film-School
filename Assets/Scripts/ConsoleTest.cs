using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;
using TMPro;

public class ConsoleTest : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public TimelineAsset timeline;
    public SignalAsset cam1Signal;
    public SignalAsset cam2Signal;

    public Material cam1Mat;
    public Material cam2Mat;

    public SignalReceiver receiver;

    public GameObject testCube;
    public Material testMaterial;

    private TrackAsset track;

    public MeshRenderer screen;

    public TextMeshPro lengthText;
    public TextMeshPro setText;


    private double m1;
    private double m2;

    // private CameraMarker[] markers;

    private List<CameraMarker> markers = new List<CameraMarker>();

    // Start is called before the first frame update
    void Start()
    {
        // track = (TrackAsset)timeline.GetOutputTracks().Where(e => e.name.Equals("CameraFlags"));
        // track = timeline.GetRootTrack(0);
        m1 = -1;
        m2 = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (playableDirector.state == PlayState.Playing)
        {
            for(int i = 0; i < markers.Count; i++)
            {
                if (playableDirector.time > markers[i].timestamp)
                {
                    SetCamera(markers[i].camID);

                    break;
                }
            }
        }
    }

    public void SetMarker1()
    {
        if(playableDirector.state == PlayState.Playing)
        {
            AddMarker(playableDirector.time, 1);
            SetCamera(1);
            //m1 = playableDirector.time;
            //SignalEmitter mark = track.CreateMarker<SignalEmitter>(playableDirector.time);
            
            //mark.asset = cam1Signal;
            testCube.GetComponent<MeshRenderer>().material = testMaterial;
        }
    }

    public void SetMarker2()
    {
        if (playableDirector.state == PlayState.Playing)
        {
            AddMarker(playableDirector.time, 2);
            SetCamera(2);
            //m2 = playableDirector.time;
            //SignalEmitter mark = track.CreateMarker<SignalEmitter>(playableDirector.time);

            //mark.asset = cam2Signal;
            testCube.GetComponent<MeshRenderer>().material = testMaterial;

        }
    }

    private void AddMarker(double time, int id)
    {
        CameraMarker marker = new CameraMarker { timestamp = time, camID = id };
        markers.Add(marker);
        setText.text = "Marker " + id + " added at " + time;
        lengthText.text = markers.Count.ToString();
    }

    private void SetCamera(int id)
    {
        if(id == 1)
        {
            screen.material = cam1Mat;
        }
        else if(id == 2)
        {
            screen.material = cam2Mat;
        }
        else
        {

        }
    }
}

public class CameraMarker : MonoBehaviour
{
    public double timestamp {get;set;}
    public int camID { get; set; }
}
