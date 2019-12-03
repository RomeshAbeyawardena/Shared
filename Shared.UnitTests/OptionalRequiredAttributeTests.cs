using Shared.Services.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Shared.UnitTests
{
    public class OptionalRequiredAttributeTests
    {
        [Theory]
        [InlineData(1, null)]
        [InlineData(0, "Cat")]
        public void Passes_validation_when_at_least_one_parameter_is_not_null(int petTypeId, string petType)
        {
            var myTestClass = new MyTestCase1 { PetType = petType, PetTypeId = petTypeId };
            
            Assert.True(TryValidateObject(myTestClass, out var context, out var results, true));
        }

        [Theory]
        [InlineData(0, null)]
        public void Fails_validation_when_all_parameters_are_null(int petTypeId, string petType)
        {
            var myTestClass = new MyTestCase1 { PetType = petType, PetTypeId = petTypeId };
            
            Assert.False(TryValidateObject(myTestClass, out var context, out var results, true));
        }

        [Fact]
        public void Fails_validation_when_parameter_does_not_exist()
        {
            var myTestClass = new MyTestCase2 { Samuel = "Thats me", Tom = "Not me" };
            
            Assert.False(TryValidateObject(myTestClass, out var context, out var results, true));
        }

        private bool TryValidateObject<T>(T model, out ValidationContext context, out ICollection<ValidationResult> results, bool validateAll)
        {
            context = new ValidationContext(model);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, validateAll);
        }
    }
    [OptionalRequired(1, nameof(PetType), nameof(PetTypeId))]
    internal class MyTestCase1
    {
        public int Id { get; set; }
        public int PetTypeId { get; set; }
        public string PetType { get; set; }
    }

    [OptionalRequired(1, nameof(Samuel), "Id")]
    internal class MyTestCase2
    {
        public string Samuel { get; set; }
        public string Tom { get; set; }
        public string Harry { get; set; }
    }

    [OptionalRequired(2, nameof(Samuel), nameof(Tom), nameof(Harry))]
    internal class MyTestCase3
    {
        public string Samuel { get; set; }
        public string Tom { get; set; }
        public string Harry { get; set; }
    }
}
