using QFramework;
using UnityEngine;
namespace LikeSoulKnight
{
    public abstract class AbstractMonoController : MonoBehaviour, IController
    {
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return LikeSoulKnight.LikeSoulKnightArchitecture.Interface;
        }
    }
}