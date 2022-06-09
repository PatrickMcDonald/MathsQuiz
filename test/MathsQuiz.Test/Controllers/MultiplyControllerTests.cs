using MathsQuiz.Controllers;

namespace MathsQuiz.Test.Controllers;

public class MultiplyControllerTests
{
    [Theory]
    [InlineData(3, 2, 6)]
    public void Get(int x, int y, int expected)
    {
        var controller = new MultiplyController();
        var actual = controller.Get(x, y);
        Assert.Equal(expected, actual.Value);
    }
}
