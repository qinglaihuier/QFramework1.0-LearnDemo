using QFramework;
using UnityEngine;
namespace LikeSoulKnight
{
    public interface IPlayerModel : IModel
    {
        public BindableProperty<int> hp { get; }

        public BindableProperty<int> speedSize { get; }

        public Vector2 position { get; set; }
        //其他
    }
    public class PlayerModel : AbstractModel, IPlayerModel
    {
        public BindableProperty<int> hp { get; } = new BindableProperty<int>();

        public BindableProperty<int> speedSize { get; } = new BindableProperty<int>();

        public Vector2 position { get; set; }

        protected override void OnInit()
        {
            //解析存储文件并读取内容
            PlayerData data = Resources.Load<PlayerData>("ScriptableObject/PlayerData");
        
            hp.Value = data.hp;
            speedSize.Value = data.speedSize;
        }
    }

}

