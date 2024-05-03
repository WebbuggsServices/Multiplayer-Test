using Photon.Pun;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Transform attachPoint;

    private PlayerInput playerInput;

    private bool isPickedUp = false;
    private SpawnObject currentObject;
    PhotonView photonView;
    void Start()
    {
        photonView = PhotonView.Get(this);
        playerInput = FindObjectOfType<PlayerInput>();
        playerInput.OnInteract += Interact;
    }

    private void OnDisable()
    {
        playerInput.OnInteract -= Interact;
    }

    [PunRPC]
    private void Interact()
    {
        if(currentObject)
        {
            if (isPickedUp) Drop();
            else
            {
                ObjectSpawner.Instance.isPicked = false;
                PickUp();
            }
        }

        if(photonView.IsMine)
        UpdateOtherSide();
    }
    public void UpdateOtherSide()
    {
        photonView.RPC(nameof(Interact), RpcTarget.Others);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isPickedUp) return;

        var spawnObject = other.GetComponent<SpawnObject>();

        if (spawnObject)
        {
            spawnObject.OnHoverIn();
            currentObject = spawnObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPickedUp) return;

        var spawnObject = other.GetComponent<SpawnObject>();

        if (spawnObject)
        {
            spawnObject.OnHoverOut();
            currentObject = null;
        }
    }

    private void PickUp()
    {
        currentObject.GetComponent<Rigidbody>().isKinematic = true;
        currentObject.GetComponent<SpawnObject>().isMine = this.GetComponentInParent<PhotonView>().IsMine;
        currentObject.transform.localPosition = attachPoint.position;
        currentObject.transform.parent = attachPoint;
        currentObject.OnHoverOut();
        isPickedUp = true;
    }

    private void Drop()
    {
        currentObject.GetComponent<Rigidbody>().isKinematic = false;
        currentObject.transform.parent = null;

        isPickedUp = false;
        currentObject = null;
    }
}
