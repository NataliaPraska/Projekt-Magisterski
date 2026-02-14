public class CheckpointIndicator : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material completedMaterial;
    
    [Header("Animation")]
    [SerializeField] private float highlightDuration = 0.3f;
    [SerializeField] private float scaleMultiplier = 1.3f;
    
    private string _checkpointId;
    private bool _isCompleted = false;
    private Vector3 _originalScale;
    private float _animationTimer = 0f;
    
    private void Awake()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
            
        _originalScale = transform.localScale;
        SetInactive();
    }
    public void Initialize(string checkpointId)
    {
        _checkpointId = checkpointId;
        _isCompleted = false;
        SetInactive();
    }
    public void SetInactive()
    {
        if (meshRenderer != null && inactiveMaterial != null)
            meshRenderer.material = inactiveMaterial;
        
        transform.localScale = _originalScale;
    }
    public void SetCompleted()
    {
        _isCompleted = true;
        if (meshRenderer != null && completedMaterial != null)
            meshRenderer.material = completedMaterial;
        _animationTimer = highlightDuration;
    }
    private void Update()
    {
        if (_animationTimer > 0f)
        {
            _animationTimer -= Time.deltaTime;
            var progress = _animationTimer / highlightDuration;
            var scale = Mathf.Lerp(1f, scaleMultiplier, Mathf.Sin(progress * Mathf.PI));
            transform.localScale = _originalScale * scale;
            if (_animationTimer <= 0f)
            {
                transform.localScale = _originalScale;
            }
        }
    }
    public string GetCheckpointId() => _checkpointId;
    public bool IsCompleted() => _isCompleted;
}