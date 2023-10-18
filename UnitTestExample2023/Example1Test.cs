namespace UnitTestExample2023;

public class Example1Test
{
    private readonly Example1 _example1 = new Example1();

    [Test]
    public void AdditionTest_1_add_2_then_3()
    {
        // new requirement : a + b => a * b
        AdditionShouldBe(1, 2, 2);
    }


    [Test]
    public void AdditionTest_3_add_2_then_5()
    {
        AdditionShouldBe(3, 2, 6);
    }


    private void AdditionShouldBe(int a, int b, int expected)
    {
        var actual = _example1.Addition(a, b);
        Assert.AreEqual(expected, actual);
    }

}