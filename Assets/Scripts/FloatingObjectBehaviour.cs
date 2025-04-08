using UnityEngine;

public class FloatingObjectBehaviour : MonoBehaviour
{
    public Vector3 MovementAxis = new Vector3(0,1,0);
    public float amplitude = 10;
    public float frequency = 10;

    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        // Rotate
        //transform.RotateAround(transform.position, new Vector3(0,1,0), Time.deltaTime);

        // Move Up and Down
        transform.position = startingPosition + amplitude * MovementAxis * Mathf.Sin((frequency * 2 * Mathf.PI) * Time.time);

        Debug.Log(Mathf.Sin(Time.time));
    }
}
