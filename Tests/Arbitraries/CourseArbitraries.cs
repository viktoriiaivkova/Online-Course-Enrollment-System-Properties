using FsCheck;
using Online_Course_Enrollment_System_Properties.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Online_Course_Enrollment_System_Properties.Tests.Arbitraries
{
    public static class CourseArbitraries
    {
        public static Arbitrary<string> NonEmptyString() =>
            Arb.Default.String().Filter(s => !string.IsNullOrWhiteSpace(s));

        public static Arbitrary<int> PositiveInt() =>
            Arb.Default.Int32().Filter(i => i > 0 && i <= 1000);

        public static Arbitrary<Student> Student()
        {
            var gen = NonEmptyString().Generator.Select(id => new Student(id));
            return Arb.From(gen);
        }

        public static Arbitrary<Course> Course()
        {
            var gen = from id in NonEmptyString().Generator
                      from capacity in Gen.Choose(1, 100)
                      select new Course(id, capacity);
            return Arb.From(gen);
        }
    }
}
