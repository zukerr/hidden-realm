using UnityEngine;
using System.Collections;
using Mirror;

public class PlayerMovement : NetworkBehaviour {

	Rigidbody2D rbody;
	Animator anim;

    public float speed;

	// Use this for initialization
	void Start () {

		rbody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
        if(hasAuthority)
            GameObject.Find("MainCamera").GetComponent<CameraController>().CamTarget = transform;
        SetupPhysics();
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(hasAuthority)
        {
            LiveMovement();
        }
	}

    private void SetupPhysics()
    {
        if(!isServer)
        {
            if(!isLocalPlayer)
            {
                rbody.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    private void LiveMovement()
    {
        //Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 movement_vector = Vector2.zero;
        if ((Input.GetKey(KeyCode.W)) && (GetComponent<PlayerUI>().myUI.ChatGO.GetComponent<ChatHandler>().mode == 0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            movement_vector = mousePos.normalized;
        }

        if (movement_vector != Vector2.zero)
        {
            anim.SetBool("iswalking", true);
            GetComponent<PlayerRpg>().isMoving = true;
            anim.SetFloat("input_x", movement_vector.x);
            anim.SetFloat("input_y", movement_vector.y);

            if (movement_vector.x > 0f)
            {
                anim.SetBool("last_dir_right", true);
            }

            if (movement_vector.x < 0f)
            {
                anim.SetBool("last_dir_right", false);
            }
        }
        else
        {
            anim.SetBool("iswalking", false);
            GetComponent<PlayerRpg>().isMoving = false;
        }


        rbody.MovePosition(rbody.position + movement_vector * Time.fixedDeltaTime * speed);
    }
}
