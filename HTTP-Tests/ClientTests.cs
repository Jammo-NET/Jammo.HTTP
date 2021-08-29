using System;
using System.Net;
using Jammo.HTTP;
using NUnit.Framework;

namespace HTTP_Tests
{
    public class ClientTests
    {
        [Test]
        public void TestValidRelativePath()
        {
            var url = new Url("https://www.github.com/");
            var client = new JHttpClient(url).NavigateRelative(new RelativePath("github"));

            Assert.Pass();
        }

        [Test]
        public void TestBackwardRelativePath()
        {
            var url = new Url("https://www.github.com/github");
            var client = new JHttpClient(url).NavigateRelative(new RelativePath(".."));
            
            Assert.True(client.Url.ToString() == "https://www.github.com/");
        }
    }
}