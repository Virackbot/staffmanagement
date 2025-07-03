using Xunit;
using FluentAssertions;

namespace StaffManagement.IntegrationTests;

public class SimpleIntegrationTest
{
    [Fact]
    public void Sample_Integration_Test_Should_Pass()
    {
        // Arrange
        var expected = "Integration Test";

        // Act
        var actual = "Integration Test";

        // Assert
        actual.Should().Be(expected);
    }
}
