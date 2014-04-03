using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using NUnit.Framework;

namespace Implementation.Test
{
    [TestFixture]
    public class RandomizerTest
    {
        [Test]
        [TestCase(0.65435234523452)]
        [TestCase(66354)]
        [TestCase(2147483647648768)]
        [TestCase("allo")]
        public void RandomizeWithPrimitiveType(object value)
        {
            var result = Randomizer.Randomize(value);

            Assert.AreNotEqual(result, value);
        }

        [Test]
        public void RandomizeClass()
        {
            var foo = new Foo();
            var prototype = new Foo();

            foo = Randomizer.Randomize(foo);

            Assert.IsFalse(foo.Equals(prototype));
        }

        [Test]
        public void RandomizeListFoo()
        {
            var foos = new List<Foo>();
            var prototype = new List<Foo>();

            foos = Randomizer.Randomize(foos);

            Assert.IsFalse(foos.SequenceEqual(prototype));
        }

        [Test]
        public void RandomizeListBar()
        {
            var bars = new List<Bar>() as IList<Bar>;
            var prototype = new List<Bar>() as IList<Bar>;

            bars = Randomizer.Randomize(bars);

            Assert.IsFalse(bars.SequenceEqual(prototype));
        }

        [Test]
        public void RandomizeListListBar()
        {
            var bars = new List<List<Bar>>();
            var prototype = new List<List<Bar>>();

            bars = Randomizer.Randomize(bars);

            Assert.IsFalse(bars.SequenceEqual(prototype));
        }

        [Test, ExpectedException(typeof(NotImplementedException))]
        public void RandomizeTypeNotFound()
        {
            Randomizer.Randomize('c');

            Assert.Fail("Randomize should throw NotImplementedException");
        }
    }
}

