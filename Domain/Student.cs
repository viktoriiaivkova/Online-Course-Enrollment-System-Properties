using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Course_Enrollment_System_Properties.Domain
{
    public class Student
    {
        public string Id { get; }

        public Student(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Invalid student id");

            Id = id;
        }
    }
}
