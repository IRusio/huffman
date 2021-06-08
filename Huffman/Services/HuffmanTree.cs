using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Huffman.Services
{
    public class HuffmanTree
    {
        private List<Node> nodes = new List<Node>();
        public Node Root { get; set; }
        public Dictionary<char, int> Frequencies = new Dictionary<char, int>();

        public void Build(string source)
        {
            foreach (var element in source)
            {
                if (!Frequencies.ContainsKey(element))
                    Frequencies.Add(element, 0);
                Frequencies[element]++;
            }

            foreach (var symbol in Frequencies)
            {
                nodes.Add(new Node() {Symbol = symbol.Key, Frequency = symbol.Value});
            }

            while (nodes.Count > 1)
            {
                var orderedNodes = nodes.OrderBy(node => node.Frequency).ToList();

                if (orderedNodes.Count >= 2)
                {
                    var twoFirstElementOfNode = orderedNodes.Take(2).ToList();

                    var parent = new Node()
                    {
                        Frequency = twoFirstElementOfNode.First().Frequency + twoFirstElementOfNode.Last().Frequency,
                        Left = twoFirstElementOfNode.First(),
                        Right = twoFirstElementOfNode.Last()
                    };

                    nodes.Remove(twoFirstElementOfNode.First());
                    nodes.Remove(twoFirstElementOfNode.Last());
                    nodes.Add(parent);
                }
            }
            this.Root = nodes.FirstOrDefault();
        }

        public string Encode(string source)
        {
            var encodedSource = new List<bool>();

            foreach (var element in source)
            {
                var encodedSymbol = this.Root.Traverse(element, new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            return encodedSource.Select(x=>x==true?"1":"0").Aggregate((i, j) => i+j);
        }

        public string ReturnJsonTree()
        {
            return JsonSerializer.Serialize(this.Root);
        }
        
        public string Decode(BitArray bits)
        {
            var currentNode = this.Root;
            var decoded = string.Empty;

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (currentNode.Right != null)
                        currentNode = currentNode.Right;
                    else if (currentNode.Left != null)
                        currentNode = currentNode.Left;

                    if (IsLeaf(currentNode))
                    {
                        decoded += currentNode.Symbol;
                        currentNode = this.Root;
                    }
                }
            }
            return decoded;
        }

        public static bool IsLeaf(Node node)
        {
            return node.Left == null && node.Right == null;
        }

        public double CalculateEntropy()
        {
            List<double> probability = new ();
            var countOfFrequency = 0;
            var result = 0.0d;
            foreach (var node in Frequencies)
            {
                countOfFrequency += node.Value;
            }
            
            foreach (var node in Frequencies)
            {
                probability.Add(node.Value/(double)countOfFrequency);
            }

            foreach (var element in probability)
            {
                result += element * Math.Log2(1 / element);
            }

            return result;
        }

        public double CalculateAvgWorldLength(Dictionary<char, string> resultString)
        {
            Dictionary<char, double> probability = new ();
            var countOfFrequency = 0;
            var result = 0.0d;
            foreach (var node in Frequencies)
            {
                countOfFrequency += node.Value;
            }
            
            foreach (var node in Frequencies)
            {
                probability.Add(node.Key, node.Value/(double)countOfFrequency);
            }
            
            foreach (var pair in resultString)
            {
                result += probability[pair.Key] * pair.Value.Length;
            }
            return result;
        }
        
    }
}