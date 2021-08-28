using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class CarController : MonoBehaviour
{
    [Header("Basic Attributes")]
    public float accelaration;
    public float turnForce, backupTurnForce, brakeForce;
    [Range(0, 1f)]
    public float wheelFriction, driftFriction, maxBrakeFriction;

    [Header("Fine Tuning")]
    [Range(0, 1)]
    public float gasLerp;
    [Range(0, 1)]
    public float brakeLerp, turnLerp, turnDecayLerp;
    public float reverseThreshhold;

    [Header("Visual Effect Tuning")]
    public float maxTireAngle;
    public float tireSpeed, tireDriveSpeed;

    public CarTilt tiltControls;



    private Transform body, frame, wheelParent;
    private Transform[] wheels;

    private Rigidbody rb;
    private Vector3 surfaceNormal,
        previousVelocity, deltaV;

    // controls
    private float gas, turn, brake, rotation;

    private float groundThreshold, tireRot, fTireRot;
    private bool ground;


    private Vector3 debugVec1, debugVec2, debugVec3;

    private void OnDrawGizmos()
    {
        if (!body)
            return;
        Gizmos.matrix = Matrix4x4.Translate(body.position);
        Gizmos.color = new Color(1, 0, 0);
        DrawGizmoArrow(debugVec1);
        Gizmos.color = new Color(0, 1, 0);
        DrawGizmoArrow(debugVec2);
        Gizmos.color = new Color(0, 0, 1);
        DrawGizmoArrow(debugVec3);
    }

    private void DrawGizmoArrow(Vector3 tip)
    {
        Vector3 tail = Vector3.zero;
        float headWidth = tip.magnitude / 8;
        Gizmos.DrawLine(tip, tail);
        tail = tip * .75f;
        Gizmos.DrawLine(tip, tail + Vector3.left * headWidth);
        Gizmos.DrawLine(tip, tail + Vector3.right * headWidth);
        //Gizmos.DrawLine(tip, tail + Vector3.forward * headWidth);
        //Gizmos.DrawLine(tip, tail + Vector3.back * headWidth);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        groundThreshold = rb.GetComponent<SphereCollider>().radius * Mathf.Sqrt(2);

        body = transform.GetChild(0);
        frame = body.GetChild(0);
        wheelParent = body.GetChild(1);
        wheels = wheelParent.GetComponentsInChildren<Transform>().
            Where(t => t != wheelParent).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateControls();
        Turn();
        UpdatePositions();
        RunWheels();
        RunFrame();
    }

    private void FixedUpdate()
    {
        previousVelocity = rb.velocity;
        if (ground)
        {
            ApplyFriction();
            rb.AddForce(body.forward * gas * accelaration);
        }
    }

    private void ApplyFriction()
    {
        Vector3 normVel = Vector3.Project(rb.velocity, surfaceNormal);
        Vector3 flatVel = Vector3.ProjectOnPlane(rb.velocity, surfaceNormal);


        Vector3 forward = Vector3.ProjectOnPlane(body.forward, surfaceNormal);
        Vector3 right = Vector3.ProjectOnPlane(body.right, surfaceNormal);

        float paraFric = wheelFriction + brake;
        paraFric = Mathf.Clamp(paraFric, 0, maxBrakeFriction);

        Vector3 paraV = Vector3.Project(flatVel, forward);
        Vector3 perpV = Vector3.Project(flatVel, right);

        if (Vector3.Dot(paraV, forward) <= reverseThreshhold)
        {
            paraFric = wheelFriction;
            rb.AddForce(-body.forward * brake * brakeForce);
        }

        paraV = paraV * (1 - paraFric);
        perpV = perpV * (1 - driftFriction);

        rb.velocity = paraV + perpV + normVel;
    }

    private void UpdateControls()
    {
        bool gasDecay = true, brakeDecay = true, turnDecay = true;
        Gamepad g = Gamepad.current;
        if (g != null)
        {
            turn = g.leftStick.x.ReadValue();
            turnDecay = turn == 0;
            gas = g.rightTrigger.ReadValue();
            gasDecay = gas == 0;
            brake = g.leftTrigger.ReadValue();
            brakeDecay = brake == 0;
        }
        var k = Keyboard.current;
        if (k.wKey.isPressed || k.upArrowKey.isPressed)
            gas = Mathf.Lerp(gas, 1, gasLerp);
        else if(gasDecay)
            gas = 0;

        if (k.sKey.isPressed || k.downArrowKey.isPressed)
            brake = Mathf.Lerp(brake, 1, brakeLerp);
        else if(brakeDecay)
            brake = 0;

        if(turnDecay)
            turn = Mathf.Lerp(turn, 0, turnDecayLerp);
        if (k.aKey.isPressed || k.leftArrowKey.isPressed)
            turn = Mathf.Lerp(turn, -1, turnLerp);
        if (k.dKey.isPressed || k.rightArrowKey.isPressed)
            turn = Mathf.Lerp(turn, 1, turnLerp);

        gas = CleanInput(gas);
        turn = CleanInput(turn);
        brake = CleanInput(brake);
    }

    private float CleanInput(float f)
    {
        return Mathf.Clamp(f, -1, 1);
    }

    private void Turn()
    {
        float v = Vector3.Dot(rb.velocity, body.forward);
        float diff = turn * v * Time.deltaTime;
        if (v > 0)
            diff *= turnForce;
        else
            diff *= backupTurnForce;
        rotation = LoopAngle(rotation + diff);
    }

    private void UpdatePositions()
    {
        body.position = rb.position;

        RaycastHit rayHit;
        Physics.Raycast(rb.position, -body.up, out rayHit);
        ground = rayHit.distance < groundThreshold;
        if (ground) surfaceNormal = rayHit.normal;
        else
        {
            Physics.Raycast(rb.position, Vector3.down, out rayHit);
            ground = rayHit.distance < groundThreshold;
            if (ground) surfaceNormal = rayHit.normal;
        }

        body.up = Vector3.Lerp(body.up, surfaceNormal, .2f);
        body.RotateAround(body.position, body.up, rotation);
    }

    private float LoopAngle(float angle)
    {
        if (angle > 360)
            return angle - 360;
        else if (rotation < 0)
            return angle + 360;
        return angle;
    }

    private void RunWheels()
    {
        float rotDiff = Vector3.Dot(rb.velocity, body.forward) * tireSpeed;
        tireRot = LoopAngle(tireRot + rotDiff);

        Quaternion q = Quaternion.Euler(Vector3.right * tireRot);
        wheels[2].localRotation = q;
        wheels[3].localRotation = q;

        fTireRot += rotDiff;
        rotDiff = gas * tireDriveSpeed;
        fTireRot = LoopAngle(fTireRot + rotDiff);
        q = Quaternion.Euler(Vector3.up * turn * maxTireAngle +
            Vector3.right * fTireRot);
        wheels[0].localRotation = q;
        wheels[1].localRotation = q;
    }

    private void RunFrame()
    {
        Vector3 diff = previousVelocity - rb.velocity;
        deltaV = diff;
        //deltaV = Vector3.Lerp(deltaV, 
        //    previousVelocity - rb.velocity, 0.025f);

        float parAcc = Vector3.Dot(deltaV, body.forward);
        float perpAcc = Vector3.Dot(deltaV, body.right);
        float normAcc = Vector3.Dot(deltaV, body.up);

        tiltControls.UpdateCarTilt(frame, parAcc, perpAcc, normAcc);

        if (Mathf.Abs(perpAcc) > 5)
            wheelParent.localRotation = Quaternion.Euler(0, 0, -perpAcc * 2);
        else
            wheelParent.localRotation = Quaternion.identity;
    }

    [System.Serializable]
    public class CarTilt
    {
        [Header("Position")]
        public CarTiltAxis paraMove;
        public CarTiltAxis normMove, perpMove;

        [Header("rotation")]
        public CarTiltAxis paraTilt;
        public CarTiltAxis perpTilt;

        public void UpdateCarTilt(Transform t, float paraAcc, float perpAcc, float normAcc)
        {
            t.localPosition = new Vector3(
                perpMove.UpdateVal(perpAcc),
                normMove.UpdateVal(normAcc),
                paraMove.UpdateVal(normAcc));

            t.localRotation = Quaternion.Euler(
                paraTilt.UpdateVal(paraAcc),
                0, perpTilt.UpdateVal(perpAcc));
        }

        [System.Serializable]
        public class CarTiltAxis
        {
            private float val;
            [Range(0, 1)]
            public float lerp = 0.1f;
            public float scale, min = -50, max = 50;

            public float UpdateVal(float f)
            {
                val = Mathf.Lerp(val, f * scale, lerp);
                return Mathf.Clamp(val, min, max);
            }
        }
    }
}
