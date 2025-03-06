using UnityEngine;

public class LineRender : MonoBehaviour
{
    LineRenderer line;
    public Transform startPoint;
    public Transform endPoint;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, startPoint.position);
        line.SetPosition(1, endPoint.position);
    }
}
