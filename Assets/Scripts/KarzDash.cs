using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarzDash : MonoBehaviour
{
    public GameObject DashIcon;

    private MovementController mc;

    private void Start()
    {
        GameManager.Instance.OnPhaseChange += StartDash;
        mc = GetComponentInChildren<MovementController>();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPhaseChange -= StartDash;
    }

    private void StartDash(PhaseState state)
    {
        if (state == PhaseState.KarzakovConflag)
        {
            Debug.Log(mc.moveSpeed);
            mc.moveSpeed = mc.moveSpeed + 4;
            Debug.Log(mc.moveSpeed);
            DashIcon.SetActive(true);
        }
    }
}
