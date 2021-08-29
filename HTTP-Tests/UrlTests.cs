using System;
using System.Net;
using System.Net.Http;
using Jammo.HTTP;
using NUnit.Framework;

namespace HTTP_Tests
{
    public class UrlTests
    {
        private readonly Url testUrl = new Url("https://www.github.com/github");
        
        [Test]
        public void TestValidUrl()
        {
            Assert.True(testUrl.IsValid);
        }

        [Test]
        public void TestUrlSecure()
        {
            Assert.True(testUrl.Secure);
        }

        [Test]
        public void TestUrlSite()
        {
            Assert.True(testUrl.Site == "www.github.com");
        }
        
        [Test]
        public void TestUrlDirective()
        {
            Assert.True(testUrl.Directive == "/github");
        }
    }
}