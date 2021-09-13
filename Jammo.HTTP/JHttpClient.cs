using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Jammo.HTTP
{
    public class JHttpClient
    {
        internal static readonly HttpClient GlobalClient = new HttpClient();
        
        public Url Url { get; }
        
        public JHttpClient(Url url)
        {
            if (!url.IsValid)
                throw new ArgumentException("Invalid url");

            Url = url;
        }

        public HttpResponseMessage Get()
        {
            return GlobalClient.GetAsync(Url.ToString(), HttpCompletionOption.ResponseContentRead).Result;
        }
        
        public HttpResponseMessage Post(HttpContent content)
        {
            return GlobalClient.PostAsync(Url.ToString(), content).Result;
        }

        public JHttpClient NavigateRelative(RelativePath path)
        {
            var directive = new RelativePath(Url.Directive).ToStringArray().ToList();
            
            foreach (var instruction in path)
            {
                switch (instruction.Direction)
                {
                    case TraversalDirection.Forward:
                        directive.Add(instruction.Value);
                        break;
                    case TraversalDirection.Backward:
                        directive.Remove(directive.Last());
                        break;
                }
            }
            
            directive.Insert(0, Url.Protocol + Url.Site);
            
            return new JHttpClient(Url.Join(directive.ToArray()));
        }
    }
}