using System;
using System.Collections.Generic;

namespace Graphs.DB.Elements
{
    public interface IElement
        : ICloneable
    {
        /// <summary>
        /// Gets the value of an attribute.
        /// </summary>
        /// <param name="name">Name of the attribute.</param>
        /// <returns>Attribute value.</returns>
        /// <remarks>
        /// <see cref="Qualify(String, String)"/>
        /// <seealso cref="Qualify(IDictionary{string, string})"/>
        /// <seealso cref="HasAttribute(string)"/>
        /// </remarks>
        public string Attribute(string name);

        /// <summary>
        /// Assigns a label to the element.
        /// </summary>
        /// <param name="label"></param>
        /// <returns><see cref="IElement{TId}"/> for fluent access.</returns>
        /// <remarks>
        /// <seealso cref="Classify(IEnumerable{string})"/>
        /// <seealso cref="Declassify(string)"/>
        /// <seealso cref="Is(string)"/>
        /// </remarks>
        public IElement Classify(string label);

        /// <summary>
        /// Assigns a set of labels to the element.
        /// </summary>
        /// <param name="label"></param>
        /// <returns><see cref="IElement{TId}"/> for fluent access.</returns>
        /// <remarks>
        /// <see cref="Classify(string)"/>
        /// <seealso cref="Declassify(string)"/>
        /// <seealso cref="Is(string)"/>
        /// </remarks>
        public IElement Classify(IEnumerable<string> labels);

        /// <summary>
        /// Unassign a label from the element.
        /// </summary>
        /// <param name="label"></param>
        /// <returns><see cref="IElement{TId}"/> for fluent access.</returns>
        /// <remarks>
        /// <seealso cref="Classify(string)"/>
        /// <seealso cref="Classify(IEnumerable{string})"/>
        /// <seealso cref="Is(string)"/>
        /// </remarks>
        public IElement Declassify(string label);

        /// <summary>
        /// Checks the map of attribute name-value pairs for a matching name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the element contains an attribute matching the name parameter.</returns>
        /// <remarks>
        /// <seealso cref="Attribute(string)"/>
        /// <seealso cref="Qualify(string, string)"/>
        /// <seealso cref="Qualify(IDictionary{string, string})"/>
        /// </remarks>
        public bool HasAttribute(string name);

        /// <summary>
        /// Checks the element for a classification matching the label.
        /// </summary>
        /// <param name="label"></param>
        /// <returns>True if the element contains a classification that matches the label.</returns>
        /// <remarks>
        /// <seealso cref="Classify(IEnumerable{string})"/>
        /// <seealso cref="Classify(IEnumerable{string})"/>
        /// <seealso cref="Declassify(string)"/>
        /// </remarks>
        public bool Is(string label);

        /// <summary>
        /// Sets adds an attribute, in the form of a name-value pair, to the element.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns><see cref="IElement{TId}"/> for fluent access.</returns>
        /// <remarks>
        /// <seealso cref="Qualify(IDictionary{string, string})"/>
        /// <seealso cref="Attribute(string)"/>
        /// <seealso cref="HasAttribute(string)"/>
        /// </remarks>
        public IElement Qualify(string name, string value);

        /// <summary>
        /// Apppends a set of attributes to the element.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns><see cref="IElement{TId}"/> for fluent access.</returns>
        /// <remarks>
        /// <seealso cref="Qualify(string, string)"/>
        /// <seealso cref="Attribute(string)"/>
        /// <seealso cref="HasAttribute(string)"/>
        /// </remarks>
        public IElement Qualify(IEnumerable<KeyValuePair<string, string>> attributes);
    }
}
