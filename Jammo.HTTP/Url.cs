using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Jammo.HTTP
{
    public readonly struct Url
    {
        private readonly string value;
        
        public readonly bool Secure;
        public readonly bool IsValid;

        public readonly string Protocol;
        public readonly string Site;
        public readonly string Directive;

        public Url(string value)
        {
            this.value = value;

            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(3),
                MaxResponseContentBufferSize = 1
            };

            try
            {
                IsValid =  client.GetAsync(this.value).Result.StatusCode == HttpStatusCode.OK;
            }
            catch (AggregateException)
            {
                IsValid = true;
            }
            catch (InvalidOperationException)
            {
                IsValid = false;
            }

            if (!IsValid)
            {
                Secure = false;
                Site = "";
                Directive = "";
            }
            
            const string urlDivider = "://";

            var protocol = this.value.UntilOrEmpty(":");
            
            var dividerLength = this.value[protocol.Length..].IndexOf(urlDivider, StringComparison.CurrentCulture);

            if (dividerLength == -1)
                dividerLength = 0;
            else
                dividerLength += urlDivider.Length;
            
            var link = this.value[(protocol.Length + dividerLength)..];
            var site = link.UntilOrEmpty("/");
            
            var args = link[site.Length..];
            
            Secure = protocol.EndsWith('s');

            Protocol = protocol + urlDivider;
            Site = site;
            Directive = args;
        }
        
        public static Url Join(params string[] paths)
        {
            var builder = new StringBuilder();
            var first = paths.FirstOrDefault();

            foreach (var path in paths)
            {
                if (string.IsNullOrWhiteSpace(path))
                    continue;

                if (path == first && path.StartsWith("/"))
                {
                    builder.Append(path.Skip(1));
                    continue;
                }
                
                if (path != first && !path.StartsWith("/"))
                    builder.Append("/");
                
                builder.Append(path);
                builder.Append("/");
            }
            
            return new Url(builder.ToString());
        }

        public Url Fix()
        {
            return new Url(value);
        }

        public override string ToString()
        {
            return value;
        }

        public bool Equals(Url other)
        {
            return value == other.value;
        }

        public override int GetHashCode()
        {
            return value != null ? value.GetHashCode() : 0;
        }
    }
}