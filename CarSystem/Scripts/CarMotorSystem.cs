using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMotorSystem : MonoBehaviour
{
   public Rigidbody car;
   public WheelCollider[] wheelsC;
   public Transform[] wheelsT;
   public float motorTorque = 9000;
   public float maxSteer = 30;
   public float brakeForce = 10000;
   public CarBoxTramission boxTramission;
   public AudioSource audioCar;
   public Text velocityText;
   public Text tramissionText;


   float steer;
   public float torque;
   float direction;
   int change;
   float soundMath;
   public bool inAlt;
   public float KMH;
   public bool inRear;


   void UpdateWheelPose(WheelCollider collider, Transform transform)
   {
       Vector3 pos;
       Quaternion quat;

       collider.GetWorldPose(out pos, out quat);

       transform.position = pos;
       transform.rotation = quat;
   }

   void UpdateWheelPoses()
   {
       UpdateWheelPose(wheelsC[0],wheelsT[0]);
       UpdateWheelPose(wheelsC[1],wheelsT[1]);
       UpdateWheelPose(wheelsC[2],wheelsT[2]);
       UpdateWheelPose(wheelsC[3],wheelsT[3]);
   }

   void Steer()
   {
       steer = direction * Input.GetAxis("Horizontal");
       wheelsC[0].steerAngle = steer;
       wheelsC[1].steerAngle = steer;

       if(KMH >= 10 && (Input.GetAxis("Horizontal") <= -0.1f || Input.GetAxis("Horizontal") >= 0.1f))
       {
           direction = maxSteer - (KMH / 2);
       }
       else
       {
           direction = maxSteer;
       }
   }

   void Torque()
   {
       KMH = car.velocity.magnitude;
       car.angularDrag = KMH / 17;


       if(soundMath < boxTramission.maxPitch)
       {
           torque = (motorTorque * 10) * change * Input.GetAxis("Vertical");
           inAlt = false;
       }
       else
       {
           torque = 0;
           inAlt = true;
       }

       if((change == 0 && Input.GetAxis("Vertical") <= -0.1f) || (change > 1 && Input.GetAxis("Vertical") <= -0.1f) && inRear)
       {
           boxTramission.changeAc = 1;
       }

       wheelsC[0].motorTorque = torque;
       wheelsC[1].motorTorque = torque;
       wheelsC[2].motorTorque = torque;
       wheelsC[3].motorTorque = torque;

       for(int y = 0; y < wheelsC.Length; y++)
       {
           if(wheelsC[y].rpm <= -10 && Input.GetAxis("Vertical") <= -0.1f)
           {
               change = 1;
               inRear = true;
           }
           else
           {
               inRear = false;
           }
       }
   }

   void Brake()
   {
       if(((Input.GetAxis("Vertical") < -0.1f && wheelsC[3].rpm > 0) || (Input.GetAxis("Vertical") > 0.1f && wheelsC[3].rpm < 0)) || inAlt || Input.GetAxis("Vertical") == 0)
        {
             for(int t = 0; t < wheelsC.Length; t++)
             {
                 wheelsC[t].brakeTorque = brakeForce * change;
             }
             Debug.LogWarning("TRAVANDO!");
        }
        else
        {
            Debug.Log("DESTRAVANDO!");
             for(int t = 0; t < wheelsC.Length; t++)
             {
                 wheelsC[t].brakeTorque = 0;
                 //wheelsC[t].motorTorque = 100; 
             }
        }
   }

   void SoundMotor()
   {
       audioCar.pitch = soundMath;
       change = boxTramission.changeAc;

       if(change == 0)
       {
           soundMath = 0.3f + Input.GetAxis("Vertical") * 1;
       }
       else
       {
           soundMath = 0.3f + ((KMH / 2) * Time.deltaTime * 40) / boxTramission.sound[change];
       }
   }

   void Tramission()
   {
       if(!boxTramission.manual)
       {
           if(KMH <= boxTramission.maxSpeed[1])
           {
               boxTramission.changeAc = 1;
           }
           if(KMH <= boxTramission.maxSpeed[2] && KMH > boxTramission.maxSpeed[1])
           {
               boxTramission.changeAc = 2;
           }
           if(KMH <= boxTramission.maxSpeed[3] && KMH > boxTramission.maxSpeed[2])
           {
               boxTramission.changeAc = 3;
           }
           if(KMH <= boxTramission.maxSpeed[4] && KMH > boxTramission.maxSpeed[3])
           {
               boxTramission.changeAc = 4;
           }
           if(KMH <= boxTramission.maxSpeed[5] && KMH > boxTramission.maxSpeed[4])
           {
               boxTramission.changeAc = 5;
           }
       }
   }

   void CarPainel()
   {
       velocityText.text = " "  + KMH.ToString("f0");


       if(change == 0)
       {
           tramissionText.text = "N";
       }
       else
       {
           if(inRear)
           {
               tramissionText.text = "R";
           }
           else
           {
               tramissionText.text = " " + change;
           }
       }
   }

   private void Update() 
   {
       UpdateWheelPoses();
       Brake();
       CarPainel();
       Tramission();
   }

   private void FixedUpdate()
   {
       Steer();
       Torque();
       SoundMotor();
   }
}
