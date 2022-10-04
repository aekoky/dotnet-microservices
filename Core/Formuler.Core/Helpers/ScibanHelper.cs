using Scriban.Runtime;
using System.Collections.Generic;
using System.Dynamic;

namespace Formuler.Core.Helpers
{
    public static class ScibanHelper
    {
        public static ScriptObject BuildScriptObject(this ExpandoObject expando)
        {
            var dict = (IDictionary<string, object>)expando;
            var scriptObject = new ScriptObject();

            foreach (var kv in dict)
            {
                //var renamedKey = StandardMemberRenamer.Rename(kv.Key);
                var renamedKey = kv.Key;
                if (kv.Value is ExpandoObject expandoValue)
                {
                    scriptObject.Add(renamedKey, BuildScriptObject(expandoValue));
                }
                else if (kv.Value is IList<object>)
                {
                    var list = new List<ScriptObject>();
                    foreach (var obj in kv.Value as IList<object>)
                    {
                        list.Add(BuildScriptObject(obj as ExpandoObject));
                    }
                    var scriptList = new ScriptObject[list.Count];
                    list.CopyTo(scriptList);
                    scriptObject.Add(renamedKey, scriptList);
                }
                else
                {
                    scriptObject.Add(renamedKey, kv.Value);
                }
            }

            return scriptObject;
        }
    }
}
