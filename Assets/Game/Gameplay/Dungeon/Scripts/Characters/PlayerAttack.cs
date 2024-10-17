using System.Collections.Generic;

using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private LayerMask attackLayer;

    private Animator anim;
    private Camera mainCamera;
    private List<IDamagable> damagablesInRange;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        damagablesInRange = new List<IDamagable>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 20,attackLayer))
            {
                IDamagable damagable = hit.transform.GetComponent<IDamagable>();

                if (damagable != null )
                {
                    SimpleAttack(hit.transform.position);
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("Defense", true);
        }
        else
        {
            anim.SetBool("Defense", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagablesInRange.Add(damagable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();

        if (damagable != null && damagablesInRange.Contains(damagable))
        {
            damagablesInRange.Remove(damagable);
        }
    }

    private void SimpleAttack(Vector3 toLook)
    {   
        if (damagablesInRange.Count > 0)
        {
            transform.LookAt(toLook);
           
            damagablesInRange[0].Damage(10);
          
            anim.SetTrigger("SimpleAttack");
        }
    }

    private void StrongAttack()
    {
        anim.SetTrigger("StrongAttack");
    }
}
