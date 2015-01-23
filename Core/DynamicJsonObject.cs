// This code is provided under the MIT license. Originally by Alessandro Pilati.
// based on http://www.drowningintechnicaldebt.com/ShawnWeisfeld/archive/2010/08/22/using-c-4.0-and-dynamic-to-parse-json.aspx

using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace SnowyPeak.Duality.Plugin.Data
{
    [Serializable]
    public class DynamicJsonObject : DynamicObject
    {
        private IDictionary<string, object> _dictionary { get; set; }

        public DynamicJsonObject(IDictionary<string, object> dictionary)
        {
            _dictionary = dictionary;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _dictionary[binder.Name];

            if (result is IDictionary<string, object>)
            {
                result = new DynamicJsonObject(result as IDictionary<string, object>);
            }
            else if (result is ArrayList && (result as ArrayList) is IDictionary<string, object>)
            {
                result = new List<DynamicJsonObject>((result as ArrayList).ToArray().Select(x => new DynamicJsonObject(x as IDictionary<string, object>)));
            }
            else if (result is ArrayList)
            {
                result = new List<object>((result as ArrayList).ToArray());
            }

            return _dictionary.ContainsKey(binder.Name);
        }
    }
}