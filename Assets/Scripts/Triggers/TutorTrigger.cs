using System.Collections;
using UnityEngine;

public class TutorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _tutorUI;
    [SerializeField] private float _time;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(TutorCor());
    }

    private IEnumerator TutorCor()
    {
        _tutorUI.SetActive(true);
        yield return new WaitForSeconds(_time);
        _tutorUI.SetActive(false);
    }
}
