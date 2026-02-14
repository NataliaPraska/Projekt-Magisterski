public enum ElevatorState
{
    Idle,
    ShowingSummary,
    ShowingSettings,
    Transitioning
}

public class ElevatorManager : MonoBehaviour
{
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnLevelLoaded;

        var dockStart = GetDockObject(ElevatorDockType.Start);
        if (dockStart != null)
            SnapToDock(dockStart);
    }
    
    public void EnterElevator(ElevatorDock dockEnd)
    {
        if (_state != ElevatorState.Idle)
            return;
        
        _currentLevelScene = dockEnd.gameObject.scene.name;
        
        SnapToDock(dockEnd);
        ShowSummary();
    }
    
    public void LoadNextLevel()
    {
        StartCoroutine(TransitionRoutine(NextLevelScene));
    }

    private IEnumerator TransitionRoutine(string levelName)
    {
        _state = ElevatorState.Transitioning;

        var behaviourMode = PlayerPrefs.GetInt("BehaviourMode", 0);
        var fullLevelName = $"{levelName}_{behaviourMode}";
        var loadOperation = SceneManager.LoadSceneAsync(fullLevelName, LoadSceneMode.Single);
        while (loadOperation is { isDone: false })
            yield return null;

        yield return new WaitForSeconds(delay);

        var dockStart = GetDockObject(ElevatorDockType.Start);
        if (dockStart == null)
        {
            Debug.LogError("Cannot complete transition: Start dock not found!");
            _state = ElevatorState.Idle;
            yield break;
        }
        
        yield return new WaitForSeconds(delay);
        
        SnapToDock(dockStart);
        
        OnTransitionFinished(dockStart);
    }

    private ElevatorDock GetDockObject(ElevatorDockType dockType)
    {
        if (_dockCache.TryGetValue(dockType, out var cachedDock))
            return cachedDock;

        var docks = FindObjectsByType<ElevatorDock>(FindObjectsSortMode.None);

        foreach (var dock in docks)
        {
            if (dock.dockType == dockType)
            {
                _dockCache[dockType] = dock;
                return dock;
            }
        }

        Debug.LogError($"Brak ElevatorDock typu {dockType} na scenie!");
        return null;
    }
}
