/****************************************************************************
 * Copyright (c) 2015 ~ 2022 liangxiegame MIT License
 *
 * QFramework v1.0
 *
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * 
 * Author:
 *  liangxie
 *  王二 soso
 * Contributor
 *  TastSong
 * 
 * Community
 *  QQ Group: 623597263
 ****************************************************************************/
using System;
using System.Collections.Generic;
using LikeSoulKnight;
using Mono.Cecil;
using UnityEngine;

namespace QFramework
{
    #region Architecture
    public interface IArchitecture
    {
        public void RegisterSystem<T>(T system) where T : ISystem;

        public void RegisterModel<T>(T model) where T : IModel;

        public void RegisterUtility<T>(T utility) where T : IUtility;

        public T GetSystem<T>() where T : class, ISystem;

        public T GetModel<T>() where T : class, IModel;

        public T GetUtility<T>() where T : class, IUtility;

        public void SendCommand<T>() where T : ICommand, new();

        public void SendCommand<T>(T command) where T : ICommand;

        public IUnRegister RegisterEvent<T>(Action<T> onEvent);

        public void SendEvent<T>() where T : new();

        public void SendEvent<T>(T e);

        public void UnRegisterEvent<T>(Action<T> onEvent);

        public T SendQuery<T>(IQuery<T> query);
    }
    abstract public class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        private static T mArchitecture = null;

        public static IArchitecture Interface
        {
            get
            {
                if (mArchitecture == null)
                {
                    MakeSureArchitecture();
                }
                return mArchitecture;
            }
        }

        private bool isInited = false;

        private List<IModel> mModels = new List<IModel>();
        private List<ISystem> mSystems = new List<ISystem>();

        public static Action<T> OnRegisterPatch = delegate { };

        public static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();
                mArchitecture.Init();

                OnRegisterPatch?.Invoke(mArchitecture);

                foreach (var model in mArchitecture.mModels)
                {
                    model.Init();
                }
                foreach (var system in mArchitecture.mSystems)
                {
                    system.Init();
                }

