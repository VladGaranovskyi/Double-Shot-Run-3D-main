using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRotater : MonoBehaviour
{
    [SerializeField] private float k = 0.5f;
    [SerializeField] private ShootControls _shootControls;
    [SerializeField] private float n = 4f;
    [SerializeField] private float border = 135f;
    private Transform _spine;
    private Transform _pistol;

    private void Start()
    {
        _spine = _shootControls.GetSpine();
        _pistol = _shootControls.GetPistol();       
    }

    public void MoveSpine(float y)
    {
        _spine.eulerAngles = Vector3.right * Mathf.Clamp((y - (Screen.height / 2f)) / n, -border, border) * k /* *Time.deltaTime*/;
        _pistol.forward = new Vector3(0f, _pistol.forward.y, _pistol.forward.z);
    }

    public void LookAtPoint(Vector3 point)
    {
        _shootControls.GetSpine().forward = (point - _shootControls.GetSpine().position).normalized;
        _shootControls.GetPistol().forward = (point - _shootControls.GetPistol().position).normalized;
    }
}
