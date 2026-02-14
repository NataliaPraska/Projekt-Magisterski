public class TaskSequence : MonoBehaviour, IComparable<TaskSequence>
{
    [field: SerializeField] public int SequenceId { get; private set; }
    [field: SerializeField] public string TaskName { get; private set; }
    [field: SerializeField] public float ProfessionalTime { get; private set; }
    [SerializeField] private List<string> requiredCheckpoints;
    public IReadOnlyList<string> RequiredCheckpoints => requiredCheckpoints;
    public UnityEvent onSequenceFinished;
    public UnityEvent onSequenceFailed;
    private SequenceState _state = SequenceState.Inactive;
    
    private void OnEnable()
    {
        GameEvents.OnCheckpointTriggered += OnCheckpointTriggered;
    }
    private void OnDisable()
    {
        GameEvents.OnCheckpointTriggered -= OnCheckpointTriggered;
    }
    
    public void StartSequence()
    {
        if (_state != SequenceState.Inactive)
            return;
        
        _reachedCheckpoints.Clear();
        _state = SequenceState.InProgress;
            
        GameEvents.OnSequenceStarted?.Invoke(SequenceId);
    }
    private void Complete()
    {
        _state = SequenceState.Completed;
        LevelRuntime.Instance.ResultCollector.RegisterSequenceResult(
            SequenceId, _sequenceDurationTimer.GetDuration(), _fails
        );
        GameEvents.OnSequenceCompleted?.Invoke(SequenceId);
    }
    private bool IsCompleted()
    {
        foreach (var cp in requiredCheckpoints)
        {
            if (!_reachedCheckpoints.Contains(cp))
                return false;
        }
        
        return true;
    }
    private void CreateIndicators()
    {
        if (indicatorPrefab == null || indicatorContainer == null || requiredCheckpoints == null)
            return;
        ClearIndicators();
        int count = requiredCheckpoints.Count;
        float spacing = count > 1 ? maxIndicatorWidth / (count - 1) : 0f;
        for (int i = 0; i < count; i++)
        {
            var checkpointId = requiredCheckpoints[i];
            var indicatorObj = Instantiate(indicatorPrefab, indicatorContainer);
            var indicator = indicatorObj.GetComponent<CheckpointIndicator>();
            if (indicator == null)
            {
                Debug.LogError($"Prefab nie ma komponentu CheckpointIndicator!");
                Destroy(indicatorObj);
                continue;
            }
            float offset = (i - (count - 1) / 2f) * spacing;
            indicatorObj.transform.localPosition = new Vector3(0f, 0f, offset);
            indicator.Initialize(checkpointId);
            _indicators[checkpointId] = indicator;
        }
    }
    private void ClearIndicators()
    {
        foreach (var indicator in _indicators.Values)
        {
            if (indicator != null)
                Destroy(indicator.gameObject);
        }
        _indicators.Clear();
    }
}