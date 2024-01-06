using OX.Wallets;

namespace OX.UI.Casino
{
    public class EnumItem<T> where T : struct
    {
        public T Target { get; private set; }
        public EnumItem(T obj)
        {
            this.Target = obj;
        }
        public override string ToString()
        {
            return UIHelper.LocalString(Target.StringValue(), Target.EngStringValue());
        }
        public  string ToWebString(OX.Wallets.ILanguage language)
        {
            return language.WebLocalString(Target.StringValue(), Target.EngStringValue());
        }
    }
}
