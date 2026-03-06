using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] float speed = 5f;
    Vector2 moveInput = Vector2.zero;


    [Header("Player Stats")]
    [SerializeField] int score = 0;
    [SerializeField] float hunger = 100;



    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        transform.Translate(move * speed * Time.deltaTime, Space.World);
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }


    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
