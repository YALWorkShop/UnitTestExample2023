﻿namespace UnitTestExample2023;

public class Example1Test
{
    private readonly Example1 _example1 = new Example1();

    [Test]
    public void AdditionTest_1_add_2_then_3()
    {
        AdditionShouldBe(1, 2, 3);
    }

    private void AdditionShouldBe(int a, int b, int expected)
    {
        var actual = _example1.Addition(a, b);
        Assert.AreEqual(expected, actual);
    }

}