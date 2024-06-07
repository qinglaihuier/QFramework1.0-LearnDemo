using System;
using QFramework;
using UnityEngine;
namespace LikeSoulKnight
{
    public class LikeSoulKnightArchitecture : Architecture<LikeSoulKnightArchitecture>
    {
        protected override void Init()
        {
            this.RegisterSystem<IGameInputSystem>(new GameInputSystem());
            this.RegisterSystem<IGunSystem>(new GunSystem());
            this.RegisterModel<IPlayerModel>(new PlayerModel());
            this.RegisterModel<IGunConfigModel>(new GunConfigModel());
        }
    }
}

