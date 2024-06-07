using QFramework;

namespace LikeSoulKnight
{
    public class GunInfo
    {
        public BindableProperty<string> name;

        public BindableProperty<int> bulletCountInGun;

    }
    public interface IGunSystem : ISystem
    {
        public GunInfo CurrentGunInfo { get; }
    }
    public class GunSystem : AbstractSystem, IGunSystem
    {
        public GunInfo CurrentGunInfo { get; } = new GunInfo()
        {
            bulletCountInGun = new BindableProperty<int>()
            {
                Value = 5
            },

            name = new BindableProperty<string>()
            {
                Value = "手枪"
            }
        };
        protected override void OnInit()
        {

        }
    }
}

