using System;
using System.Collections.Generic;

using Numitor.SDK.Model;

namespace Numitor.SDK.Model.QuestionModel
{
    [Serializable]
    public class Question : BaseModel
    {
        public string question;
        public string[] answers;
        public int points;

        public string toString()
        {
            return "Question: " + question + "\n" +
                "Answers: \n\t" +
                answers[0] + "\n\t" +
                answers[1] + "\n\t" +
                answers[2] + "\n\t" +
                answers[3] + "\n" +
                "Points: " + points;

        }
    }
}