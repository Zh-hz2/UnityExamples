using UnityEngine;

public class Platform : MonoBehaviour
{
    Vector3 movement;
    GameObject topLine;
    public float speed;


    void Start()
    {
        movement.y = speed;
        topLine = GameObject.Find("TopLine");

    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        transform.position += movement * Time.deltaTime;

        if(transform.position.y >= topLine.transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
