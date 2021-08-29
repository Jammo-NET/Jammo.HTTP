using System;
using System.Collections.Generic;
using System.Linq;

namespace Jammo.HTTP
{
    public class JHttpClient
    {
        private Url url;
        public Url Url => url;
        
        public JHttpClient(Url url)
        {
            if (!url.IsValid)
                throw new ArgumentException("Invalid url");

            this.url = url;
        }

        public JHttpClient NavigateRelative(RelativePath path)
        {
            var directive = new RelativePath(url.Directive).ToStringArray().ToList();
            
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
            
            directive.Insert(0, url.Protocol + url.Site);
            
            return new JHttpClient(Url.Join(directive.ToArray()));
        }
    }
}