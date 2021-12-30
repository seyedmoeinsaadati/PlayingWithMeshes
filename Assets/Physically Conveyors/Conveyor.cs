using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Conveyor : MonoBehaviour
{
    public bool active = true;
    public float speed;
    public Vector3 direction = Vector3.back;

    private Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (active)
        {
            Move();
        }
    }

    public void Move()
    {
        Vector3 pos = rb.position;
        rb.position += direction.normalized * (speed * Time.fixedDeltaTime);
        rb.MovePosition(pos);
    }
}