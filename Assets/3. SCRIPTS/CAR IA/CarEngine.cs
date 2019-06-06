using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{

    public Transform path;
    public WheelCollider roueAvantGauche;
    public WheelCollider roueAvantDroite;
    public WheelCollider roueArriereGauche;
    public WheelCollider roueArriereDroite;


    public float maxSteerAngle = 45f;
    public float turnSpeed = 5f;
    public float speed = 80f;
    public float maxBrakeTorque = 150f;
    public float rangeChekpoint = 1f;
    public float currentSpeed;
    public float maxSpeed = 100f;
    public Vector3 centerOfMass;
    public bool isBraking = false;

    [Header("Sensors")]
    public float sensorLenght = 5f;
    public Vector3 frontSensorPosition = new Vector3 ( 0, 0.2f, 0.5f) ;
    public float frontSideSensorPosition = 0.2f;
    public float frontSensorAngle = 30f;

    /*
      si la voiture a des feux arriere qui s'allume quand elle freine

        public Texture2D textureNormal;
        public Texture2D textureBraking;
        public Renderer carRenderer;
   
    */

    private List<Transform> nodes;
    private int currentNode = 0;
    private bool avoiding = false;
    private float targetSteerAngle = 0;

    [Header("Pièges")]
    [Header("Bumper")]
    public float jumpForce = 10f;
    private Rigidbody rb;

    [Header("Slower")]
    public float vitesseBridee = 20f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;

        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    private void FixedUpdate()
    {
        Sensors();
        ApplySteer();
        Drive();
        CheckWaypointDistance();
        Braking();
        LerpToSteerAngle();


    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;
        float avoidMultiplier = 0;
        avoiding = false;



        // front right sensor
        sensorStartPos += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLenght))
        {
            //if (!hit.collider.CompareTag("Terrain"))
            //{
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 1f;
            //}
        }

        // front right angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLenght))
        {
            //if (!hit.collider.CompareTag("Terrain"))
            //{
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 0.5f;
            //}
        }

        // front left sensor
        sensorStartPos -= transform.right * frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLenght))
        {
            //if (!hit.collider.CompareTag("Terrain"))
            //{
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 1f;
            //}
        }

        // front left angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLenght))
        {
            //if (!hit.collider.CompareTag("Terrain"))
            //{
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 0.5f;
            //}
        }

        // front center sensor
        if ( avoidMultiplier == 0)
        {
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLenght))
        {
            //if (!hit.collider.CompareTag("Terrain"))
            //{
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                    if(hit.normal.x < 0)
                    {
                        avoidMultiplier = -1;
                    }
                    else
                    {
                        avoidMultiplier = 1;
                    }
            //}
        }
        }

        if (avoiding)
        {
            targetSteerAngle = maxSteerAngle * avoidMultiplier;
        }
    }

    private void ApplySteer()
    {
        if (avoiding) return;
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        targetSteerAngle = newSteer;
    }
    private void Drive ()
    {
        currentSpeed = 2 * Mathf.PI * roueAvantDroite.radius * roueAvantDroite.rpm * 60 / 1000;

        if(currentSpeed < maxSpeed && !isBraking)
        {
            roueAvantGauche.motorTorque = speed;
            roueAvantDroite.motorTorque = speed;
        }
        else
        {
            roueAvantGauche.motorTorque = 0;
            roueAvantDroite.motorTorque = 0;
        }
       
    }

    private void CheckWaypointDistance()
    {
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < rangeChekpoint)
        {
            if(currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }

    private void Braking()
    {
        
         if (isBraking) 
         {
            roueArriereGauche.brakeTorque = maxBrakeTorque;
            roueArriereDroite.brakeTorque = maxBrakeTorque;

            //carRenderer.material.mainTexture = textureBraking;
        }
         else
         {
            roueArriereGauche.brakeTorque = 0;
            roueArriereDroite.brakeTorque = 0;

            //carRenderer.material.mainTexture = textureNormal;
        }
         
    }
    private void LerpToSteerAngle()
    {
        roueAvantGauche.steerAngle = Mathf.Lerp(roueAvantGauche.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        roueAvantDroite.steerAngle = Mathf.Lerp(roueAvantDroite.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }



    public void BumperContact()
    {
        Debug.Log("HEy");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void Slower()
    {
        Debug.Log("HO");
        maxSpeed = vitesseBridee;
        maxBrakeTorque = 50000f;
    }

    public void UnSlower()
    {
        // Remetre les valeur qui on été selectionner sur l'inspector
        maxSpeed = 300f;
        maxBrakeTorque = 50f;
    }
}
