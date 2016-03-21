using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ListviewWithRefresh
{
    public class SwipeListAdapter : BaseAdapter
    {
        private Activity activity;
        private LayoutInflater inflater;
        private List<ElementModel> whateverList;

        public SwipeListAdapter(Activity activity, List<ElementModel> list)
        {
            this.activity = activity;
            this.whateverList = list;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return whateverList[position].Abstract + whateverList[position].Altered.ToString();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null) { 
                inflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
            }

            if (convertView == null) { 
                convertView = inflater.Inflate(Resource.Layout.list_row, null);
            }

            TextView title = (TextView)convertView.FindViewById(Resource.Id.title);
            title.Text = whateverList[position].Abstract  + " " + whateverList[position].Altered.ToString();

            return convertView;
        }

        public override int Count { get { return whateverList.Count; } }
    }
}