using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class ChangeDirection : MonoBehaviour
{
    float cooldown = 0.75f;
    float cooldown_timer;
    bool can_flip = true;
    private XRController controller = null; 
    void Start()
    {
        controller = GetComponent<XRController>();
        cooldown_timer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {

        if(controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 dir))
        {

            if (cooldown_timer < cooldown)
            {
                cooldown_timer += Time.deltaTime;            
            }
            else
            {
                can_flip = true;
                cooldown_timer = cooldown;

            }
                
            if(dir.x >= 0.5f && can_flip)
            {
                gameObject.transform.parent.RotateAround(new Vector3(transform.position.x, 0, transform.position.z), Vector3.up, 90.0f);
                GameManager.instance.FadeIn(0.0f);
                GameManager.instance.FadeOut(1.0f);
                can_flip = false;
                cooldown_timer = 0;

            }else if(dir.x <= -0.5f && can_flip)
            {
                gameObject.transform.parent.RotateAround(new Vector3(transform.position.x, 0, transform.position.z), Vector3.up, -90.0f);
                GameManager.instance.FadeIn(0.0f);
                GameManager.instance.FadeOut(1.0f);
                can_flip = false;
                cooldown_timer = 0;
            }
        }

       
    }
}
