using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace DictionaryBenchmarks
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
        private readonly Dictionary<string, Student> students;
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
            students = new Dictionary<string, Student>(N);

            for (int i = 1; i <= N; i++)
            {
                var student = new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = $"{firstNames[rnd.Next(0, firstNames.Count)]}",
                    lastName = $"{lastNames[rnd.Next(0, lastNames.Count)]}"
                };

                var key = $"i - {student.firstName[0]}{student.lastName[0]}{student.ID}";
                students.Add(key, student);
            }
        }

        [Benchmark]
        public string LinqAggregate() =>
            students.Aggregate(
                new StringBuilder(),
                (sb, kvp) => sb.AppendFormat
                (
                    "{0} : {1},{2} - {3}",
                    kvp.Key,
                    kvp.Value.firstName,
                    kvp.Value.lastName,
                    (kvp.Value.average > 60) ? kvp.Value.average.ToString() : "Failed"
                ),
                sb => sb.ToString());

        [Benchmark]
        public string LoopAggregate()
        {
            var builder = new StringBuilder(students.Count);
            var iter = students.GetEnumerator();

            while (iter.MoveNext())
            {
                var kvp = iter.Current;
                builder.AppendFormat
                    (
                        "{0} : {1},{2} - {3}",
                        kvp.Key,
                        kvp.Value.firstName,
                        kvp.Value.lastName,
                        (kvp.Value.average > 60) ? kvp.Value.average.ToString() : "Failed"
                    );
            }

            return builder.ToString();
        }

        [Benchmark]
        public string IteratorAggregate()
        {
            var builder = new StringBuilder();
            foreach (var kvp in students)
            {
                builder.AppendFormat
                    (
                        "{0} : {1},{2} - {3}",
                        kvp.Key,
                        kvp.Value.firstName,
                        kvp.Value.lastName,
                        (kvp.Value.average > 60) ? kvp.Value.average.ToString() : "Failed"
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
        public Dictionary<string, Student> LinqPopulate()
        {
            var rnd = new Random();
            return students.Select(s => new Student()
            {
                average = rnd.Next(50, 101),
                ID = s * N,
                firstName = firstNames[rnd.Next(0, firstNames.Count)],
                lastName = lastNames[rnd.Next(0, lastNames.Count)]
            }).ToDictionary(s => $"i - {s.firstName[0]}{s.lastName[0]}{s.ID}", s => s);
        }

        [Benchmark]
        public Dictionary<string, Student> LoopPopulate()
        {
            var rnd = new Random();
            var result = new Dictionary<string, Student>(N);

            for (int i = 1; i <= N; i++)
            {
                var student = new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = firstNames[rnd.Next(0, firstNames.Count)],
                    lastName = lastNames[rnd.Next(0, lastNames.Count)]
                };

                var key = $"i - {student.firstName[0]}{student.lastName[0]}{student.ID}";
                result.Add(key, student);
            }

            return result;
        }

        [Benchmark]
        public Dictionary<string, Student> IteratorPopulate()
        {
            var rnd = new Random();
            var result = new Dictionary<string, Student>(N);

            foreach (var s in students)
            {
                var student = new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = s * N,
                    firstName = firstNames[rnd.Next(0, firstNames.Count)],
                    lastName = lastNames[rnd.Next(0, lastNames.Count)]
                };

                var key = $"i - {student.firstName[0]}{student.lastName[0]}{student.ID}";
                result.Add(key, student);
            }

            return result;
        }
    }

    [MemoryDiagnoser]
    public class IterateStudent
    {
        private const int N = 1_000_000;
        private readonly Dictionary<string, Student> students;
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
            students = new Dictionary<string, Student>(N);

            for (int i = 1; i <= N; i++)
            {
                var student = new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = $"{firstNames[rnd.Next(0, firstNames.Count)]}",
                    lastName = $"{lastNames[rnd.Next(0, lastNames.Count)]}"
                };

                var key = $"i - {student.firstName[0]}{student.lastName[0]}{student.ID}";
                students.Add(key, student);
            }
        }

        [Benchmark]
        public int LinqIterate()
        {
            return students.Count(kvp =>
                kvp.Key.Length > 0 &&
                kvp.Key.Contains('-') &&
                kvp.Value.average >= 50 &&
                kvp.Value.ID < long.MaxValue);
        }

        [Benchmark]
        public int LoopIterate()
        {
            static bool valid(KeyValuePair<string, Student> kvp)
            {
                return kvp.Key.Length > 0 &&
                        kvp.Key.Contains('-') &&
                        kvp.Value.average >= 50 &&
                        kvp.Value.ID < long.MaxValue;
            }

            int count = 0;
            var iter = students.GetEnumerator();

            while (iter.MoveNext())
            {
                var kvp = iter.Current;
                if (valid(kvp)) count++;
            }

            return count;
        }

        [Benchmark]
        public int IteratorIterate()
        {
            static bool valid(KeyValuePair<string, Student> kvp)
            {
                return kvp.Key.Length > 0 &&
                        kvp.Key.Contains('-') &&
                        kvp.Value.average >= 50 &&
                        kvp.Value.ID < long.MaxValue;
            }

            int count = 0;
            foreach (var kvp in students)
            {
                if (valid(kvp)) count++;
            }

            return count;
        }
    }

    [MemoryDiagnoser]
    public class ContainsStudent
    {
        private const int N = 1_000_000;
        private readonly Dictionary<string, Student> students;
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
            students = new Dictionary<string, Student>(N);

            for (int i = 1; i <= N; i++)
            {
                var student = new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = $"{firstNames[rnd.Next(0, firstNames.Count)]}",
                    lastName = $"{lastNames[rnd.Next(0, lastNames.Count)]}"
                };

                var key = $"i - {student.firstName[0]}{student.lastName[0]}{student.ID}";
                students.Add(key, student);
            }
        }

        [Benchmark]
        public KeyValuePair<string, Student> LinqContains()
        {
            return students.First(kvp =>
                        kvp.Value.average >= 70 &&
                        kvp.Value.average <= 85 &&
                        kvp.Value.firstName.Contains(' ') &&
                        kvp.Value.lastName.Contains("ez"));
        }

        [Benchmark]
        public KeyValuePair<string, Student> LoopContains()
        {
            static bool IsTarget(KeyValuePair<string, Student> kvp)
            {
                return kvp.Value.average >= 70 &&
                        kvp.Value.average <= 85 &&
                        kvp.Value.firstName.Contains(' ') &&
                        kvp.Value.lastName.Contains("ez");
            }

            var iter = students.GetEnumerator();

            while (iter.MoveNext())
            {
                var kvp = iter.Current;
                if (IsTarget(kvp)) return kvp;
            }

            return default;
        }

        [Benchmark]
        public KeyValuePair<string, Student> IteratorContains()
        {
            static bool IsTarget(KeyValuePair<string, Student> kvp)
            {
                return kvp.Value.average >= 70 &&
                        kvp.Value.average <= 85 &&
                        kvp.Value.firstName.Contains(' ') &&
                        kvp.Value.lastName.Contains("ez");
            }

            foreach (var kvp in students)
            {
                if (IsTarget(kvp)) return kvp;
            }

            return default;
        }
    }

    [MemoryDiagnoser]
    public class FilterStudent
    {
        private const int N = 1_000_000;
        private readonly Consumer consumer;
        private readonly Dictionary<string, Student> students;
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

            consumer = new Consumer();
            students = new Dictionary<string, Student>(N);

            for (int i = 1; i <= N; i++)
            {
                var student = new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = $"{firstNames[rnd.Next(0, firstNames.Count)]}",
                    lastName = $"{lastNames[rnd.Next(0, lastNames.Count)]}"
                };

                var key = $"i - {student.firstName[0]}{student.lastName[0]}{student.ID}";
                students.Add(key, student);
            }
        }

        [Benchmark]
        public void LinqFilter()
        {
            students.Where(kvp =>
                    kvp.Value.average >= 70 &&
                    (kvp.Value.firstName.Contains(' ') || kvp.Value.lastName.Contains(' ')) &&
                    kvp.Value.ID >= 3 * N).Consume(consumer);
        }

        [Benchmark]
        public void LoopFilter()
        {
            static bool IsContained(KeyValuePair<string, Student> kvp)
            {
                return kvp.Value.average >= 70 &&
                        (kvp.Value.firstName.Contains(' ') || kvp.Value.lastName.Contains(' ')) &&
                        kvp.Value.ID >= 3 * N;
            }

            var result = new List<KeyValuePair<string, Student>>();
            var iter = students.GetEnumerator();

            while (iter.MoveNext())
            {
                var kvp = iter.Current;
                if (IsContained(kvp)) result.Add(kvp);
            }

            result.Consume(consumer);
        }

        [Benchmark]
        public void IteratorFilter()
        {
            static bool IsContained(KeyValuePair<string, Student> kvp)
            {
                return kvp.Value.average >= 70 &&
                        (kvp.Value.firstName.Contains(' ') || kvp.Value.lastName.Contains(' ')) &&
                        kvp.Value.ID >= 3 * N;
            }

            var result = new List<KeyValuePair<string, Student>>();
            foreach (var kvp in students)
            {
                if (IsContained(kvp)) result.Add(kvp);
            }

            result.Consume(consumer);
        }
    }

    [MemoryDiagnoser]
    public class CopyStudent
    {
        private const int N = 1_000_000;
        private readonly Dictionary<string, Student> students;
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
            students = new Dictionary<string, Student>(N);

            for (int i = 1; i <= N; i++)
            {
                var student = new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = $"{firstNames[rnd.Next(0, firstNames.Count)]}",
                    lastName = $"{lastNames[rnd.Next(0, lastNames.Count)]}"
                };

                var key = $"i - {student.firstName[0]}{student.lastName[0]}{student.ID}";
                students.Add(key, student);
            }
        }

        [Benchmark]
        public Dictionary<string, Student> LinqCopy() =>
            students.Select(kvp =>
                new Student()
                {
                    average = kvp.Value.average,
                    ID = kvp.Value.ID,
                    firstName = kvp.Value.firstName,
                    lastName = kvp.Value.lastName
                }).ToDictionary(k => $"i - {k.firstName[0]}{k.lastName[0]}{k.ID}", v => v);

        [Benchmark]
        public Dictionary<string, Student> LoopCopy()
        {
            var copy = new Dictionary<string, Student>(students.Count);
            var iter = students.GetEnumerator();

            while (iter.MoveNext())
            {
                var kvp = iter.Current;
                var student = new Student()
                {
                    average = kvp.Value.average,
                    ID = kvp.Value.ID,
                    firstName = kvp.Value.firstName,
                    lastName = kvp.Value.lastName
                };
                var key = $"i - {student.firstName[0]}{student.lastName[0]}{student.ID}";
                copy.Add(key, student);
            }

            return copy;
        }

        [Benchmark]
        public Dictionary<string, Student> IteratorCopy()
        {
            var copy = new Dictionary<string, Student>(students.Count);
            foreach (var kvp in students)
            {
                var student = new Student()
                {
                    average = kvp.Value.average,
                    ID = kvp.Value.ID,
                    firstName = kvp.Value.firstName,
                    lastName = kvp.Value.lastName
                };
                var key = $"i - {student.firstName[0]}{student.lastName[0]}{student.ID}";
                copy.Add(key, student);
            }

            return copy;
        }
    }

    [MemoryDiagnoser]
    public class MapStudent
    {
        private const int N = 1_000_000;
        private readonly Dictionary<string, Student> students;
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
            students = new Dictionary<string, Student>(N);

            for (int i = 1; i <= N; i++)
            {
                var student = new Student()
                {
                    average = rnd.Next(50, 101),
                    ID = i * N,
                    firstName = $"{firstNames[rnd.Next(0, firstNames.Count)]}",
                    lastName = $"{lastNames[rnd.Next(0, lastNames.Count)]}"
                };

                var key = $"i - {student.firstName[0]}{student.lastName[0]}{student.ID}";
                students.Add(key, student);
            }
        }

        [Benchmark]
        public List<KeyValuePair<int, string>> LinqMap() =>
                students.Select((kvp, index) =>
                    new KeyValuePair<int, string>(index, $"{kvp.Value.lastName},{kvp.Value.firstName} - {kvp.Value.average}")).ToList();

        [Benchmark]
        public List<KeyValuePair<int, string>> LoopMap()
        {
            var result = new List<KeyValuePair<int, string>>(students.Count);
            var iter = students.GetEnumerator();

            for (int i = 0; iter.MoveNext(); i++)
            {
                var kvp = iter.Current;
                result.Add(new KeyValuePair<int, string>(i, $"{kvp.Value.lastName},{kvp.Value.firstName} - {kvp.Value.average}"));
            }

            return result;
        }

        [Benchmark]
        public List<KeyValuePair<int, string>> IteratorMap()
        {
            var result = new List<KeyValuePair<int, string>>(students.Count);
            int i = 0;

            foreach (var kvp in students)
            {
                result.Add(new KeyValuePair<int, string>(i, $"{kvp.Value.lastName},{kvp.Value.firstName} - {kvp.Value.average}"));
                i++;
            }

            return result;
        }
    }
}
