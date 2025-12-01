using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Course_Enrollment_System_Properties.Domain
{
    public class Course
    {
        public string Id { get; }
        public int Capacity { get; }
        private readonly List<Student> _students = new();

        public Course(string id, int capacity)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException();

            if (capacity <= 0)
                throw new ArgumentException();

            Id = id;
            Capacity = capacity;
        }

        public IReadOnlyCollection<Student> Students => _students;

        public bool Enroll(Student student)
        {
            if (_students.Contains(student))
                return false;

            if (_students.Count >= Capacity)
                return false;

            _students.Add(student);
            return true;
        }
    }

}
