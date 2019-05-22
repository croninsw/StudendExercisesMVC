using System;

namespace StudentExercises
{
    public class NSSPerson
    {
        // private classes to set students name and slack information

        private string _firstName { get; }

        private string _lastName { get; }

        private string _slackName { get; set; }

        public string FullName
        {
            get
            {
                return $"{_firstName} {_lastName}";
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
        }

        public NSSPerson(string FirstName, string LastName, string SlackName)
        {
            _firstName = FirstName;
            _lastName = LastName;
            _slackName = SlackName;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}