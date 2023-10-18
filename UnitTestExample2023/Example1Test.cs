namespace UnitTestExample2023;

public class Example1Test
{
    [Test]
    public void AdditionTest_1_add_2_then_3()
    {
        var example1 = new Example1();
        var actual = example1.Addition(1, 2);
        Assert.AreEqual(3,actual);
    }
}