using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectMover : MonoBehaviourPun, IPunObservable // Bunu yazdigimizda scripti Photon View icindeki observera da eklemek gerekiyor calismasi icin
{
    private Animator anim;
    [SerializeField] private float moveSpeed = 1.0f;

    // Photon Transform View Islemi Yapiyor

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if(stream.IsReading) 
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
        */
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(base.photonView.IsMine)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            transform.position += new Vector3(x, y, 0) * Time.deltaTime * moveSpeed;
            UpdateMovingBoolean(x != 0 || y != 0);
        }
    }

    private void UpdateMovingBoolean(bool moving)
    {
        //anim.SetBool("Moving", moving);
    }
}
