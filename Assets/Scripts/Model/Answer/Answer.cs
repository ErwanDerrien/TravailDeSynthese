using System;
using System.Collections.Generic;

using Numitor.SDK.Model;

namespace Numitor.SDK.Model.AnswerModel
{
    [Serializable]
    public class Answer : BaseModel
    {
        public int index;

        public Answer(string id, int index)
        {
            base.id = id;
            this.index = index;
        }

        override public String ToString()
        {
            return "id: " + id + "\n" + " index: " + index + "\n";
        }
    }
}