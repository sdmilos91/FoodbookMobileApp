using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.Renderers
{
    public class CustomEntry : Entry
    {
        public CustomEntry()
        {
            MyRadius = 0;
            EntryType = EntryTypes.DEFAULT;
        }

        public float? MyRadius { get; set; }
        public EntryTypes EntryType { get; set; }
    }

    public enum EntryTypes
    {
        DEFAULT = 1,
        MULTILINE = 2,
    }
}
