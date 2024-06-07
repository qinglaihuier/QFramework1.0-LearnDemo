
using System;
using QFramework;
using UnityEngine;

namespace LikeSoulKnight
{
    public interface IState
    {
        public void Init(Action<PlayerStateEnum> mChangeStateAction, GameObject gameObject);
        public void Enter();
        public void Update();
        public void FixedUpdate();
        public void Exit();
    }

    public abstract class AbstractPlayerState : IController, IState
    {
        private GameInput mGameInput;
        protected GameInput gameInput
        {
            get { return mGameInput; }
        }

        protected Action<PlayerStateEnum> changeStateAction;

        protected GameObject gameObject;

        protected IPlayerModel playerModel;
        void IState.Enter()
        {

            OnEnter();
        }

        void IState.Exit()
        {
            OnExit();
        }

        void IState.FixedUpdate()
        {
            OnFixedUpdate();
        }

        void IState.Update()
        {
            OnUpdate();
        }
        protected abstract void OnEnter();
        protected abstract void OnExit();
        protected abstract void OnFixedUpdate();
        protected abstract void OnUpdate();

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return LikeSoulKnight.LikeSoulKnightArchitecture.Interface;
        }

        void IState.Init(Action<PlayerStateEnum> mChangeStateAction, GameObject gameObject)
        {
            if (mGameInput == null)
            {
                mGameInput = (this as IController).GetArchitecture().GetSystem<IGameInputSystem>().GetGameInput();
            }
            this.changeStateAction = mChangeStateAction;

            this.gameObject = gameObject;

            playerModel = this.GetModel<IPlayerModel>();
        }
    }
    public class PlayerMoveState : AbstractPlayerState
    {
        private Rigidbody2D mRb;
        protected override void OnEnter()
        {
            mRb = gameObject.GetComponent<Rigidbody2D>();
            Vector2 direction = gameInput.GamePlayInputMaps.Movement.ReadValue<Vector2>();
            mRb.velocity = direction * playerModel.speedSize.Value;
        }

        protected override void OnExit()
        {

        }

        protected override void OnFixedUpdate()
        {
            if (gameInput.GamePlayInputMaps.Movement.IsPressed() == false)
            {
                changeStateAction?.Invoke(PlayerStateEnum.Idle);
            }

            Vector2 direction = gameInput.GamePlayInputMaps.Movement.ReadValue<Vector2>();
            direction = direction.normalized;
            mRb.velocity = direction * playerModel.speedSize.Value;

        }

        protected override void OnUpdate()
        {
            playerModel.position = gameObject.transform.position;
            Vector3 localScale = gameObject.transform.localScale;

            float readX = gameInput.GamePlayInputMaps.Movement.ReadValue<Vector2>().x;

            if (readX!= 0)
            {
                localScale.x = Mathf.Sign(readX);
                gameObject.transform.localScale = localScale;
            }

        }
    }
    public class PlayerIdleState : AbstractPlayerState
    {
        protected override void OnEnter()
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        protected override void OnExit()
        {
        }

        protected override void OnFixedUpdate()
        {

        }

        protected override void OnUpdate()
        {
            if (gameInput.GamePlayInputMaps.Movement.IsPressed() == true)
            {
                changeStateAction?.Invoke(PlayerStateEnum.Move);
            }
        }
    }
}
