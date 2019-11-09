using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace LinkedListBenchmarks
{
    public class Student
    {
        public int average;
        public long ID;
        public string firstName;
        public string lastName;

        public static List<string> firstNames = new List<string>()
        {
            // Simple Male
            "Juan",
            "Carlos",
            "Manuel",
            "Francisco",
            "Mauricio",
            "Eduardo",
            // Simple Female
            "Fernanda",
            "María",
            "Sofía",
            "Ana",
            "Carla",
            "Marlene",
            // Composite Male
            "Juan Manuel",
            "Luis Carlos",
            "Manuel Alejandro",
            "Javier Francisco",
            "Luis Eduardo",
            "José Luis",
            // Composite Female
            "María Fernanda",
            "María Jose",
            "Sofía Paulina",
            "Ana Belén",
            "Daniela Alejandra",
            "Luz Angélica"
        };
        public static List<string> lastNames = new List<string>()
        {
            "García",
            "Rodríguez",
            "Hernández",
            "López",
            "Martínez",
            "González",
            "Pérez",
            "Sánchez",
            "Ramírez",
            "Torres",
            "Flores",
            "Rivera",
            "Gómez",
            "Díaz",
            "Cruz",
            "Morales",
            "Reyes",
            "Gutiérrez",
            "Ortiz"
        };
    }

    [MemoryDiagnoser]
    public class ReduceStudent
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private LinkedList<Student> students;

        [GlobalSetup]
        public void ReduceSetup()
        {
            var rnd = new Random();
            students = new LinkedList<Student>();

            for (int i = 1; i <= N; i++)
            {
                students.AddLast(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = Student.firstNames[rnd.Next(0, Student.firstNames.Count)],
                    lastName = Student.lastNames[rnd.Next(0, Student.lastNames.Count)]
                });
            }
        }

        [Benchmark]
        public string LambdaReduce() =>
            students.Aggregate(
                new StringBuilder(),
                (sb, n) => sb.AppendFormat
                (
                    "{0}, {1} - {2}",
                    n.lastName,
                    n.firstName,
                    (n.average > 60) ? n.average.ToString() : "Failed"
                ),
                sb => sb.ToString());

        [Benchmark]
        public string LoopReduce()
        {
            var builder = new StringBuilder(students.Count);
            var head = students.First;

            while(head != null)
            {
                var student = head.Value;
                builder.AppendFormat
                    (
                        "{0}, {1} - {2}",
                        student.lastName,
                        student.firstName,
                        (student.average > 60) ? student.average.ToString() : "Failed"
                    );

                head = head.Next;
            }

            return builder.ToString();
        }

        [Benchmark]
        public string IteratorReduce()
        {
            var builder = new StringBuilder();
            foreach (var student in students)
            {
                builder.AppendFormat
                    (
                        "{0}, {1} - {2}",
                        student.lastName,
                        student.firstName,
                        (student.average > 60) ? student.average.ToString() : "Failed"
                    );
            }

            return builder.ToString();
        }
    }

    [MemoryDiagnoser]
    public class PopulateStudent
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private IEnumerable<int> students;

        [GlobalSetup]
        public void PopulateSetup()
        {
            students = Enumerable.Range(1, N);
        }

        [Benchmark]
        public LinkedList<Student> LambdaPopulate()
        {
            var rnd = new Random();
            var init = students.Select(n => new Student()
            {
                average = rnd.Next(50, 101),
                ID = n * N,
                firstName = Student.firstNames[rnd.Next(0, Student.firstNames.Count)],
                lastName = Student.lastNames[rnd.Next(0, Student.lastNames.Count)]
            });

            return new LinkedList<Student>(init);
        }

        [Benchmark]
        public LinkedList<Student> LoopPopulate()
        {
            var rnd = new Random();
            var result = new LinkedList<Student>();

            for (int i = 1; i <= N; i++)
            {
                result.AddLast(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = Student.firstNames[rnd.Next(0, Student.firstNames.Count)],
                    lastName = Student.lastNames[rnd.Next(0, Student.lastNames.Count)]
                });
            }

            return result;
        }

        [Benchmark]
        public LinkedList<Student> IteratorPopulate()
        {
            var rnd = new Random();
            var result = new LinkedList<Student>();

            foreach (var n in students)
            {
                result.AddLast(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = n * N,
                    firstName = Student.firstNames[rnd.Next(0, Student.firstNames.Count)],
                    lastName = Student.lastNames[rnd.Next(0, Student.lastNames.Count)]
                });
            }

            return result;
        }
    }

    [MemoryDiagnoser]
    public class IterateStudent
    {
        private const int N = 1_000_000;
        private readonly LinkedList<Student> students;

        public IterateStudent()
        {
            var rnd = new Random();
            students = new LinkedList<Student>();

            for (int i = 1; i <= N; i++)
            {
                students.AddLast(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = Student.firstNames[rnd.Next(0, Student.firstNames.Count)],
                    lastName = Student.lastNames[rnd.Next(0, Student.lastNames.Count)]
                });
            }
        }

        [Benchmark]
        public int LambdaIterate() => students.Count(n => n.firstName.Length > 0 && n.average >= 50 && n.ID < long.MaxValue);

        [Benchmark]
        public int LoopIterate()
        {
            int count = 0;
            var head = students.First;
            static bool valid(int len, int avg, long id) => len > 0 && avg >= 50 && id < long.MaxValue;

            while(head != null)
            {
                var s = head.Value;

                if (valid(s.firstName.Length, s.average, s.ID))
                {
                    count++;
                }

                head = head.Next;
            }

            return count;
        }

        [Benchmark]
        public int IteratorIterate()
        {
            int count = 0;
            static bool valid(int len, int avg, long id) => len > 0 && avg >= 50 && id < long.MaxValue;

            foreach (var s in students)
            {
                if (valid(s.firstName.Length, s.average, s.ID))
                {
                    count++;
                }
            }

            return count;
        }
    }

    [MemoryDiagnoser]
    public class ContainsStudent
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private List<Student> students;

        [GlobalSetup]
        public void ContainsSetup()
        {
            var rnd = new Random();
            students = new List<Student>(N);

            for (int i = 1; i <= N; i++)
            {
                students.Add(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = Student.firstNames[rnd.Next(0, Student.firstNames.Count)],
                    lastName = Student.lastNames[rnd.Next(0, Student.lastNames.Count)]
                });
            }
        }

        [Benchmark]
        public bool LambdaContains() => students.Any(s => s.average >= 70 && s.average <= 85 && s.firstName.Contains(' ') && s.lastName.Contains("es"));

        [Benchmark]
        public bool LoopContains()
        {
            for (int i = 0; i < students.Count; i++)
            {
                var s = students[i];
                if (s.average >= 70 && s.average <= 85 && s.firstName.Contains(' ') && s.lastName.Contains("es"))
                {
                    return true;
                }
            }

            return false;
        }

        [Benchmark]
        public bool IteratorContains()
        {
            foreach (var s in students)
            {
                if (s.average >= 70 && s.average <= 85 && s.firstName.Contains(' ') && s.lastName.Contains("es"))
                {
                    return true;
                }
            }

            return false;
        }
    }

    [MemoryDiagnoser]
    public class FilterStudent
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private int target;
        private Consumer consumer;
        private LinkedList<Student> students;

        [GlobalSetup]
        public void FilterSetup()
        {
            var rnd = new Random();

            students = new LinkedList<Student>();
            consumer = new Consumer();
            target = rnd.Next(1, N / 2);

            for (int i = 1; i <= N; i++)
            {
                students.AddLast(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = Student.firstNames[rnd.Next(0, Student.firstNames.Count)],
                    lastName = Student.lastNames[rnd.Next(0, Student.lastNames.Count)]
                });
            }
        }

        [Benchmark]
        public void LambdaFilter() => students.Where(s => s.average > 50 && s.average < 70 && s.firstName.Contains('i', StringComparison.InvariantCulture) && s.ID > target).Consume(consumer);

        [Benchmark]
        public void LoopFilter()
        {
            var result = new List<Student>();
            var head = students.First;

            while(head != null)
            {
                var s = head.Value;
                if (s.average > 50 && s.average < 70 && s.firstName.Contains('i', StringComparison.InvariantCulture) && s.ID > target)
                {
                    result.Add(s);
                }

                head = head.Next;
            }

            result.Consume(consumer);
        }

        [Benchmark]
        public void IteratorFilter()
        {
            var result = new List<Student>();
            foreach (var s in students)
            {
                if (s.average > 50 && s.average < 70 && s.firstName.Contains('i', StringComparison.InvariantCulture) && s.ID > target)
                {
                    result.Add(s);
                }
            }

            result.Consume(consumer);
        }
    }

    [MemoryDiagnoser]
    public class CopyStudent
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private LinkedList<Student> students;

        [GlobalSetup]
        public void CopySetup()
        {
            var rnd = new Random();

            students = new LinkedList<Student>();

            for (int i = 1; i <= N; i++)
            {
                students.AddLast(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = Student.firstNames[rnd.Next(0, Student.firstNames.Count)],
                    lastName = Student.lastNames[rnd.Next(0, Student.lastNames.Count)]
                });
            }
        }

        [Benchmark]
        public LinkedList<Student> LambdaCopy() => new LinkedList<Student>(students.Select(s => new Student() { average = s.average, ID = s.ID, firstName = s.firstName, lastName = s.lastName }));

        [Benchmark]
        public LinkedList<Student> LoopCopy()
        {
            var copy = new LinkedList<Student>();
            var head = students.First;

            while(head != null)
            {
                var student = head.Value;
                copy.AddLast(new Student()
                {
                    average = student.average,
                    ID = student.ID,
                    firstName = student.firstName,
                    lastName = student.lastName
                });

                head = head.Next;
            }

            return copy;
        }

        [Benchmark]
        public LinkedList<Student> IteratorCopy()
        {
            var copy = new LinkedList<Student>();
            foreach (var s in students)
            {
                copy.AddLast(new Student()
                {
                    average = s.average,
                    ID = s.ID,
                    firstName = s.firstName,
                    lastName = s.lastName
                });
            }

            return copy;
        }
    }

    [MemoryDiagnoser]
    public class MapStudent
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private LinkedList<Student> students;

        [GlobalSetup]
        public void MapSetup()
        {
            var rnd = new Random();
            students = new LinkedList<Student>();

            for (int i = 1; i <= N; i++)
            {
                students.AddLast(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = Student.firstNames[rnd.Next(0, Student.firstNames.Count)],
                    lastName = Student.lastNames[rnd.Next(0, Student.lastNames.Count)]
                });
            }
        }

        [Benchmark]
        public Dictionary<long, string> LambdaMap() => students.ToDictionary(s => s.ID, s => $"{s.lastName}, {s.firstName}");

        [Benchmark]
        public Dictionary<long, string> LoopMap()
        {
            var result = new Dictionary<long, string>(students.Count);
            var head = students.First;

            while (head != null)
            {
                var student = head.Value;
                result.Add(student.ID, $"{student.lastName}, {student.firstName}");

                head = head.Next;
            }

            return result;
        }

        [Benchmark]
        public Dictionary<long, string> IteratorMap()
        {
            var result = new Dictionary<long, string>(students.Count);
            foreach (var s in students)
            {
                result.Add(s.ID, $"{s.lastName}, {s.firstName}");
            }

            return result;
        }
    }
}
