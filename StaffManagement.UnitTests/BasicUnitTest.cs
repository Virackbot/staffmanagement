using Xunit;
using FluentAssertions;

namespace StaffManagement.UnitTests;

public class BasicUnitTest
{
    [Fact]
    public void Sample_Unit_Test_Should_Pass()
    {
        // Arrange
        var expected = "Unit Test Working";

        // Act
        var actual = "Unit Test Working";

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(5, 5, 10)]
    [InlineData(-1, 1, 0)]
    public void Add_TwoNumbers_ShouldReturnSum(int a, int b, int expected)
    {
        // Act
        var result = a + b;

        // Assert
        result.Should().Be(expected);
    }
}
