using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIllionare
{
    class Question
    {
        public string[] answers = new string[4];
        public string question;
        public string true_answer;

        public Question(string quest,string a1,string a2,string a3,string a4,string true_a)
        {
            question = quest;
            answers[0] = a1;
            answers[1] = a2;
            answers[2] = a3;
            answers[3] = a4;
            true_answer = true_a;
        }
        
    }
}
