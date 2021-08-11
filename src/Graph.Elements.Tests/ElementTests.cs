using System;
using Xunit;


namespace Graph.Elements.Tests
{
    public class ElementTests
    {
        private class ConcreteElement : Element
        {
            public override object Clone()
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void Element_Default_Constructor_Succeeds()
        {
            var element = new ConcreteElement();
            Assert.NotNull(element);
            Assert.NotEqual(Guid.Empty, element.Id);
            Assert.Empty(element.Classes);
            Assert.Empty(element.Attributes);
        }

        [Fact]
        public void Element_Classify_Single_Succeeds()
        {
            var element = new ConcreteElement();
            Assert.NotNull(element);

            var label = "class";
            element.Classify(label);
            Assert.NotEmpty(element.Classes);
            Assert.True(element.Is(label));
        }

        [Fact]
        public void Element_Classify_Single_Twice_Succeeds()
        {
            var element = new ConcreteElement();
            Assert.NotNull(element);

            var label = "class";
            element.Classify(label);
            element.Classify(label);
            Assert.NotEmpty(element.Classes);
            Assert.Single(element.Classes);
            Assert.True(element.Is(label));
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

            var labels = new string[] { "class1", "class2" };
            element.Classify(labels);
            Assert.NotEmpty(element.Classes);
            Assert.True(element.Is(labels[0]));
            Assert.True(element.Is(labels[1]));
        }

        [Fact]
        public void Element_Declassify_Succeeds()
        {
            var element = new ConcreteElement();
            Assert.NotNull(element);

            var label = "class";
            element.Classify(label);
            Assert.NotEmpty(element.Classes);
            Assert.True(element.Is(label));
            element.Declassify(label);
            Assert.False(element.Is(label));
        }

        [Fact]
        public void Element_Declassify_Twice_Succeeds()
        {
            var element = new ConcreteElement();
            Assert.NotNull(element);

            var label = "class";
            element.Classify(label);
            Assert.NotEmpty(element.Classes);
            Assert.True(element.Is(label));
            element.Declassify(label);
            element.Declassify(label);
            Assert.False(element.Is(label));
        }

        [Fact]
        public void Element_Classify_Enumerable_Succeeds_And_Ignores_Duplicate()
        {
            var element = new ConcreteElement();
            Assert.NotNull(element);

            var labels = new string[] { "class1", "class1" };
            element.Classify(labels);
            Assert.NotEmpty(element.Classes);
            Assert.Single(element.Classes);
            Assert.True(element.Is(labels[0]));
        }
    }

}
