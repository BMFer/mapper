using Microsoft.VisualStudio.TestTools.UnitTesting;
using JabberBoxer.Codomain;
using JabberBoxer.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JabberBoxer.Mapping.Lib.UnitTests
{
    [TestClass]
    public abstract class TestBase
    {
        protected Stopwatch sw;
        protected Person _person;
        protected Address _address;

        protected const int OneMillion = 1000000;

        #region test helpers

        protected Func<Person, PersonDto, bool> PersonPropertiesMatch = (p, dto) => {

            if (dto.Id.Equals(p.Id) && dto.FirstName.Equals(p.FirstName)
            && dto.LastName.Equals(p.LastName) && dto.Phone.Equals(p.Phone)
            && dto.Email.Equals(p.Email))
                return true;

            return false;
        };

        protected Func<Address, AddressDto, bool> AddressPropertiesMatch = (a, dto) => {

            if (dto.Street.Equals(a.Street) && dto.City.Equals(a.City)
            && dto.State.Equals(a.State) && dto.Zip.Equals(a.Zip))
                return true;

            return false;
        };

        protected IEnumerable<Person> GetPeople(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var p = Person.Create();
                p.Id = Guid.NewGuid();
                yield return p;
            }
        }

        #endregion

        #region validators

        protected RelationshipValidator RelationshipValidator => new RelationshipValidator();
        protected BindingValidator BindingValidator => new BindingValidator();

        protected MappingValidator MappingValidator => new MappingValidator();

        #endregion

        #region lifecycle

        [TestInitialize]
        public void Init()
        {
            sw = new Stopwatch();

            _person = Person.Create();
            _address = Address.Create();
        }

        [TestCleanup]
        public void Kill()
        {
            _person = null;
            _address = null;
            sw = null;
        }

        #endregion

        #region statistics functions

        protected double TestPerformance(Action action)
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
        protected double Sum(params double[] list)
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
        protected double Average(params double[] list)
        {
            double sum = Sum(list);
            double result = (double)sum / list.Length;
            return result;
        }
        #endregion
    }
}
