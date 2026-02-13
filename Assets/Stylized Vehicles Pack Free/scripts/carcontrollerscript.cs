using Unity.Mathematics;
using UnityEngine;

public class carcontrollerscript : MonoBehaviour
{
    private Rigidbody playerrb;
    public wheelcolliders wheelcolliders;
    public wheelmeshes meshes;
    public float gasinput;
    public float steerinput;
    public float motorpower;
    private float speed;
    public AnimationCurve steeringcurve;
    private float slipangle;
    public float brakeinput;
    public float brakepower;
    public Transform carcenterofmass;
    public Rigidbody rigidbody1;

    // Boost Variables
    public float boostPower = 800f;
    public float boostDuration = 2.0f;
    private float currentMotorPower;
    private float initialMotorPower;
    private bool isBoosting = false;

    // Cache wheel colliders to reduce GetComponent calls
    private WheelCollider[] allWheels;

    void Start()
    {
        playerrb = gameObject.GetComponent<Rigidbody>();
        rigidbody1.centerOfMass = carcenterofmass.transform.localPosition;

        initialMotorPower = motorpower;
        currentMotorPower = initialMotorPower;

        // Cache all wheel colliders
        allWheels = new WheelCollider[]
        {
            wheelcolliders.frwheel,
            wheelcolliders.flwheel,
            wheelcolliders.blwheel,
            wheelcolliders.brwheel
        };
    }

    void Update()
    {
        speed = playerrb.linearVelocity.magnitude;
        checkinput();
        HandleBoost(); // Check for boost input every frame
    }

    void FixedUpdate()
    {
        // Move ALL physics-related operations to FixedUpdate
        applysteering();
        motorforce();
        applybrake();
        applyposition();
    }

    void checkinput()
    {
        gasinput = Input.GetAxis("Vertical");
        steerinput = Input.GetAxis("Horizontal");

        // Set brake input based on space key
        brakeinput = Input.GetKey(KeyCode.Space) ? 1f : 0f;
    }

    void applybrake()
    {
        float finalBrake = brakeinput * brakepower;

        // Reset motor torque when braking to prevent conflict
        if (finalBrake > 0.1f)
        {
            wheelcolliders.brwheel.motorTorque = 0f;
            wheelcolliders.blwheel.motorTorque = 0f;
        }

        wheelcolliders.flwheel.brakeTorque = finalBrake * 0.7f;
        wheelcolliders.frwheel.brakeTorque = finalBrake * 0.7f;
        wheelcolliders.blwheel.brakeTorque = finalBrake * 0.3f;
        wheelcolliders.brwheel.brakeTorque = finalBrake * 0.3f;
    }

    void applysteering()
    {
        float steeringangle = steerinput * steeringcurve.Evaluate(speed);
        wheelcolliders.frwheel.steerAngle = steeringangle;
        wheelcolliders.flwheel.steerAngle = steeringangle;
    }

    void motorforce()
    {
        float finalTorque = currentMotorPower * gasinput;
        wheelcolliders.brwheel.motorTorque = finalTorque;
        wheelcolliders.blwheel.motorTorque = finalTorque;
    }

    void applyposition()
    {
        updatewheel(wheelcolliders.frwheel, meshes.frmesh);
        updatewheel(wheelcolliders.flwheel, meshes.flmesh);
        updatewheel(wheelcolliders.blwheel, meshes.blmesh);
        updatewheel(wheelcolliders.brwheel, meshes.brmesh);
    }

    void updatewheel(WheelCollider coll, MeshRenderer mesh)
    {
        Quaternion quat;
        Vector3 position;
        coll.GetWorldPose(out position, out quat);
        mesh.transform.position = position;
        mesh.transform.rotation = quat;
    }

    // Boost Logic Functions
    void HandleBoost()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isBoosting)
        {
            StartBoost();
        }
    }

    void StartBoost()
    {
        isBoosting = true;
        currentMotorPower = boostPower;
        Invoke("StopBoost", boostDuration);
    }

    void StopBoost()
    {
        isBoosting = false;
        currentMotorPower = initialMotorPower;
    }

    // Clean up when destroyed to prevent memory leaks
    void OnDestroy()
    {
        // Ensure all Invoke calls are cancelled
        CancelInvoke();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "hurdle")
        {
            //Time.timeScale = 0f;
            SimpleSceneLoaderByIndex.gameOverPanel.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class wheelcolliders
{
    public WheelCollider frwheel;
    public WheelCollider flwheel;
    public WheelCollider blwheel;
    public WheelCollider brwheel;
}

[System.Serializable]
public class wheelmeshes
{
    public MeshRenderer frmesh;
    public MeshRenderer flmesh;
    public MeshRenderer blmesh;
    public MeshRenderer brmesh;
}