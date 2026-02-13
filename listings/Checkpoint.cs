public void Trigger()
{
    if (_hasTriggered)
        return;
        
    _hasTriggered = true;
    GameEvents.OnCheckpointTriggered?.Invoke(checkpointId);
    onTrigger?.Invoke();
}

private void OnTriggerEnter(Collider other)
{
    if (!other.CompareTag(GameConstants.PlayerTag))
        return;
        
    Trigger();
}