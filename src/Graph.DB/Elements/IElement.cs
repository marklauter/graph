using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Graphs.DB.Elements
{
    public interface IElement<TId>
        : ICloneable
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        [Key]
        TId Id { get; }

        /// <summary>
        /// Gets the value of an attribute.
        /// </summary>
        /// <param name="name">Name of the attribute.</param>
        /// <returns>Attribute value.</returns>
        /// <remarks>
        /// <see cref="Qualify(String, String)"/>
        /// <seealso cref="Qualify(IDictionary{String, String})"/>
        /// <seealso cref="HasAttribute(String)"/>
        /// </remarks>
        string Attribute(string name);

        /// <summary>
        /// Assigns a label to the element.
        /// </summary>
        /// <param name="label"></param>
        /// <returns><see cref="IElement{TId}"/> for fluent access.</returns>
        /// <remarks>
        /// <seealso cref="Classify(IEnumerable{String})"/>
        /// <seealso cref="Declassify(String)"/>
        /// <seealso cref="IsClass(String)"/>
        /// </remarks>
        IElement<TId> Classify(string label);

        /// <summary>
        /// Assigns a set of labels to the element.
        /// </summary>
        /// <param name="label"></param>
        /// <returns><see cref="IElement{TId}"/> for fluent access.</returns>
        /// <remarks>
        /// <see cref="Classify(String)"/>
        /// <seealso cref="Declassify(String)"/>
        /// <seealso cref="IsClass(String)"/>
        /// </remarks>
        IElement<TId> Classify(IEnumerable<string> labels);

        /// <summary>
        /// Unassign a label from the element.
        /// </summary>
        /// <param name="label"></param>
        /// <returns><see cref="IElement{TId}"/> for fluent access.</returns>
        /// <remarks>
        /// <seealso cref="Classify(String)"/>
        /// <seealso cref="Classify(IEnumerable{String})"/>
        /// <seealso cref="IsClass(String)"/>
        /// </remarks>
        IElement<TId> Declassify(string label);

        /// <summary>
        /// Checks the map of attribute name-value pairs for a matching name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the element contains an attribute matching the name parameter.</returns>
        /// <remarks>
        /// <seealso cref="Attribute(String)"/>
        /// <seealso cref="Qualify(String, String)"/>
        /// <seealso cref="Qualify(IDictionary{String, String})"/>
        /// HasA
        /// </remarks>
        bool HasAttribute(string name);

        /// <summary>
        /// Checks the element for a classification matching the label.
        /// </summary>
        /// <param name="label"></param>
        /// <returns>True if the element contains a classification that matches the label.</returns>
        /// <remarks>
        /// <seealso cref="Classify(IEnumerable{String})"/>
        /// <seealso cref="Classify(IEnumerable{String})"/>
        /// <seealso cref="Declassify(String)"/>
        /// IsA
        /// </remarks>
        bool IsClass(string label);

        /// <summary>
        /// Sets adds an attribute, in the form of a name-value pair, to the element.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns><see cref="IElement{TId}"/> for fluent access.</returns>
        /// <remarks>
        /// <seealso cref="Qualify(IDictionary{String, String})"/>
        /// <seealso cref="Attribute(String)"/>
        /// <seealso cref="HasAttribute(String)"/>
        /// </remarks>
        IElement<TId> Qualify(string name, string value);

        /// <summary>
        /// Apppends a set of attributes to the element.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns><see cref="IElement{TId}"/> for fluent access.</returns>
        /// <remarks>
        /// <seealso cref="Qualify(String, String)"/>
        /// <seealso cref="Attribute(String)"/>
        /// <seealso cref="HasAttribute(String)"/>
        /// </remarks>
        IElement<TId> Qualify(IEnumerable<KeyValuePair<string, string>> attributes);
    }
}
