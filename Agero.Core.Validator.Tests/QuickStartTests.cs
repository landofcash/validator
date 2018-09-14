﻿using Agero.Core.Validator.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agero.Core.Validator.Tests
{
    public class MyClass
    {
        [IntValidate("Id must be greater than 0.", MinValue = 1)]
        public int Id { get; set; }

        [StringValidate("Text must have lenghth between 2 and 100 characters.", MinLength = 2, MaxLength = 100)]
        public string Text { get; set; }

        [ComplexTypeValidate] // This attribute tells ValidationHelper to validate MyValueClass object
        [NotNullValidate("MyValue must be specified.")]
        public MyValueClass MyValue { get; set; }
    }

    public class MyValueClass
    {
        [NotNullValidate("Value must be specified.")]
        public object Value { get; set; }
    }

    [TestClass]
    public class QuickStartTests
    {
        [TestMethod]
        public void Test_Validate_Should_Return_Errors()
        {
            var myClass =
                new MyClass
                {
                    Id = -1, // Invalid
                    Text = "Some text", // Valid
                    MyValue = // Valid
                        new MyValueClass
                        {
                            Value = null // Invalid
                        }
                };

            var validator = new ValidationHelper();
            var validationErrors = validator.Validate(myClass);

            Assert.AreSame(myClass, validationErrors.Object);
            Assert.AreEqual(2, validationErrors.Errors.Length);

            Assert.AreEqual("Id", validationErrors.Errors[0].Key);
            Assert.AreEqual("Id must be greater than 0.", validationErrors.Errors[0].Message);

            Assert.AreEqual("Value", validationErrors.Errors[1].Key);
            Assert.AreEqual("Value must be specified.", validationErrors.Errors[1].Message);
        }
    }
}
