namespace EntityAsyncTask.Core
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return $"{Id} - {FirstName} - {LastName} - {Age}";
        }
    }
}
