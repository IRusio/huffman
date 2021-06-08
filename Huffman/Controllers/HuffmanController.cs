using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Huffman.Services;
using Microsoft.AspNetCore.Mvc;

namespace Huffman.Controllers
{
    [Route("Huffman")]
    public class HuffmanController : ControllerBase
    {
        private readonly HuffmanTree tree;
        public HuffmanController()
        {
            tree = new HuffmanTree();
        }

        [HttpPost("jsonTree")]
        public IActionResult DeserializeJsonTree(string content)
        {
            this.tree.Build(content);
            var result = tree.ReturnJsonTree();
            var entropy = tree.CalculateEntropy();

            Dictionary<char, string> resultString = new();
            foreach (char element in content.Distinct())
            {
                resultString.Add(element, tree.Encode(element.ToString()));
            }

            var avg = tree.CalculateAvgWorldLength(resultString);
            
            var resultObject = new {tree = result, entropy = entropy, avg = avg};
            return Ok(resultObject);
        }
    }
}