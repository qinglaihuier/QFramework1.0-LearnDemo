using System;
using System.Collections;
using UnityEngine;
namespace LikeSoulKnight
{
    public class LifeCycle : MonoBehaviour
    {
        public float survivalTime = 8f;

        public Action onLifeEnd;

        WaitForSeconds waitForSeconds;

        private void Awake()
        {
            waitForSeconds = new WaitForSeconds(survivalTime);
        }

        private void OnEnable()
        {
            StartCoroutine(nameof(LifeEnd));
        }
        IEnumerator LifeEnd()
        {
            yield return waitForSeconds;
            onLifeEnd?.Invoke();
        }
        public void EndLifeAtOnce()
        {
            StopAllCoroutines();
            onLifeEnd?.Invoke();
        }
    }
}