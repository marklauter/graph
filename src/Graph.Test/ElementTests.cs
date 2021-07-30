using Graph.Elements;
using System;
using Xunit;

namespace Graph.Test
{
    public class ElementTests
    {
        private class ConcreteElement : Element { }

        [Fact]
        public void Element_Default_Constructor_Succeeds()
        {
            var element = new ConcreteElement();
            Assert.NotNull(element);
            Assert.NotEqual(Guid.Empty, element.Id);
            Assert.Empty(element.Classifications);
            Assert.Empty(element.Features);
        }

        [Fact]
        public void Element_Classify_Single_Succeeds()
        {
            var element = new ConcreteElement();
            Assert.NotNull(element);

            var classification = "class";
            element.Classify(classification);
            Assert.NotEmpty(element.Classifications);
            Assert.Equal(classification, element.Classifications[0]);
        }

        [Fact]
        public void Element_Classify_Single_Fails_NullOrEmpty_Check()
        {
            var element = new ConcreteElement();
            Assert.NotNull(element);

            var classification = String.Empty;
            Assert.Throws<ArgumentException>(() => element.Classify(classification));
        }

        [Fact]
        public void Element_Classify_Enumerable_Succeeds()
        {
            var element = new ConcreteElement();
            Assert.NotNull(element);

            var classifications = new string[] { "class1", "class2" };
            element.Classify(classifications);
            Assert.NotEmpty(element.Classifications);
            Assert.Equal(classifications[0], element.Classifications[0]);
            Assert.Equal(classifications[1], element.Classifications[1]);
        }

        [Fact]
        public void Element_Classify_Enumerable_Succeeds_And_Ignores_Duplicate()
        {
            var element = new ConcreteElement();
            Assert.NotNull(element);

            var classifications = new string[] { "class1", "class1" };
            element.Classify(classifications);
            Assert.NotEmpty(element.Classifications);
            Assert.Equal(1, element.Classifications.Count);
            Assert.Equal(classifications[0], element.Classifications[0]);
        }
    }
}
