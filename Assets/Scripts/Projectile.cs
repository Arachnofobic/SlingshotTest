using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
     bool isDragging;
     float shotDelay;
     float maxDrag = 5f;
     Rigidbody2D rb;
     Rigidbody2D slingrb;
     SpringJoint2D sj;
     LineRenderer lr;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sj = GetComponent<SpringJoint2D>();
        slingrb = sj.connectedBody;
        shotDelay = 1 / (sj.frequency * 4);
        lr = GetComponent<LineRenderer>();
    }

    
    void Update()
    {
        if (isDragging)
        {
            DragProjectile();

        }
        
    }

    void FixedUpdate()
    {
        Invoke("CheckProjectileMovement",2);
    }

    void DragProjectile()
    {
        LineRendererPosition();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint((Input.mousePosition));
        
        float distance = Vector2.Distance(mousePosition, slingrb.position);

        if (distance > maxDrag)
        {
            Vector2 direction = (mousePosition - slingrb.position).normalized;
            rb.position = slingrb.position + direction * maxDrag;

        }
        else
        {
            rb.position = mousePosition;
        }
    }

    void LineRendererPosition()
    {
        Vector3[] positions=new Vector3[2];
        positions[0] = rb.position;
        positions[1] = slingrb.position;
        lr.SetPositions(positions);

    }

    void OnMouseDown()
    {
        isDragging = true;
        rb.isKinematic = true;
        ;


    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.isKinematic = false;
        StartCoroutine(Shot());


    }

    IEnumerator Shot()
    {
        yield return new WaitForSeconds(shotDelay);
        sj.enabled = false;


    }

    void CheckProjectileMovement()
    {
        if (rb.IsSleeping())
        {
            Debug.Log("Not moving. Spawn new projectile to shoot");
        }
        
    }
}
