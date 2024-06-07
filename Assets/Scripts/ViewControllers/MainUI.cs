using System;
using QFramework;
using UnityEngine;
namespace LikeSoulKnight
{
    public class MainUI : AbstractMonoController
    {
        private IGunSystem mGunSystem;

        private string mDescription;

        private readonly Lazy<GUIStyle> mFrontStyle = new Lazy<GUIStyle>(
                () => new GUIStyle(GUI.skin.label)
                {
                    fontSize = 40
                }
                );
        private void Awake()
        {
            mGunSystem = this.GetSystem<IGunSystem>();
            mDescription = this.SendQuery<string>(new QueryGunDescription(mGunSystem.CurrentGunInfo.name.Value));
        }
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 60, 300, 100), $"枪内子弹 : {mGunSystem.CurrentGunInfo.bulletCountInGun.Value}", mFrontStyle.Value);
            GUI.Label(new Rect(10, 130, 300, 100), $"描述 : {mDescription}", mFrontStyle.Value);
        }
    }
}
