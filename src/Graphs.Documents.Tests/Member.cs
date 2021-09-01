using System;
using System.ComponentModel.DataAnnotations;

namespace Graphs.Documents.Tests
{
    public sealed class Member
    {
        [Key]
        public Guid Id { get; } = Guid.NewGuid();
    }
}
