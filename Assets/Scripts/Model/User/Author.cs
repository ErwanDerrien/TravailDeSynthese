using System;
using System.Collections.Generic;

using Numitor.SDK.Model;

namespace Numitor.SDK.Model.AuthorModel
{
    [Serializable]
    public class Author : BaseModel
    {
        public string password;

        public Author(string id, string password)
        {
            base.id = id;
            this.password = password;
        }
    }
}