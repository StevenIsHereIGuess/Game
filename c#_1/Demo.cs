using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    // Start is called before the first frame update
    public int myAge = 13;

    private string myName = "Steven";
    
    // Update is called once per frame
   void Start(){
     MyFunction();

   }

    public void MyFunction(){

      Debug.Log("I have no idea what im doing"); //its is Debug.Log right? not Console.Write?

    }
}