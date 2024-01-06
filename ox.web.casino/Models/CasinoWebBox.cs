using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntDesign.ProLayout;
using OX.Wallets;

namespace OX.Web.Models
{
    public class CasinoWebBox : WebBoxBlazor
    {
        public override bool SupportMobile => OXRunTime.RunMode == RunMode.Server;
        public override uint BoxIndex { get { return 1000; } }
        public CasinoWebBox() : base()
        {
        }
        public override void Init()
        {

        }
        public override MenuDataItem[] GetMemus(string language)
        {
            List<MenuDataItem> list = new List<MenuDataItem>();
            if (OXRunTime.RunMode == RunMode.Server)
            {
                list.Add(new MenuDataItem
                {
                    Path = "/_pc/casino",
                    Name = UIHelper.WebLocalString(language, "娱乐", "Casino"),
                    Key = "casino",
                    Children = new MenuDataItem[] {
                    new MenuDataItem
                    {
                        Path = "/_pc/casino/rooms",
                        Name =  UIHelper.WebLocalString(language,"房间列表", "Room List"),
                        Key = "rooms"
                    },
                     new MenuDataItem
                    {
                        Path = "/_pc/casino/bury",
                        Name =  UIHelper.WebLocalString(language,"爆雷", "Bury"),
                        Key = "bury"
                    },
                     new MenuDataItem
                    {
                        Path = "/_pc/casino/feedback",
                        Name =  UIHelper.WebLocalString(language,"娱乐信托池", "Feedback Trust Pool"),
                        Key = "casinofeedback"
                    }
                }
                });
            }
            return list.ToArray();
        }
        public override MenuDataItem[] GetMobileMemus(string language)
        {
            List<MenuDataItem> list = new List<MenuDataItem>();
            if (OXRunTime.RunMode == RunMode.Server)
            {
                list.Add(new MenuDataItem
                {
                    Path = "/_m/casino",
                    Name = UIHelper.WebLocalString(language, "娱乐", "Casino"),
                    Key = "casino"
                });
                list.Add(new MenuDataItem
                {
                    Path = "/_m/bury",
                    Name = UIHelper.WebLocalString(language, "爆雷", "Bury"),
                    Key = "bury"
                });
            }
            return list.ToArray();
        }

    }
}
