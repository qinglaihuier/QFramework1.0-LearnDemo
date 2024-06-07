using UnityEngine;
using QFramework;
namespace LikeSoulKnight
{
    public class ShootingCommand : AbstractCommand
    {
        public static readonly ShootingCommand Single = new ShootingCommand();

        protected override void OnExecute()
        {
            IGunSystem gunSystem = this.GetSystem<IGunSystem>();
            gunSystem.CurrentGunInfo.bulletCountInGun.Value -= 1;
        }
    }
}
