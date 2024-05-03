using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private CharacterController cc;
    private PlayerInput input;
    PhotonView photonView;

    public Material mineMat, enemyMat;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            this.transform.GetChild(0).GetComponent<MeshRenderer>().material = mineMat;
            cc = GetComponent<CharacterController>();
            input = FindObjectOfType<PlayerInput>();

            input.OnMove += Input_OnMove;
        }
        else
        {
            this.transform.GetChild(0).GetComponent<MeshRenderer>().material = enemyMat;
            //this.enabled = false;
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void Input_OnMove(Vector3 val)
    {
        if (photonView.IsMine)
        {
            cc.Move(val * Time.deltaTime * moveSpeed);

            if (cc.velocity.sqrMagnitude > 0)
                transform.rotation = Quaternion.LookRotation(val);
        }
    }

    private void OnDisable()
    {
        if (photonView.IsMine)
            input.OnMove -= Input_OnMove;
    }
}
