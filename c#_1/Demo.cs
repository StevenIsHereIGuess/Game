using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    // Start is called before the first frame update
    public int myAge = 13;

    private string myName = "Steven";
    string[] sentence;
    
    // Update is called once per frame
                                                    
   void Awake(){
        
        string[] sentence = {"This is my first atempt at a game", "This is my first platformer game", "Hello this is my first game"};
        for (int i = 0; i < sentence.Length; i++)
          {
            Debug.Log(sentence[i]);
          }
    

   }

    public void MyFunction(){

      Debug.Log("I have no idea what im doing"); //its is Debug.Log right? not Console.Write?

    }

  
}