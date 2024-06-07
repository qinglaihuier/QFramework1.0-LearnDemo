using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LikeSoulKnight;
using QFramework;
using UnityEngine;
namespace LikeSoulKnight
{
    public class ObjectPool : MonoSingleton<ObjectPool>
    {
        Dictionary<string, Queue<GameObject>> belongToObjecPools = new Dictionary<string, Queue<GameObject>>();

        //是否限制最大数量
        //缓存？  字符串常量池子?

        const string PREFABS_PATH = "Prefabs/";

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 从对象池中获取游戏场景中对象
        /// </summary>
        /// <param name="path">游戏对象对应预制体Resources/Prefabs文件夹下的路径</param>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns></returns>
        public static GameObject GetMonoGameObject(string path)
        {
            if (mInstance.belongToObjecPools.ContainsKey(path) == false)
            {
                mInstance.belongToObjecPools.Add(path, new Queue<GameObject>());
            }

            Queue<GameObject> queue = mInstance.belongToObjecPools[path];

            if (queue == null)
            {
                queue = new Queue<GameObject>();
            }

            if (queue.Count == 0)
            {
                var newObj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(PREFABS_PATH + path));

                string newObjName = newObj.gameObject.name;
                newObjName = newObjName.Replace("(Clone)", "");

                string parentName = string.Intern(newObjName + "Parent");

                Transform parent = mInstance.transform.Find(parentName);

                newObj.gameObject.name = path;

                if (parent == null)
                {
                    parent = new GameObject(parentName).transform;
                    parent.name = parentName;
                    parent.parent = mInstance.transform;
                }
                newObj.transform.parent = parent;
                queue.Enqueue(newObj);
            }

            var obj = queue.Dequeue();

            IBelongToObjecPool belongToObjecPool = obj.GetComponent<IBelongToObjecPool>();

            belongToObjecPool.Reset();

            if (belongToObjecPool.OnBackToObjectPool == null)
            {
                belongToObjecPool.OnBackToObjectPool = GoBackPool;
            }

            return obj;
        }
        private static void GoBackPool(GameObject @gameObject)
        {
            string name = @gameObject.name;
            if (mInstance.belongToObjecPools.TryGetValue(name, out Queue<GameObject> queue))
            {
                queue.Enqueue(@gameObject);
            }
            else
            {
                mInstance.belongToObjecPools.Add(name, new Queue<GameObject>());
                mInstance.belongToObjecPools[name].Enqueue(@gameObject);
            }
        }
    }

    public abstract class AbstractMonoObjectInObjectPool : AbstractMonoController, IBelongToObjecPool
    {
        public Action<GameObject> OnBackToObjectPool { get; set; }
        public void BackToObjectPool()
        {
            gameObject.SetActive(false);
            OnBackToObjectPool(gameObject);
        }

        /// <summary>
        /// 传入对象池引用
        /// </summary>
        /// <param name="objectPool"></param>
        void IBelongToObjecPool.Reset()
        {
            gameObject.SetActive(true);
            OnReset();
        }
        public abstract void OnReset();
    }

    public interface IBelongToObjecPool
    {
        public Action<GameObject> OnBackToObjectPool { get; set; }

        /// <summary>
        /// 由自己决定的属性的重置
        /// </summary>
        public void Reset();

        public void BackToObjectPool();
    }
    // public interface IObjectPool
    // {
    //     public GameObject GetMonoGameObject(string path);
    //     public void GoBackPool(GameObject @gameObject);

    // }
}
