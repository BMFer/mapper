using JabberBoxer.Codomain;
using JabberBoxer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using static System.String;

namespace Some.Domain.ConsoleApp
{
    using static JabberBoxer.Mapping.Lib.F;

    class Program
    {
        const int OneMillion = 1000000;
        static Person[] people;
        static Stopwatch sw = new Stopwatch();

        static void Main(string[] args)
        {
            
            // Create a binding relationship
            Bind<Person, PersonDto>();

            // Collect 1 million person objects
            people = GetPeople(OneMillion).ToArray();

            var dto = MapSingle();
            var peopleList = MapArray();

            MapOneMillionSingles();

            Console.ReadLine();
        }

        static PersonDto MapSingle()
        {
            var person = Person.Create();

            PersonDto dto = null;

            var elapsed = TestPerformance(() => dto = Map<PersonDto>(person));

            Console.WriteLine(Format("MapSingle Elapsed: {0}ms{1}", elapsed, "\n"));

            return dto;
        }

        static IEnumerable<PersonDto>  MapArray()
        {
            IEnumerable<PersonDto> personDtos = null;

            var elapsed = TestPerformance(() => personDtos = Map<PersonDto>(people).ToList());

            Console.WriteLine(Format("MapArray Elapsed: {0}ms{1}", elapsed, "\n"));

            return personDtos;
        }

        static void MapOneMillionSingles()
        {
            var person = Person.Create();

            PersonDto dto = null;

            var elapsed = TestPerformance(() => {
                for (int i = 0; i < OneMillion; i++)
                    dto = Map<PersonDto>(person);
                
            });

            Console.WriteLine(Format("MapOneMillionSingles Elapsed: {0}ms{1}", elapsed, "\n"));
        }


        static protected IEnumerable<Person> GetPeople(int count)
        {
            for (int i = 0; i < count; i++)
                yield return Person.Create(); 
        }

        #region statistics functions

        static private double TestPerformance(Action action)
        {
            sw.Start();
            action();
            sw.Stop();
            double elapsed = sw.Elapsed.TotalMilliseconds;
            sw.Reset();
            return elapsed;
        }
        /// <summary>
        /// Sum a list of double values
        /// </summary>
        /// <param name="list">List of double</param>
        /// <returns>double: summed list</returns>
        static private double Sum(params double[] list)
        {
            double result = 0;

            for (int i = 0; i < list.Length; i++)
            {
                result += list[i];
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static private  double Average(params double[] list)
        {
            double sum = Sum(list);
            double result = (double)sum / list.Length;
            return result;
        }
        #endregion
    }
}
