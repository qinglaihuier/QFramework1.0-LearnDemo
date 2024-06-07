using UnityEditor.Callbacks;
using UnityEngine;
namespace LikeSoulKnight
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(LifeCycle))]
    public class Bullet : AbstractMonoObjectInObjectPool, IBelongToObjecPool
    {
        private Rigidbody2D mRb;

        private LifeCycle mLifeCycle;
        private void Awake()
        {
            mRb = GetComponent<Rigidbody2D>();
            mRb.velocity = transform.right * 10f;
            GetComponent<LifeCycle>().onLifeEnd += BackToObjectPool;
            mLifeCycle = GetComponent<LifeCycle>();
        }
        public override void OnReset()
        {

        }
        public void SetVelocity(Vector2 direction)
        {
            mRb.velocity = direction.normalized * 10f;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
            }
            if (other.CompareTag("Ground"))
            {
                mLifeCycle.EndLifeAtOnce();
            }
        }

    }
}