                mArchitecture.mModels.Clear();
                mArchitecture.mSystems.Clear();
                mArchitecture.isInited = true;
            }
        }

        private IOCContainer mContainer = new IOCContainer();

        private ITypeEventSystem mTypeEventSystem = new TypeEventSystem();

        protected abstract void Init();


        public void RegisterModel<modelT>(modelT model) where modelT : IModel
        {
            model.SetArchitecture(this);

            mContainer.Register<modelT>(model);


            if (isInited)
            {
                model.Init();
            }
            else
            {
                mModels.Add(model);
            }
        }
        public void RegisterSystem<T1>(T1 system) where T1 : ISystem
        {
            system.SetArchitecture(this);

            mContainer.Register<T1>(system);


            if (isInited)
            {
                system.Init();
            }
            else
            {
                mSystems.Add(system);
            }
        }
        public void RegisterUtility<T1>(T1 utility) where T1 : IUtility
        {
            mContainer.Register<T1>(utility);
        }
        public T1 GetSystem<T1>() where T1 : class, ISystem
        {
            return mContainer.Get<T1>();
        }
        public T1 GetModel<T1>() where T1 : class, IModel
        {
            return mContainer.Get<T1>();
        }
        public T1 GetUtility<T1>() where T1 : class, IUtility
        {
            return mContainer.Get<T1>();
        }

        public void SendCommand<T1>() where T1 : ICommand, new()
        {
            T1 command = new T1();
            command.SetArchitecture(this);
            command.Execute();
        }

        public void SendCommand<T1>(T1 command) where T1 : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        public IUnRegister RegisterEvent<T1>(Action<T1> onEvent)
        {
            return mTypeEventSystem.Register<T1>(onEvent);
        }

        public void SendEvent<T1>() where T1 : new()
        {
            mTypeEventSystem.Send<T1>();
        }

        public void SendEvent<T1>(T1 e)
        {
            mTypeEventSystem.Send<T1>(e);
        }

        public void UnRegisterEvent<T1>(Action<T1> onEvent)
        {
            mTypeEventSystem.UnRegister<T1>(onEvent);
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }
    }
    #endregion

    #region  Controller
    public interface IController : IBelongToArchitecture, ICanGetModel, ICanGetSystem, ICanSendCommand, ICanRegisterEvent, ICanSendQuery
    {

    }

    #endregion

    #region  System
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetUtility, ICanRegisterEvent, ICanSendEvent, ICanGetSystem, ICanSendQuery
    {
        void Init();
    }
    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture mArchitecture;

        void ISystem.Init()
        {
            OnInit();
        }
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
        protected abstract void OnInit();
    }
    #endregion

    #region  Model
    public interface IModel : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent, ICanSendQuery
    {
        public void Init();
    }
    public abstract class AbstractModel : IModel
    {
        private IArchitecture mArchitecture;
        void IModel.Init()
        {
            OnInit();
        }
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
        protected abstract void OnInit();
    }
    #endregion

    #region  Utility
    public interface IUtility
    {

    }
    #endregion

    #region  Command
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetSystem, ICanGetUtility, ICanSendEvent, ICanSendCommand, ICanSendQuery
    {
        public void Execute();
    }
    public abstract class AbstractCommand : ICommand
    {
        private IArchitecture mArchitecture;
        void ICommand.Execute()
        {
            OnExecute();
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
        protected abstract void OnExecute();
    }

    #endregion

    #region  Query
    public interface IQuery<T> : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetSystem, ICanGetUtility
    {
        T Do();
    }
    public abstract class AbstractQuery<T> : IQuery<T>
    {
        private IArchitecture mArchitecture;
        public T Do()
        {
            return OnDo();
        }
        protected abstract T OnDo();

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
    }
    #endregion

    #region  TypeEventSystem
    public interface IOnEvent<T> where T : struct
    {
        public void OnEvent(T e);
    }
    public static class IOnEventExtension
    {
        public static IUnRegister RegisterEvent<T>(this IOnEvent<T> onEvent) where T : struct
        {
            return TypeEventSystem.Global.Register<T>(onEvent.OnEvent);
        }
        // public static void UnRegisterEvent<T>(this IOnEvent<T> onEvent)
        // {
        //     TypeEventSystem.Global.UnRegister<T>(onEvent.OnEvent);
        // }
    }
    public class TypeEventSystem : ITypeEventSystem
    {
        private Dictionary<Type, IRegistrations> mEventRegistrations = new Dictionary<Type, IRegistrations>();

        public static readonly TypeEventSystem Global = new TypeEventSystem();

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            Type type = typeof(T);

            if (mEventRegistrations.ContainsKey(type) == false)
            {
                mEventRegistrations.Add(type, new Registrations<T>());
            }

            (mEventRegistrations[type] as Registrations<T>).onEvent += onEvent;

            return new TypeEventSystemUnRegister<T>() { TypeEventSystem = this, Action = onEvent };
        }

        public void Send<T>() where T : new()
        {
            if (mEventRegistrations.TryGetValue(typeof(T), out IRegistrations registrations))
            {
                T theEvent = new T();
                (registrations as Registrations<T>).onEvent?.Invoke(theEvent);
            }
        }

        public void Send<T>(T theEvent)
        {
            if (mEventRegistrations.TryGetValue(typeof(T), out IRegistrations registrations))
            {
                (registrations as Registrations<T>).onEvent?.Invoke(theEvent);
            }
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            if (mEventRegistrations.TryGetValue(typeof(T), out IRegistrations registrations))
            {
                (registrations as Registrations<T>).onEvent -= onEvent;
            }
        }
    }
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();

        void Send<T>(T e);

        IUnRegister Register<T>(Action<T> onEvent);

        void UnRegister<T>(Action<T> onEvent);
    }

    public interface IRegistrations
    {

    }
    public class Registrations<T> : IRegistrations
    {
        public Action<T> onEvent;
    }
    public interface IUnRegister
    {
        void UnRegister();
    }
    public class TypeEventSystemUnRegister<T> : IUnRegister
    {
        public TypeEventSystem TypeEventSystem { get; set; }

        public Action<T> Action { get; set; }

        public void UnRegister()
        {
            TypeEventSystem.UnRegister<T>(Action);
        }
    }
    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        HashSet<IUnRegister> unRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            unRegisters.Add(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in unRegisters)
                unRegister.UnRegister();

            unRegisters.Clear();
        }
    }
    public static class UnRegisterExtension
    {
        public static void UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject)
        {
            if (gameObject.TryGetComponent<UnRegisterOnDestroyTrigger>(out UnRegisterOnDestroyTrigger unRegisterOnDestroyTrigger))
            {
                unRegisterOnDestroyTrigger.AddUnRegister(unRegister);
            }
            else
            {
                gameObject.AddComponent<UnRegisterOnDestroyTrigger>().AddUnRegister(unRegister);
            }
        }
    }
    #endregion

    #region  IOCContainer
    public class IOCContainer
    {
        private Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        public void Register<T>(T instance)
        {
            var key = typeof(T);

            if (!mInstances.ContainsKey(key))
            {
                mInstances.Add(key, instance);
            }
            else
            {
                mInstances[key] = instance;
            }
        }
        public T Get<T>() where T : class
        {
            var key = typeof(T);

            if (mInstances.TryGetValue(key, out var instance))
            {
                return instance as T;
            }
            return null;
        }
    }
    #endregion

    #region  BindableProperty
    public class BindableProperty<T>
    {
        private T mValue;

        public T Value
        {
            get => mValue;
            set
            {
                if (value == null && mValue == null) return;
                if (value != null && value.Equals(mValue) == true) return;
                mValue = value;

                mOnValueChanged?.Invoke(mValue);
            }
        }
        public BindableProperty(T defaultValue = default)
        {
            mValue = defaultValue;
        }
        private Action<T> mOnValueChanged;

        public IUnRegister Register(Action<T> onValueChanged)
        {
            mOnValueChanged += onValueChanged;

            return new BindablePropertyUnRegister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }
        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(mValue);
            return Register(onValueChanged);
        }
        public static implicit operator T(BindableProperty<T> property)
        {
            return property.Value;
        }
        public override string ToString()
        {
            return mValue.ToString();
        }
        public void UnRegister(Action<T> onValueChanged)
        {
            mOnValueChanged -= onValueChanged;
        }

        // public BindableProperty()
        // {
        //     Type type = typeof(T);

        //     if (type.IsClass)
        //     {
        //         //mValue = (T)type.Assembly.CreateInstance(type.FullName);
        //         mValue = (T)Activator.CreateInstance(type, "".ToCharArray());
        //     }
        // }
    }
    public class BindablePropertyUnRegister<T> : IUnRegister
    {
        public BindableProperty<T> BindableProperty;

        public Action<T> OnValueChanged;

        public void UnRegister()
        {
            BindableProperty.UnRegister(OnValueChanged);
            BindableProperty = null;
            OnValueChanged = null;
        }
    }
    #endregion

    #region  Rule
    public interface ICanGetModel : IBelongToArchitecture
    {

    }
    public static class CanGetModelExtension
    {
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }
    public interface ICanGetSystem : IBelongToArchitecture
    {

    }
    public static class CanGetSystemExtension
    {
        public static T GetSystem<T>(this ICanGetModel self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }
    public interface ICanGetUtility : IBelongToArchitecture
    {

    }
    public static class CanGetUtilityExtension
    {
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }
    }
    public interface ICanRegisterEvent : IBelongToArchitecture
    {

    }
    public static class ICanRegisterEventExtension
    {
        public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            return self.GetArchitecture().RegisterEvent<T>(onEvent);
        }
        public static void UnRegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            self.GetArchitecture().UnRegisterEvent<T>(onEvent);
        }
    }
    public interface ICanSendCommand : IBelongToArchitecture
    {

    }
    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }
        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand<T>(command);
        }
    }
    public interface ICanSendEvent : IBelongToArchitecture
    {

    }
    public static class ICanSendEventExtension
    {
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }
        public static void SendEvent<T>(this ICanSendEvent self, T e)
        {
            self.GetArchitecture().SendEvent<T>(e);
        }
    }
    public interface ICanSendQuery : IBelongToArchitecture
    {

    }
    public static class CanSendQueryExtensions
    {
        public static T SendQuery<T>(this ICanSendQuery self, IQuery<T> query)
        {
            return self.GetArchitecture().SendQuery<T>(query);
        }
    }
    public interface IBelongToArchitecture
    {
        public IArchitecture GetArchitecture();
    }
    public interface ICanSetArchitecture
    {
        public void SetArchitecture(IArchitecture architecture);
    }
    #endregion

}