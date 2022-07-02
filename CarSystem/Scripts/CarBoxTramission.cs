using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBoxTramission : MonoBehaviour
{
   public TramissionBase typeOfTramission = TramissionBase.manual;
   public float maxPitch;
   public float[] sound;
   public float[] maxSpeed;
   public int changeAc;
   public KeyCode addChange = KeyCode.RightShift;
   public KeyCode reduceChange = KeyCode.RightControl;

   public bool manual;
   bool done;
   bool wait;
   private void Start() {
       if(typeOfTramission == TramissionBase.manual)
       {
           manual = true;
       }
       else
       {
           manual = false;
       }
   }

    void Tramission()
    {
        if(manual)
        {
            if(Input.GetKey(addChange) && !done && !wait)
            {
                StartCoroutine("addOrReduce", 1);
                done = true;
            }

            if(Input.GetKey(reduceChange) && !done && !wait)
            {
                StartCoroutine("addOrReduce", -1);
                done = true;
            }

            if(changeAc <= 0)
            {
                changeAc = 0;
            }
            if(changeAc >= maxSpeed.Length - 1)
            {
                changeAc = maxSpeed.Length - 1;
            }
        }
    }

    void Update()
    {
        Tramission();
    }

    IEnumerator addOrReduce(int value)
    {
        yield return new WaitForSeconds(0.1f);
        if(value < 0 && done)
        {
            changeAc--;
            done = false;
            wait = true;
        }
        if(value > 0 && done)
        {
            changeAc++;
            done = false;
            wait = true;
        }
        yield return new WaitForSeconds(1f);
        wait = false;
        value = 0;
    }

   public enum TramissionBase
   {
       manual,
       automatic
   };
}
