using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Huffman.Services
{
    public class Node
    {
        public char Symbol { get; set; }
        public int Frequency { get; set; }
        public Node Right { get; set; }
        public Node Left { get; set; }

        public IEnumerable<bool> Traverse(char symbol, IEnumerable<bool> data)
        {
            if (Right == null && Left == null)
                return symbol.Equals(this.Symbol)? data : null;
            else
            {
                IEnumerable<bool> left = null;
                IEnumerable<bool> right = null;

                if (Left != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = Left.Traverse(symbol, leftPath);
                }

                if (Right != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = Right.Traverse(symbol, rightPath);
                }

                return left != null? left : right;
            }
        }
    }
}