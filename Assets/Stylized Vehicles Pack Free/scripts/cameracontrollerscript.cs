using UnityEngine;

public class cameracontrollerscript : MonoBehaviour
{
    public Transform player;
    private Rigidbody playerrb;
    public Vector3 offset;
    public float speed;

    void Start()
    {
        playerrb=player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 playforward=(playerrb.linearVelocity+player.transform.forward).normalized;
        transform.position=Vector3.Lerp(transform.position,player.position+player.transform.TransformVector(offset)+
            playforward*(-5f),speed*Time.deltaTime);
        transform.LookAt(player);
    }
}
