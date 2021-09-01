using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Graphs.Documents.Tests
{
    public class CacheTests
    {
        [Fact]
        public void Cache_EventArgs()
        {
            var document = (Document<Member>)new Member();

            var documentEventArgs = new CacheItemEvictedEventArgs<Member>(document, EvictionReason.Removed);
            Assert.Equal(document, documentEventArgs.Document);
            Assert.Equal(EvictionReason.Removed, documentEventArgs.Reason);

            var readArgs = new CacheReadEventArgs(document.Key, CacheReadType.Hit);
            Assert.Equal(document.Key, readArgs.Key);
            Assert.Equal(CacheReadType.Hit, readArgs.ReadType);

        }
    }
}
