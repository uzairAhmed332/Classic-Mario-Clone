using NaughtyAttributes;
using TarodevGhost;
using UnityEngine;

public class GhostRunner : MonoBehaviour {
    [SerializeField] private Transform _recordTarget;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField, Range(1, 10)] private int _captureEveryNFrames = 2;

    private ReplaySystem _system;

    private void Awake() => _system = new ReplaySystem(this);
    
/*    private void OnEnable() => FinishLine.Crossed += OnFinishLineCrossed;
    private void OnDisable() => FinishLine.Crossed -= OnFinishLineCrossed;*/
    
    private void OnFinishLineCrossed(bool runStarting) {
        if (runStarting) {
            _system.StartRun(_recordTarget, _captureEveryNFrames);
            _system.PlayRecording(RecordingType.Best, Instantiate(_ghostPrefab));
        }
        else {
            _system.FinishRun();
            _system.StopReplay();
        }
    }
    [Button]
    public void startRecording() {
        _system.StartRun(_recordTarget, _captureEveryNFrames);
    }

    [Button]
    public void stopRecording()
    {
        _system.GetRun(RecordingType.Last, out var run);
        _system.SetSavedRun(run);

        _system.FinishRun();
       
        //_system.StopReplay();

    }

    [Button]
    public void playRecording()
    {
        _system.PlayRecording(RecordingType.Last, Instantiate(_ghostPrefab)); // The ghost should be a very basic prefab without colliders or rigidbodies. See the demo scene for an example.
    }

}

