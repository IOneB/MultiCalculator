using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestServer.Models.Tokens;

namespace RestServerTests.Tokens
{
    [TestClass]
    public class TokenSequenceValidatorTests
    {
        [TestMethod]
        public void ValidateCorrectSequenceTest()
        {
            //Arrange
            var sequenceValidator = new TokenSequenceValidator();
            var tupleSequence = ("Open" + nameof(BracketToken), nameof(NumericToken));
            //Act
            var correctSequence = sequenceValidator.Validate(tupleSequence);
            //Assert
            Assert.IsTrue(correctSequence);
        }
        [TestMethod]
        public void ValidateIncorrectSequenceTest()
        {
            //Arrange
            var sequenceValidator = new TokenSequenceValidator();
            var tupleSequence = (nameof(OperatorToken), "Close" + nameof(BracketToken));
            //Act
            var correctSequence = sequenceValidator.Validate(tupleSequence);
            //Assert
            Assert.IsFalse(correctSequence);
        }
        [TestMethod]
        public void ValidateCorrectValueTest()
        {
            //Arrange
            var sequenceValidator = new TokenSequenceValidator();
            var tupleSequence = ("AbraCadabra", nameof(NumericToken));
            //Act
            var correctSequence = sequenceValidator.Validate(tupleSequence);
            //Assert
            Assert.IsFalse(correctSequence);
        }
    }
}
