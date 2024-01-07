using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShootControls : MonoBehaviour
{
    [SerializeField] private Transform _spine;
    [SerializeField] private List<ParticleSystem> shootEffects = new List<ParticleSystem>();
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private LayerMask _bulletLayer;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private Transform _pistol;
    public LineRenderer lineRenderer { get; private set; }
    public Holder holder { get; private set; }
    public LaserProjector laserProjector { get; private set; }
    public BodyRotater bodyRotater { get; private set; }
    public Shooter shooter { get; private set; }
    public LayerMask BulletLayer { get => _bulletLayer; }
    public LayerMask TargetLayer { get => _targetLayer; }

    private void OnEnable()
    {
        laserProjector = GetComponent<LaserProjector>();
        bodyRotater = GetComponent<BodyRotater>();
        shooter = GetComponent<Shooter>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public RaycastHit GetHitFromPistol()
    {
        RaycastHit hit;
        Ray ray = new Ray(_shootPoint.position, _shootPoint.forward);
        Physics.Raycast(ray, out hit, Mathf.Infinity, _bulletLayer);
        return hit;
    }

    public RaycastHit GetHitFromPistol(LayerMask layer)
    {
        RaycastHit[] hits;
        Ray ray = new Ray(_shootPoint.position, _shootPoint.forward);
        hits = Physics.RaycastAll(ray, Mathf.Infinity, layer);
        for(int i = hits.Length - 1; i >= 0; i--)
        {
            if (hits[i].collider.gameObject.GetComponent<EnemyBone>() == null && hits[i].collider.gameObject.GetComponent<BulletPropagator>() == null && hits[i].collider.tag != "Border")
            {
                return hits[i];
            }
        }
        return hits[0];
    }

    public RaycastHit[] GetHitsFromPistol()
    {
        RaycastHit[] rh = new RaycastHit[2];
        RaycastHit hit = GetHitFromPistol();
        rh[0] = hit;
        RaycastHit newHit;
        Ray ray = new Ray(hit.point, Vector3.Reflect((hit.point - _shootPoint.position).normalized, hit.normal));
        Physics.Raycast(ray, out newHit, Mathf.Infinity, _bulletLayer);
        rh[1] = newHit;
        return rh;
    }

    public RaycastHit[] GetHitsFromPistol(LayerMask layer)
    {
        RaycastHit[] rh = new RaycastHit[2];
        RaycastHit hit = GetHitFromPistol(layer);
        rh[0] = hit;
        RaycastHit newHit;
        Ray ray = new Ray(hit.point, Vector3.Reflect((hit.point - _shootPoint.position).normalized, hit.normal));
        Physics.Raycast(ray, out newHit, Mathf.Infinity, layer);
        rh[1] = newHit;
        return rh;
    }

    public Transform GetShootPoint() { return _shootPoint; }
    public Transform GetSpine() { return _spine; }
    public Transform GetPistol() { return _pistol; }
    public List<ParticleSystem> GetEffects() { return shootEffects; }
}
