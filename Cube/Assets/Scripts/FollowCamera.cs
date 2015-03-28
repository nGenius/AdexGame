using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Camera mainCamera;
    private Transform fastTransform;
    private Vector3 currentVelocity;

    void Awake()
    {
        fastTransform = transform;
        mainCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        Vector3 targetPosition = GameControl.Instance.playerCharacter.fastTransform.position;
        fastTransform.position = Vector3.SmoothDamp(fastTransform.position, targetPosition, 
            ref currentVelocity, Time.deltaTime);
    }
}