public class SequenceManager : MonoBehaviour
{
    private int _currentIndex = 0;

    private List<TaskSequence> _sequences = new();
    public IReadOnlyList<TaskSequence> Sequences => _sequences;
    private void Start()
    {
        GatherSequences();
        ActivateSequence(0);
    }
    private void OnEnable()
    {
        GameEvents.OnSequenceCompleted += OnSequenceCompleted;
    }
    private void OnDisable()
    {
        GameEvents.OnSequenceCompleted -= OnSequenceCompleted;
    }
    private void GatherSequences()
    {
        _sequences = FindObjectsByType<TaskSequence>(FindObjectsSortMode.None).ToList();
        _sequences.Sort();
        
        for (var i = 0; i < _sequences.Count; i++)
        {
            if (_sequences[i].SequenceId != i)
            {
                Debug.LogWarning($"Sequence IDs are not sequential! Expected {i}, found {_sequences[i].SequenceId}");
            }
        }
    }
    private void ActivateSequence(int index)
    {
        if (index >= _sequences.Count)
        {
            LevelRuntime.Instance.LevelTimer.Stop();
            GameEvents.OnAllSequencesCompleted?.Invoke();
            return;
        }
        _sequences[index].StartSequence();
    }
    private void OnSequenceCompleted(int sequenceId)
    {
        _currentIndex++;
        ActivateSequence(_currentIndex);
    }
}