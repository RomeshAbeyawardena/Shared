using Xunit;
using Shared.Library.Extensions;
using System;

namespace Shared.UnitTests
{
    public class EnumerableExtensionTests
    {
        
        [Theory]
        [InlineData("Rose", new [] { "Daisy", "Tulip", "Lily", "Gemini", "Orchid" }, new [] { "Rose" } )]
        [InlineData("Daisy", new [] { "Tulip", "Lily", "Gemini", "Orchid" }, new [] { "Daisy", "Rose" } )]
        [InlineData("Tulip", new [] { "Lily", "Gemini", "Orchid" }, new [] { "Daisy", "Rose", "Tulip" } )]
        [InlineData("Lily", new [] { "Gemini", "Orchid" }, new [] { "Daisy", "Rose", "Tulip", "Lily" } )]
        [InlineData("Gemini", new [] { "Orchid" }, new [] { "Daisy", "Rose", "Tulip", "Lily", "Gemini" } )]
        [InlineData("Orchid", new string[0], new [] { "Daisy", "Rose", "Tulip", "Lily", "Gemini", "Orchid" } )]
        public void SkipFrom_returns_array_with_all_items_after_specified_valid_item_when_specfied_value_exists(string item, string[] expectedItems, string[] unexpectedItems)
        {
            if(expectedItems == null)
                throw new ArgumentNullException(nameof(expectedItems));

            if(unexpectedItems == null)
                throw new ArgumentNullException(nameof(unexpectedItems));

            var flowerArray = new [] { "Rose", "Daisy", "Tulip", "Lily", "Gemini", "Orchid" };

            var newArray = flowerArray.SkipFrom(item);

            if(expectedItems.Length == 0)
                Assert.Empty(newArray);
            else
                Assert.NotEmpty(newArray);

            foreach(var expected in expectedItems)
                Assert.Contains(expected, newArray);

            foreach(var expected in unexpectedItems)
                Assert.DoesNotContain(expected, newArray);

        }

        [Theory]
        [InlineData("Bob")]
        public void SkipFrom_returns_original_values_when_specified_item_does_not_exist(string item)
        {
            var flowerArray = new [] { "Rose", "Daisy", "Tulip", "Lily", "Gemini", "Orchid" };
            var newArray = flowerArray.SkipFrom(item);
            Assert.Equal(flowerArray, newArray);
        }

        [Theory]
        [InlineData("Rose", 0)]
        [InlineData("Daisy", 1)]
        [InlineData("Tulip", 2)]
        [InlineData("Lily", 3)]
        [InlineData("Gemini", 4)]
        [InlineData("Orchid", 5)]
        [InlineData("Pertunia", -1)]
        public void GetIndex(string value, int expectedIndex)
        {
            var flowerArray = new [] { "Rose", "Daisy", "Tulip", "Lily", "Gemini", "Orchid" };
            var actualIndex = flowerArray.GetIndex(a => a == value);
            Assert.Equal(expectedIndex, actualIndex);
        }

        public class MyTestCase
        {

        }
    }
}
