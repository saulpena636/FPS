using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public Animator animator;
    public float speed = 10.0f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float sphereRadius = 0.3f;
    public LayerMask groundMask;
    bool isGrounded;
    Vector3 velocity;
    public float jumpHeight = 3.0f;
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        characterController.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // Apply impact
        if (impact.magnitude > 0.2f)
        {
            characterController.Move(impact * Time.deltaTime);
        }
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

        //animator.SetFloat("velX", x);
    }

    private Vector3 impact = Vector3.zero;

    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / 3.0f; // mass
    }
}
