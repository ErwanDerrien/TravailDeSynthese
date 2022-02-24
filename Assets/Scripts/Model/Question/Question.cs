using System;
using System.Collections.Generic;

using Numitor.SDK.Model;

namespace Numitor.SDK.Model.QuestionModel {
    [Serializable]
    public class Question : BaseModel {
        public string question;
        public string[] answers;
        public int points;
    }
}