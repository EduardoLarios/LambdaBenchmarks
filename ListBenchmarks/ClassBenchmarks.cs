using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace ListBenchmarks
{
    public class Student
    {
        public int average;
        public long ID;
        public string firstName;
        public string lastName;
    }

    [MemoryDiagnoser]
    public class ReduceStudent
    {
        private const int N = 1_000_000;
        private readonly List<Student> students;
        private readonly List<string> firstNames = new List<string>()
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
        private readonly List<string> lastNames = new List<string>()
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

        public ReduceStudent()
        {
            var rnd = new Random();
            students = new List<Student>(N);

            for (int i = 1; i <= N; i++)
            {
                students.Add(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = firstNames[rnd.Next(0, firstNames.Count)],
                    lastName = lastNames[rnd.Next(0, lastNames.Count)]
                });
            }
        }

        [Benchmark]
        public string LinqAggregate() =>
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
        public string LoopAggregate()
        {
            var builder = new StringBuilder(students.Count);
            for (int i = 0; i < students.Count; i++)
            {
                builder.AppendFormat
                    (
                        "{0}, {1} - {2}",
                        students[i].lastName,
                        students[i].firstName,
                        (students[i].average > 60) ? students[i].average.ToString() : "Failed"
                    );
            }

            return builder.ToString();
        }

        [Benchmark]
        public string IteratorAggregate()
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
        private const int N = 1_000_000;
        private readonly IEnumerable<int> students;
        private readonly List<string> firstNames = new List<string>()
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
        private readonly List<string> lastNames = new List<string>()
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

        public PopulateStudent()
        {
            students = Enumerable.Range(1, N);
        }

        [Benchmark]
        public List<Student> LinqPopulate()
        {
            var rnd = new Random();
            return students.Select(n => new Student()
            {
                average = rnd.Next(50, 101),
                ID = n * N,
                firstName = firstNames[rnd.Next(0, firstNames.Count)],
                lastName = lastNames[rnd.Next(0, lastNames.Count)]
            }).ToList();
        }

        [Benchmark]
        public List<Student> LoopPopulate()
        {
            var rnd = new Random();
            var result = new List<Student>(N);

            for (int i = 1; i <= N; i++)
            {
                result.Add(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = firstNames[rnd.Next(0, firstNames.Count)],
                    lastName = lastNames[rnd.Next(0, lastNames.Count)]
                });
            }

            return result;
        }

        [Benchmark]
        public List<Student> IteratorPopulate()
        {
            var rnd = new Random();
            var result = new List<Student>(N);

            foreach (var n in students)
            {
                result.Add(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = n * N,
                    firstName = firstNames[rnd.Next(0, firstNames.Count)],
                    lastName = lastNames[rnd.Next(0, lastNames.Count)]
                });
            }

            return result;
        }
    }

    [MemoryDiagnoser]
    public class IterateStudent
    {
        private const int N = 1_000_000;
        private readonly List<Student> students;
        private readonly List<string> firstNames = new List<string>()
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
        private readonly List<string> lastNames = new List<string>()
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

        public IterateStudent()
        {
            var rnd = new Random();
            students = new List<Student>(N);

            for (int i = 1; i <= N; i++)
            {
                students.Add(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = firstNames[rnd.Next(0, firstNames.Count)],
                    lastName = lastNames[rnd.Next(0, lastNames.Count)]
                });
            }
        }

        [Benchmark]
        public int LinqIterate() => students.Count(n => n.firstName.Length > 0 && n.average >= 50 && n.ID < long.MaxValue);

        [Benchmark]
        public int LoopIterate()
        {
            int count = 0;
            static bool valid(int len, int avg, long id) => len > 0 && avg >= 50 && id < long.MaxValue;

            for (int i = 0; i < students.Count; i++)
            {
                var s = students[i];

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
        private const int N = 1_000_000;
        private readonly List<Student> students;
        private readonly List<string> firstNames = new List<string>()
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
        private readonly List<string> lastNames = new List<string>()
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

        public ContainsStudent()
        {
            var rnd = new Random();
            students = new List<Student>(N);

            for (int i = 1; i <= N; i++)
            {
                students.Add(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = firstNames[rnd.Next(0, firstNames.Count)],
                    lastName = lastNames[rnd.Next(0, lastNames.Count)]
                });
            }
        }

        [Benchmark]
        public Student LinqContains() => students.Find(s => s.average >= 70 && s.average <= 85 && s.firstName.Contains(' ') && s.lastName.Contains("es"));

        [Benchmark]
        public Student LoopContains()
        {
            for (int i = 0; i < students.Count; i++)
            {
                var s = students[i];
                if (s.average >= 70 && s.average <= 85 && s.firstName.Contains(' ') && s.lastName.Contains("es"))
                {
                    return s;
                }
            }

            return null;
        }

        [Benchmark]
        public Student IteratorContains()
        {
            foreach (var s in students)
            {
                if (s.average >= 70 && s.average <= 85 && s.firstName.Contains(' ') && s.lastName.Contains("es"))
                {
                    return s;
                }
            }

            return null;
        }
    }

    [MemoryDiagnoser]
    public class FilterStudent
    {
        private const int N = 1_000_000;
        private readonly int target;
        private readonly Consumer consumer;
        private readonly List<Student> students;
        private readonly List<string> firstNames = new List<string>()
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
        private readonly List<string> lastNames = new List<string>()
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

        public FilterStudent()
        {
            var rnd = new Random();

            students = new List<Student>(N);
            consumer = new Consumer();
            target = rnd.Next(1, N / 2);

            for (int i = 1; i <= N; i++)
            {
                students.Add(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = firstNames[rnd.Next(0, firstNames.Count)],
                    lastName = lastNames[rnd.Next(0, lastNames.Count)]
                });
            }
        }

        [Benchmark]
        public void LinqFilter() => students.Where(s => s.average > 50 && s.average < 70 && s.firstName.Contains('i', StringComparison.InvariantCulture) && s.ID > target).Consume(consumer);

        [Benchmark]
        public void LoopFilter()
        {
            var result = new List<Student>();
            for (int i = 0; i < students.Count; i++)
            {
                var s = students[i];
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
        private const int N = 1_000_000;
        private readonly List<Student> students;
        private readonly List<string> firstNames = new List<string>()
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
        private readonly List<string> lastNames = new List<string>()
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

        public CopyStudent()
        {
            var rnd = new Random();

            students = new List<Student>(N);

            for (int i = 1; i <= N; i++)
            {
                students.Add(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = firstNames[rnd.Next(0, firstNames.Count)],
                    lastName = lastNames[rnd.Next(0, lastNames.Count)]
                });
            }
        }

        [Benchmark]
        public List<Student> LinqCopy() => students.Select(s => new Student() { average = s.average, ID = s.ID, firstName = s.firstName, lastName = s.lastName }).ToList();

        [Benchmark]
        public List<Student> LoopCopy()
        {
            var copy = new List<Student>(students.Count);
            for (int i = 0; i < students.Count; i++)
            {
                copy.Add(new Student()
                {
                    average = students[i].average,
                    ID = students[i].ID,
                    firstName = students[i].firstName,
                    lastName = students[i].lastName
                });
            }

            return copy;
        }

        [Benchmark]
        public List<Student> IteratorCopy()
        {
            var copy = new List<Student>(students.Count);
            foreach (var s in students)
            {
                copy.Add(new Student()
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
        private const int N = 1_000_000;
        private readonly List<Student> students;
        private readonly List<string> firstNames = new List<string>()
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
        private readonly List<string> lastNames = new List<string>()
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

        public MapStudent()
        {
            var rnd = new Random();
            students = new List<Student>(N);

            for (int i = 1; i <= N; i++)
            {
                students.Add(new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = firstNames[rnd.Next(0, firstNames.Count)],
                    lastName = lastNames[rnd.Next(0, lastNames.Count)]
                });
            }
        }

        [Benchmark]
        public Dictionary<long, string> LinqMap() => students.ToDictionary(s => s.ID, s => $"{s.lastName}, {s.firstName}");

        [Benchmark]
        public Dictionary<long, string> LoopMap()
        {
            var result = new Dictionary<long, string>(students.Count);
            for (int i = 0; i < students.Count; i++)
            {
                result.Add(students[i].ID, $"{students[i].lastName}, {students[i].firstName}");
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
