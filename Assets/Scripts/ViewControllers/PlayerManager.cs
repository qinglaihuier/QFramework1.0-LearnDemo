using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum PlayerStateEnum
{
    Idle,
    Move
}
namespace LikeSoulKnight
{
    public class PlayerManager : AbstractMonoController
    {

        private Dictionary<PlayerStateEnum, IState> mPlayerStatesDic = new Dictionary<PlayerStateEnum, IState>();

        private IState mNowState;
        private void Awake()
        {
            mPlayerStatesDic.Add(PlayerStateEnum.Idle, new PlayerIdleState());
            mPlayerStatesDic.Add(PlayerStateEnum.Move, new PlayerMoveState());

            foreach (var state in mPlayerStatesDic)
            {
                state.Value.Init(ChangeState, gameObject);
            }

            ChangeState(PlayerStateEnum.Idle);
        }
        private void Update()
        {
            mNowState.Update();
        }
        private void FixedUpdate()
        {
            mNowState.FixedUpdate();
        }

        private void ChangeState(PlayerStateEnum targetState)
        {
            if (mNowState != null)
            {
                mNowState.Exit();
            }
            mNowState = mPlayerStatesDic[targetState];
            mNowState.Enter();
        }

    }
}

