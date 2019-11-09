using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace HashSetBenchmarks
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
        private HashSet<Student> students;

        [GlobalSetup]
        public void ReduceSetup()
        {
            var rnd = new Random();
            students = new HashSet<Student>(N);

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
        public string LambdaReduce() =>
            students.Aggregate(
                new StringBuilder(),
                (sb, s) => sb.AppendFormat
                (
                    "{0}, {1} - {2}",
                    s.lastName,
                    s.firstName,
                    (s.average > 60) ? s.average.ToString() : "Failed"
                ),
                sb => sb.ToString());

        [Benchmark]
        public string LoopReduce()
        {
            var builder = new StringBuilder(students.Count);
            var iter = students.GetEnumerator();

            while (iter.MoveNext())
            {
                var student = iter.Current;
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
        public HashSet<Student> LambdaPopulate()
        {
            var rnd = new Random();
            var init = students.Select(s => new Student()
            {
                average = rnd.Next(50, 101),
                ID = s * N,
                firstName = Student.firstNames[rnd.Next(0, Student.firstNames.Count)],
                lastName = Student.lastNames[rnd.Next(0, Student.lastNames.Count)]
            });

            return new HashSet<Student>(init);
        }

        [Benchmark]
        public HashSet<Student> LoopPopulate()
        {
            var rnd = new Random();
            var result = new HashSet<Student>();

            for (int i = 1; i <= N; i++)
            {
                result.Add(new Student()
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
        public HashSet<Student> IteratorPopulate()
        {
            var rnd = new Random();
            var result = new HashSet<Student>();

            foreach (var s in students)
            {
                result.Add(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = s * N,
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
        [Params(100, 1000, 10_000)]
        public int N;
        private HashSet<Student> students;

        [GlobalSetup]
        public void IterateSetup()
        {
            var rnd = new Random();
            students = new HashSet<Student>(N);

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
        public int LambdaIterate() => students.Count(s => s.firstName.Length > 0 && s.average >= 50 && s.ID < long.MaxValue);

        [Benchmark]
        public int LoopIterate()
        {
            int count = 0;
            var iter = students.GetEnumerator();
            static bool valid(int len, int avg, long id) => len > 0 && avg >= 50 && id < long.MaxValue;

            while (iter.MoveNext())
            {
                var s = iter.Current;
                if (valid(s.firstName.Length, s.average, s.ID))
                {
                    count++;
                }
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
        private HashSet<Student> students;

        [GlobalSetup]
        public void ContainsSetup()
        {
            var rnd = new Random();
            students = new HashSet<Student>(N);

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
            var iter = students.GetEnumerator();

            while(iter.MoveNext())
            {
                var s = iter.Current;
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
        private HashSet<Student> students;

        [GlobalSetup]
        public void FilterSetup()
        {
            var rnd = new Random();

            students = new HashSet<Student>(N);
            consumer = new Consumer();
            target = rnd.Next(1, N / 2);

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
        public void LambdaFilter() => students.Where(s => s.average > 50 && s.average < 70 && s.firstName.Contains('i', StringComparison.InvariantCulture) && s.ID > target).Consume(consumer);

        [Benchmark]
        public void LoopFilter()
        {
            var result = new List<Student>();
            var iter = students.GetEnumerator();

            while(iter.MoveNext())
            {
                var s = iter.Current;
                if (s.average > 50 && s.average < 70 && s.firstName.Contains('i', StringComparison.InvariantCulture) && s.ID > target)
                {
                    result.Add(s);
                }
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
        private HashSet<Student> students;

        [GlobalSetup]
        public void CopySetup()
        {
            var rnd = new Random();

            students = new HashSet<Student>(N);

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
        public HashSet<Student> LambdaCopy() => new HashSet<Student>(students.Select(s => new Student() { average = s.average, ID = s.ID, firstName = s.firstName, lastName = s.lastName }));

        [Benchmark]
        public HashSet<Student> LoopCopy()
        {
            var copy = new HashSet<Student>(students.Count);
            var iter = students.GetEnumerator();

            while(iter.MoveNext())
            {
                var student = iter.Current;
                copy.Add(new Student()
                {
                    average = student.average,
                    ID = student.ID,
                    firstName = student.firstName,
                    lastName = student.lastName
                });
            }

            return copy;
        }

        [Benchmark]
        public HashSet<Student> IteratorCopy()
        {
            var copy = new HashSet<Student>(students.Count);
            foreach (var student in students)
            {
                copy.Add(new Student()
                {
                    average = student.average,
                    ID = student.ID,
                    firstName = student.firstName,
                    lastName = student.lastName
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
        private HashSet<Student> students;

        [GlobalSetup]
        public void MapSetup()
        {
            var rnd = new Random();
            students = new HashSet<Student>(N);

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
        public Dictionary<long, string> LambdaMap() => students.ToDictionary(s => s.ID, s => $"{s.lastName}, {s.firstName}");

        [Benchmark]
        public Dictionary<long, string> LoopMap()
        {
            var result = new Dictionary<long, string>(students.Count);
            var iter = students.GetEnumerator();

            while(iter.MoveNext())
            {
                var student = iter.Current;
                result.Add(student.ID, $"{student.lastName}, {student.firstName}");
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
