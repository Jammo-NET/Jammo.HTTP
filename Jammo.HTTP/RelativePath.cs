using System.Collections;
using System.Collections.Generic;

namespace Jammo.HTTP
{
    public readonly struct RelativePath : IEnumerable<RelativePart>
    {
        private readonly string path;
        private readonly string backwardInstruction;
        private readonly string delimiter;

        public RelativePath(string path)
        {
            this.path = path;
            backwardInstruction = "..";
            delimiter = "/";
        } // Follows the URL/URI pattern
        
        public RelativePath(string path, string delimiter, string backwardInstruction)
        {
            this.path = path;
            this.delimiter = delimiter;
            this.backwardInstruction = backwardInstruction;
        }

        public override string ToString()
        {
            return path;
        }

        public string[] ToStringArray()
        {
            return path.Split(delimiter);
        }

        public bool Equals(RelativePath other)
        {
            return path == other.path;
        }

        public override bool Equals(object obj)
        {
            return obj is RelativePath other && Equals(other);
        }

        public override int GetHashCode()
        {
            return path != null ? path.GetHashCode() : 0;
        }

        public IEnumerator<RelativePart> GetEnumerator()
        {
            foreach (var part in ToStringArray())
            {
                if (part == backwardInstruction)
                    yield return new RelativePart(TraversalDirection.Backward, part);
                else
                    yield return new RelativePart(TraversalDirection.Forward, part);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    public readonly struct RelativePart
    {
        public readonly TraversalDirection Direction;
        public readonly string Value;

        public RelativePart(TraversalDirection direction, string value)
        {
            Direction = direction;
            Value = value;
        }
    }

    public enum TraversalDirection
    {
        Forward,
        Backward
    }
}