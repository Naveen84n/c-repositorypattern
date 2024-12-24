using Lkq.Core.RulesRepo.Common;
using Lkq.Models.RulesRepo.CompNine;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Lkq.Core.RulesRepo.Tests
{
    public class StructureHelperTests
    {
       
        [Fact]
        public void ExtractStructure_ValidType_ReturnsStructure()
        {
            //Act
            var response = StructureHelper.ExtractStructure(typeof(CompNine));

            //Assert
            Assert.Equal(1, response.Count);
            Assert.Equal("CompNineData", response[0].Name);

            response = response[0].Children;
            Assert.Equal(5, response.Count);
            Assert.Equal("BuildDate", response[0].Name);
            Assert.Equal(0, response[0].Children.Count);
            Assert.Equal("Description", response[1].Name);
            Assert.Equal(0, response[1].Children.Count);
            Assert.Equal("Found", response[2].Name);
            Assert.Equal(0, response[2].Children.Count);
            Assert.Equal("ModelYear", response[3].Name);
            Assert.Equal(0, response[3].Children.Count);
            Assert.Equal("OptionList", response[4].Name);

            response = response[4].Children;
            Assert.Equal(2, response.Count);
            Assert.Equal("Code", response[0].Name);
            Assert.Equal("Value", response[1].Name);
        }

        [Fact]
        public void ExtractStructure_StringType_ReturnsZeroStructure()
        {
            //Act
            var response = StructureHelper.ExtractStructure(typeof(string));

            //Assert
            Assert.Equal(0, response.Count);
        }

        [Fact]
        public void ExtractStructure_OtherDataTypes_ReturnsStructure()
        {
            //Act
            var response = StructureHelper.ExtractStructure(typeof(TestStructureHelperClass));

            //Assert
            Assert.Equal(5, response.Count);
            Assert.Equal("Color", response[0].Name);
            Assert.Equal(0, response[0].Children.Count);
            Assert.Equal("ComplexArray", response[1].Name);
            Assert.Equal(2, response[1].Children.Count);
            Assert.Equal("compNineOptions", response[2].Name);
            Assert.Equal(2, response[2].Children.Count);
            Assert.Equal("CompNineOptionsprop", response[3].Name);
            Assert.Equal(2, response[3].Children.Count);
            Assert.Equal("IntArray", response[4].Name);
            Assert.Equal(0, response[4].Children.Count);
        }

    }

    public class TestStructureHelperClass
    {
        public int[] IntArray = new int[10];
        public CompNineOption[] ComplexArray = new CompNineOption[2];
        public ConsoleColor Color;
        public IEnumerable<CompNineOption> compNineOptions;
        public CompNineOption[] CompNineOptionsprop { get; set; }
    }

}
