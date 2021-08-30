using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class CarController : MonoBehaviour
{
    [Header("Basic Attributes")]
    public float accelaration;
    public float turnForce, backupTurnForce, airTurnForce, brakeForce,
        jumpForce;
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
    [Range(0, 2)]
    public float driftParticleIntensity;
    public int maxparticlesPerFrame = 50;

    [Header("Noises")]
    public AudioSource driveSfx;
    public AudioSource driftSfx;

    [Range(0, 1)]
    public float driveSoundUpLerp, driveSoundDownLerp,
        driftSoundUpLerp, driftSoundDownLerp;

    private float driveVol = 0, maxDriveVol = .2f,
        driftVol = 0, MaxDriftVol = .3f;

    public CarTilt tiltControls;



    private Transform body, frame, wheelParent;
    private Transform[] wheels;

    private ParticleSystem[] driftParticles;

    private Rigidbody rb;
    private Vector3 surfaceNormal,
        previousVelocity, deltaV;

    private TrickHandler trickHandler;

    // controls
    private float gas, turn, brake, rotation;

    private float groundThreshold, tireRot, fTireRot;
    private bool tricks = false, jump;
    public static bool ground;

    private float groundTime;


    //private Vector3 debugVec1, debugVec2, debugVec3;

    //private void OnDrawGizmos()
    //{
    //    if (!body)
    //        return;
    //    Gizmos.matrix = Matrix4x4.Translate(body.position);
    //    Gizmos.color = new Color(1, 0, 0);
    //    DrawGizmoArrow(debugVec1);
    //    Gizmos.color = new Color(0, 1, 0);
    //    DrawGizmoArrow(debugVec2);
    //    Gizmos.color = new Color(0, 0, 1);
    //    DrawGizmoArrow(debugVec3);
    //}

    //private void DrawGizmoArrow(Vector3 tip)
    //{
    //    Vector3 tail = Vector3.zero;
    //    float headWidth = tip.magnitude / 8;
    //    Gizmos.DrawLine(tip, tail);
    //    tail = tip * .75f;
    //    Gizmos.DrawLine(tip, tail + Vector3.left * headWidth);
    //    Gizmos.DrawLine(tip, tail + Vector3.right * headWidth);
    //}

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

        Transform particleParent = body.GetChild(2);
        driftParticles = particleParent.GetChild(0).GetComponentsInChildren<ParticleSystem>();

        trickHandler = GetComponent<TrickHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateControls();
    }

    private void FixedUpdate()
    {
        UpdatePositions();
        Turn();
        RunWheels();
        RunFrame();
        previousVelocity = rb.velocity;
        if (ground)
        {
            ApplyFriction();
            rb.AddForce(body.forward * gas * accelaration);
            rb.AddForce(-Physics.gravity * 0.8f);
            if (jump)
            {
                jump = false;
                rb.AddForce(body.up * jumpForce, ForceMode.Impulse);
            }
        }
        else gas = 0;

        // run drive sound
        float lerp = driveSoundUpLerp;
        float val = gas * maxDriveVol;
        if (val < driveVol)
            lerp = driveSoundDownLerp;
        driveVol = Mathf.Lerp(driveVol, val, lerp);
        driveSfx.volume = driveVol;
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
        jump = false;
        Gamepad g = Gamepad.current;
        if (g != null)
        {
            turn = g.leftStick.x.ReadValue();
            turnDecay = turn == 0;
            gas = g.rightTrigger.ReadValue();
            gasDecay = gas == 0;
            brake = g.leftTrigger.ReadValue();
            brakeDecay = brake == 0;

            jump |= g.aButton.wasPressedThisFrame;
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

        jump |= k.spaceKey.wasPressedThisFrame;
    }

    private float CleanInput(float f)
    {
        return Mathf.Clamp(f, -1, 1);
    }

    private void Turn()
    {
        if (ground)
        {
            float v = Vector3.Dot(rb.velocity, body.forward);
            float diff = turn * v * Time.fixedDeltaTime;
            if (v > 0)
                diff *= turnForce;
            else
                diff *= backupTurnForce;
            rotation = LoopAngle(rotation + diff);
        }
        else
        {
            rotation = LoopAngle(rotation + turn * airTurnForce);
        }
    }

    private void UpdatePositions()
    {
        body.position = rb.position;
        ground = false;

        RaycastHit rayHit;
        var hit = Physics.Raycast(rb.position + body.forward * 1.25f, 
            -body.up, out rayHit);
        ground = hit && rayHit.distance < groundThreshold;
        if (ground) surfaceNormal = rayHit.normal;
        else
        {
            hit = Physics.Raycast(rb.position, Vector3.down, out rayHit);
            ground = hit && rayHit.distance < groundThreshold;
            if (ground) surfaceNormal = rayHit.normal;
            else
            {
                hit = Physics.Raycast(rb.position, -body.up, out rayHit);
                ground = hit && rayHit.distance < groundThreshold;
                if (ground) surfaceNormal = rayHit.normal;
            }
        }

        body.up = Vector3.Lerp(body.up, surfaceNormal, .2f);
        body.RotateAround(body.position, body.up, rotation);

        if (ground)
            groundTime = Time.time;

        if(!ground && !tricks && Time.time > groundTime + .25f)
        {
            tricks = true;
            trickHandler.StartTricks();
        }else if(ground && tricks)
        {
            tricks = false;
            trickHandler.EndTricks();
        }
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

        if (ground)
        {
            RunParticles(parAcc, perpAcc, normAcc);

            //run drift sound
            float lerp = driftSoundUpLerp;
            float val = Mathf.Abs(perpAcc / 10) * MaxDriftVol;
            if (val < driveVol)
                lerp = driftSoundDownLerp;
            driftVol = Mathf.Lerp(driftVol, val, lerp);
            driftSfx.volume = driftVol;
        }
        else
        {
            driftVol = 0;
            driftSfx.volume = driftVol;
        }
    }

    private void RunParticles(float para, float perp, float norm)
    {
        foreach(ParticleSystem p in driftParticles)
        {
            if (perp < 0)
                p.transform.localRotation = Quaternion.Euler(Vector3.up * 90);
            else
                p.transform.localRotation = Quaternion.Euler(Vector3.down * 90);
        }
        int n = (int)(Mathf.Pow(perp, 2));
        if (n > 1)
            n = (int)(n * driftParticleIntensity);
        n = Mathf.Clamp(n, 0, maxparticlesPerFrame);
        driftParticles[0].Emit(n);
        driftParticles[1].Emit(n);

        n = (int)(Mathf.Pow(perp / 2, 2));
        if (n > 1)
            n = (int)(n * driftParticleIntensity);
        n = Mathf.Clamp(n, 0, maxparticlesPerFrame);
        driftParticles[2].Emit(n);
        driftParticles[3].Emit(n);
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
