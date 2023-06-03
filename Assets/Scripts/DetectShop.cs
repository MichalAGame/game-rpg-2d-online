using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectShop : MonoBehaviour
{
    public bool shopDetected;
    public GameObject shopUI;
    public GameObject questIcon;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shopDetected == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                shopUI.SetActive(true);
            }
            else
            {
                shopUI.SetActive(false);
            }
        }
    }

        private void OnTriggerStay2D(Collider2D other)
        {
        if(other.tag == ("Shop"))
            {
                shopDetected = true;
                questIcon.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == ("Shop"))
            {
                shopDetected = false;
                questIcon.SetActive(false);
            }
        }
    
}
