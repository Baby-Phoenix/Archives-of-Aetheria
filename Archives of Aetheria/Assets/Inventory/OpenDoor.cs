using UnityEngine;
using UnityEngine.UI;
public class OpenDoor : MonoBehaviour
{
    public float radius = 3f;
    public Player p;
    bool isInRange = false;
    private Transform player;
    private GameObject gate;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        gate = GameObject.FindWithTag("Gate");
    }


    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if(distance<= radius)
        {
            Interact();
        }
    }
    public void InRange(Transform playerTransform)
    {
        isInRange = true;
        player = playerTransform;
    }
    public void OutRange()
    {
        isInRange = false;
        player = null;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    public virtual void Interact()
    {
        gate.GetComponent<Animator>().SetBool("openGate", true);
    }
}
