using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float height = 10f;
    [SerializeField] float smoothSpeed = 5f;

    Vector3 offset;

    void Start()
    {
        offset = new Vector3(0f, height, 0f);

        transform.position = player.position + offset;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}