using LikeSoulKnight;
using QFramework;
using UnityEngine;
using UnityEngine.InputSystem;
namespace LikeSoulKnight
{
    // [System.Serializable]
    // public struct ShootEvent
    // {
    //     public string bulletPath;
    // }
    public class PlayerCenterPoint : AbstractMonoController
    {
        public bool IsAbleToShoot { get; set; } = true;

        private IGameInputSystem mGameInputSystem;
        private IGunSystem mGunSystem;
        private GameInput mGameInput;

        const string BULLET_PATH = "Bullets/NormalBullet";
        private void Awake()
        {
            mGameInputSystem = this.GetSystem<IGameInputSystem>();
            mGunSystem = this.GetSystem<IGunSystem>();
            mGameInput = mGameInputSystem.GetGameInput();
        }

        private void LateUpdate()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 direction = (Vector2)(Camera.main.ScreenToWorldPoint(mousePos) - transform.position);
            direction.Normalize();
            transform.right = direction * transform.lossyScale.x;


            if (mGameInput.GamePlayInputMaps.Shooting.WasPressedThisFrame())
            {
                if (IsAbleToShoot && mGunSystem.CurrentGunInfo.bulletCountInGun > 0)
                {
                    GameObject bullet = ObjectPool.GetMonoGameObject(BULLET_PATH);
                    bullet.transform.position = transform.GetChild(0).position;
                    bullet.GetComponent<Bullet>().SetVelocity(direction);
                    bullet.transform.right = direction;

                    this.SendCommand(ShootingCommand.Single);
                }
            }
        }
    }

}
