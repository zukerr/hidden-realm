using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LookAtMouse : NetworkBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(hasAuthority)
        {
            if(GetComponent<PlayerRpg>().isMoving)
            {
                Rotate();
            }
        }
    }

    private void Rotate()
    {
        //rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
