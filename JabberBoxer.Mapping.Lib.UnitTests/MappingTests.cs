using JabberBoxer.Codomain;
using JabberBoxer.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JabberBoxer.Mapping.Lib.UnitTests
{
    using static F;

    [TestClass]
    public class MappingTests : TestBase
    {

        #region relationship tests

        /*
        * A relationship is used to bring two classes together and create 
        * a 'recreatable' unique id.
        */

        [TestMethod]
        public void Relationship_CanCreate()
        {
            IRelationship relationship = Relate<Person, PersonDto>();
            var actual = RelationshipValidator.IsValid(relationship);

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void Relationship_Can_Recreate_Id()
        {
            IRelationship createdRelationship = Relate<Person, PersonDto>();
            IRelationship recreatedRelationship = Relate<Person, PersonDto>();

            Assert.AreEqual(createdRelationship.Id, recreatedRelationship.Id);
        }

        #endregion-

        #region binding tests

        /*  
         *  Bindings will convert a relationship to a lambda expression then create 
         *  a compiled action and provide access to the unique relationship id.
        */

        [TestMethod]
        public void Binding_CanCreate()
        {
            Binding binding = NewBinding<Person, PersonDto>();
            var actual = BindingValidator.IsValid(binding);

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void Binding_Action_CanMap()
        {
            Binding binding = NewBinding<Person, PersonDto>();
            PersonDto personDto = new PersonDto();

            if (BindingValidator.IsValid(binding))
            {
                binding.GetAction().Invoke(_person, personDto);
            }

            bool actual =
                personDto.FirstName.Equals(_person.FirstName)
                && personDto.LastName.Equals(_person.LastName)
                && personDto.Phone.Equals(_person.Phone)
                && personDto.Nickname.Equals(_person.Nickname);

            Assert.AreEqual(true, actual);
        }

        #endregion

        #region mapping tests

        /*  
         *  Mapping can return a light weight map object.
        */

        [TestMethod]
        public void Mapping_CanCreate()
        {
            Mapping mapping = NewMapping(NewBinding<Person, PersonDto>());

            var actual = MappingValidator.IsValid(mapping);

            Assert.AreEqual(true, actual);
        }

        #endregion

        #region mapping cache tests

        [TestMethod]
        public void MappingCache_CanCacheMultiple()
        {
            // cache a person map

            Binding binding = NewBinding<Person, PersonDto>();
            Mapping mapping = NewMapping(binding);
            CacheMap(CacheMapCommand(mapping.Key, mapping.Map));
            var cachedMapping = GetMap(GetMapCommand(binding.Id));
            PersonDto personDto = new PersonDto();
            cachedMapping.Invoke(_person, personDto);

            var actual = PersonPropertiesMatch(_person, personDto);

            Assert.AreEqual(true, actual);

            binding = NewBinding<Address, AddressDto>();
            mapping = NewMapping(binding);
            CacheMap(CacheMapCommand(mapping.Key, mapping.Map));
            cachedMapping = GetMap(GetMapCommand(binding.Id));
            AddressDto addressDto = new AddressDto();
            cachedMapping.Invoke(_address, addressDto);
            actual = AddressPropertiesMatch(_address, addressDto);

            Assert.AreEqual(true, actual);

        }

        [TestMethod]
        public void MappingCache_Reteival_IsFast()
        {
            Binding personBinding = NewBinding<Person, PersonDto>();
            Mapping personMapping = NewMapping(personBinding);
            CacheMap(CacheMapCommand(personMapping.Key, personMapping.Map));
            RetrieveMapCommand retrievePersonMapCommand = GetMapCommand(personBinding.Id);

            List<double> elapseTimes = new List<double>();

            Action<object, object> map = null;

            for (int i = 0; i < 10; i++)
            {
                double elapsed = TestPerformance(() =>
                    {
                        for (int iMap = 0; iMap < OneMillion; iMap++)
                            map = GetMap(retrievePersonMapCommand);
                    }
                );

                elapseTimes.Add(elapsed);
            }

            var avg = Average(elapseTimes.ToArray());

            Debug.WriteLine(avg);

            var expected = 100;

            var actual = avg < expected;

            Assert.AreEqual(true, actual);

        }

        [TestMethod]
        public void MappingCache_Map_Retrieval_IsFast()
        {
            Binding personBinding = NewBinding<Person, PersonDto>();
            Mapping personMapping = NewMapping(personBinding);
            RetrieveMapCommand retrieveMap = GetMapCommand(personBinding.Id);
            CacheMap(CacheMapCommand(personMapping.Key, personMapping.Map));
            Action<object, object> map = null;

            List<double> elapseTimes = new List<double>();

            for (int i = 0; i < 10; i++)
            {
                double elapsed = TestPerformance(() =>
                {
                    for (int iMap = 0; iMap < OneMillion; iMap++)
                        map = GetMap(retrieveMap);
                });

                elapseTimes.Add(elapsed);
            }

            var avg = Average(elapseTimes.ToArray());

            Debug.WriteLine(avg);

            var expected = 50;

            var actual = avg < expected;

            Assert.AreEqual(true, actual);
        }

        #endregion

        #region mapper tests

        [TestMethod]
        public void Mapper_CanMap()
        {
            Bind<Person, PersonDto>();

            var personDto = Map<PersonDto>(_person);

            Assert.IsTrue(PersonPropertiesMatch(_person, personDto));
        }

        [TestMethod]
        public void Mapper_CanMap_Fast()
        {
            Bind<Person, PersonDto>();

            object personDto = null;

            List<double> elapsedTimes = new List<double>();

            // Run 10 times to so an average can be calculated.
            for (int i = 0; i < 10; i++)
            {
                double elapsed = TestPerformance(
                    () =>
                    {
                        for (int ii = 0; ii < OneMillion; ii++)
                            personDto = Map<PersonDto>(_person);

                    });

                elapsedTimes.Add(elapsed);

            }

            double avg = Average(elapsedTimes.ToArray());

            Debug.WriteLine(avg);

            double expected = 1000;
            bool actual = avg < expected;

            Assert.AreEqual(true, actual);

        }

        [TestMethod]
        public void Mapper_CanMap_Array()
        {
            Bind<Person, PersonDto>();

            var people = GetPeople(OneMillion).ToArray();

            List<double> elapsedTimes = new List<double>();

            List<IEnumerable<PersonDto>> bigList = new List<IEnumerable<PersonDto>>(); ;
            IEnumerable<PersonDto> personDtos = null;
            // Run 10 times to so an average can be calculated.
            for (int i = 0; i < 10; i++)
            {
                double elapsed = TestPerformance(
                    () =>
                    {

                        personDtos = Map<PersonDto>(people);
                        bigList.Add(personDtos);

                    });

                elapsedTimes.Add(elapsed);
            }

            double avg = Average(elapsedTimes.ToArray());

            // 10 list of 1 million personDto objects.
            var lists = bigList.Select(x => x.AsParallel().ToList()).ToList();


            Debug.WriteLine(avg);

            // var result = personDtos;

        }

        #endregion

    }
}
