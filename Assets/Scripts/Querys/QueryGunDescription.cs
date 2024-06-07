using QFramework;
namespace LikeSoulKnight
{
    public class QueryGunDescription : AbstractQuery<string>
    {
        private string mName;

        public QueryGunDescription(string name)
        {
            mName = name;
        }
        protected override string OnDo()
        {
            return this.GetModel<IGunConfigModel>().GetConfigItemByName(mName).Description;
        }
    }
}