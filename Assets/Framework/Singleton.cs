using Unity.VisualScripting;
using UnityEngine;
namespace LikeSoulKnight
{
    #region  Singleton
    public class MonoSingleton<T> : AbstractMonoController where T : MonoSingleton<T>
    {
        protected static T mInstance;

        protected virtual void Awake()
        {
            if (mInstance != null)
            {
                Destroy(gameObject);
#if UNITY_EDITOR
                Debug.LogWarning("The singleton is generated multiple times");
#endif
                return;
            }
            mInstance = this as T;
        }
    }
    #endregion
}