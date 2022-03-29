using System;
using System.Collections.Generic;

namespace Numitor.SDK.Model
{
    [Serializable]
    public class BaseModel
    {
        public string id;

        public override string ToString()
        {
            List<string> output = new List<string>();
            if (!string.IsNullOrEmpty(id)) output.Add($"id: `{id}`");
            return String.Join(", ", output);
        }
    }
}