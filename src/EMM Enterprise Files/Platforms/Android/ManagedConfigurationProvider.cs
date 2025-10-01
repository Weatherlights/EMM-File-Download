using Android.Content;
using Android.OS;
using Android.Systems;
using static Android.OS.Build;
using static Android.OS.Bundle;
using static Android.OS.Parcelable;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Runtime;

namespace EMM_Enterprise_Files
{
    public partial class ManagedConfigurationProvider
    {
        public partial string GetStringValue(string key)
        {
            var manager = (RestrictionsManager)Platform.AppContext.GetSystemService(Context.RestrictionsService);
            var bundle = manager.ApplicationRestrictions;

            if (bundle != null)
            {
                if (bundle.ContainsKey(key))
                    return bundle.GetString(key);
                else
                    return null;
            }

            return null;
        }
        
        public partial List<Bundle> GetBundleArrayList(string key)
        {
            var manager = (RestrictionsManager)Platform.AppContext.GetSystemService(Context.RestrictionsService);
            Bundle bundle = manager.ApplicationRestrictions;
          
            //Android.OS.IParcelable[] bundles = bundle.GetParcelableArray(key, bundle.Class);


            Java.Lang.Object[] bundles = bundle.GetParcelableArray(key, bundle.Class);
            if (bundles == null)
            {
                bundle = DummyBundle.getDummyBundle();
                bundles = bundle.GetParcelableArray(key, bundle.Class);
            }


           
            if (bundles != null)
            {
                List<Bundle> bundleList = new List<Bundle>();
                foreach (var b in bundles)
                {
                    bundleList.Add((Bundle)b);
                }

                return bundleList; 
            }

            return null;
        }
    }
}
