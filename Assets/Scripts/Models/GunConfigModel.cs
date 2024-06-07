using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
interface IGunConfigModel : IModel
{
    GunConfigItem GetConfigItemByName(string name);
}
class GunConfigItem
{
    public GunConfigItem(string name, int bulletMaxCount, float attack, float frequency, float shootDistance, bool needBullet, float reloadSeconds, string description)
    {
        Name = name;
        BulletMaxCount = bulletMaxCount;
        Attack = attack;
        Frequency = frequency;
        ShootDistance = shootDistance;
        NeedBullet = needBullet;
        ReloadSeconds = reloadSeconds;
        Description = description;
    }
    public string Name { get; }
    public int BulletMaxCount { get; }
    public float Attack { get; }
    public float Frequency { get; }
    public float ShootDistance { get; }
    public bool NeedBullet { get; }
    public float ReloadSeconds { get; }
    public string Description { get; }
}
public class GunConfigModel : AbstractModel, IGunConfigModel
{
    private Dictionary<string, GunConfigItem> mGunConfigItems = new Dictionary<string, GunConfigItem>()
        {
            { "手枪", new GunConfigItem("手枪", 7, 1, 1, 0.5f, false, 3, "默认枪") },
            { "冲锋枪", new GunConfigItem("冲锋枪", 30, 1, 6, 0.34f, true, 3, "无") },
            { "步枪", new GunConfigItem("步枪", 50, 3, 3, 1f, true, 1, "有一定后坐力") },
            { "狙击枪", new GunConfigItem("狙击枪", 12, 6, 1, 1f, true, 5, "红外瞄准+后坐力大") },
            { "火箭筒", new GunConfigItem("火箭筒", 1, 5, 1, 1f, true, 4, "跟踪+爆炸") },
            { "霰弹枪", new GunConfigItem("霰弹枪", 1, 1, 1, 0.5f, true, 1, "一次发射 6 ~ 12 个子弹") },
        };
        
    protected override void OnInit()
    {
    }

    GunConfigItem IGunConfigModel.GetConfigItemByName(string name)
    {
        return mGunConfigItems[name];
    }
}
