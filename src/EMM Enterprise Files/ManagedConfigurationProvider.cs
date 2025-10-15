using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMM_Enterprise_Files
{
    public partial class ManagedConfigurationProvider
    {


        public partial string GetStringValue(string key);
        public partial List<Bundle> GetBundleArrayList(string key);
    }
}
