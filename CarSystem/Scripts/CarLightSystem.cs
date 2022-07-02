using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLightSystem : MonoBehaviour
{
    public GameObject frontLightLow;
    public GameObject frontLightMediuns;
    public GameObject frontLightMax;
    public GameObject brakeLights;
    [Space(15)]
    public KeyCode activeLight = KeyCode.L;
    public KeyCode maxLight = KeyCode.K;
    public KeyCode signLight = KeyCode.J;
    [Space(15)]
    public GameObject leftSignLight;
    public GameObject rightSignLight;
    public GameObject rearSignLight;
    public KeyCode leftSign = KeyCode.LeftBracket;
    public KeyCode rightSign = KeyCode.RightBracket;
    public CarMotorSystem car;
    public bool MedeasComoBase = true;


    bool low;
    bool Medium;
    bool Max;
    bool brake;
    bool left;
    bool right;
    bool rear;
    bool wait;


    void ApplyLights()
    {
        //Low
        frontLightLow.SetActive(low);
        //Max
        frontLightMax.SetActive(Max);
        //Medium
        frontLightMediuns.SetActive(Medium);
        //Brake
        brakeLights.SetActive(brake);
    }

    void SignLights()
    {
        //Left Sign
        leftSignLight.SetActive(left);
        //Rigth Sign
        rightSignLight.SetActive(right);
        //Rear Sign
        rearSignLight.SetActive(rear);
    }

    void ControComand()
    {
        if(Input.GetKeyUp(leftSign))
        {
            left = !left;
        }

        if(Input.GetKeyUp(rightSign))
        {
            right = !right;
        }

        if(Input.GetKeyUp(activeLight))
        {
            if(low)
            {
                if(low && Medium)
                {
                    low = false;
                    Medium = false;
                }
                else
                {
                    Medium = true;
                }
            }
            else
            {
                low = true;
            }
        }

        if(!Medium && MedeasComoBase)
        {
            Max = false;
        }

        if(Input.GetKeyUp(maxLight))
        {
            Max = !Max;
        }

        if(Input.GetAxis("Vertical") <= -0.1f && !car.inRear)
        {
            brake = true;
        }
        else
        {
            brake = false;
        }
        rear = car.inRear;
    }

    private void Update() 
    {
        SignLights();
        ApplyLights();
        ControComand();    
    }
}
