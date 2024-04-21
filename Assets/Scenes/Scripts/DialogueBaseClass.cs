using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem{
    public class DialogueBaseClass : MonoBehaviour
    {
        private IEnumerator WriteText(string text, TMPro.TextMeshProUGUI textLabel, float time){
            textLabel.text = "";
            foreach(char c in text){
                textLabel.text += c;
                yield return new WaitForSeconds(time);
            }
        }

        private void Start(){
            
            StartCoroutine(WriteText("Just graduated from Knight Academy and I am ready for my first Adventure to save the weak!", GetComponent<TMPro.TextMeshProUGUI>(), 0.08f));
        }



            
    }

}

