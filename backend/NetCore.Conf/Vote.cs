using System;

namespace NetCore.Conf
{
    public class Vote
    {
        public string Username { get; set; }

        public int QuestionId { get; set; }

        public int Usefull { get; set; }

        public int Crazy { get; set; }

        public void Validate()
        {
            if (Usefull < 0 || Usefull > 10)
                throw new ArgumentException("Usefull should be a number between 0 and 10");
            if (Crazy < 0 || Crazy > 10)
                throw new ArgumentException("Crazy should be a number between 0 and 10");
            if (string.IsNullOrEmpty(Username))
                throw new ArgumentException("Username should be a twitter account name or a person full name");
        }
    }
}