public enum ElevatorDockType { Start, End }
public class ElevatorDock : MonoBehaviour
{
    public void Initialize()
    {
        OpenDoorsInternal();
    }
    
    public void OpenDoorsViaButton()
    {
        if (dockType != ElevatorDockType.End)
            return;

        UpdateElevator();
        OpenDoorsInternal();
    }

    private void OpenDoorsInternal()
    {
        doorAnimation.Play(GameConstants.DoorOpenAnimation);
    }
    
    private void CloseDoors()
    {
        doorAnimation.Play(GameConstants.DoorCloseAnimation);
    }

    private void UpdateElevator()
    {
        if (dockType == ElevatorDockType.End)
            ElevatorManager.Instance.NextLevelScene = nextScene;
        
        ElevatorManager.Instance.EnterElevator(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dockType != ElevatorDockType.End)
            return;
        
        if (!other.CompareTag(GameConstants.PlayerTag))
            return;
        
        CloseDoors();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (dockType != ElevatorDockType.Start)
            return;
        
        if (!other.CompareTag(GameConstants.PlayerTag))
            return;
        
        CloseDoors();
    }
}