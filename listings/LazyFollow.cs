public class LazyFollowUI : MonoBehaviour
{
    private void Update()
    {
        var forward = Vector3.ProjectOnPlane(camera.forward, Vector3.up).normalized;
        var desiredPos = camera.position + forward * distance + Vector3.up * heightOffset;
        
        var angle = Vector3.Angle(transform.position - camera.position, camera.forward);
        if (angle > angleThreshold)
            shouldFollow = true;

        if (shouldFollow)
        {
            transform.position = Vector3.Lerp(
                transform.position, desiredPos,
                followSpeed * Time.deltaTime);

            Quaternion lookRot = Quaternion.LookRotation(
                transform.position - camera.position);
                
            transform.rotation = Quaternion.Slerp(
                transform.rotation, lookRot,
                followSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, desiredPos) < 0.01f)
                shouldFollow = false;
        }
    }
}