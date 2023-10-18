using NSubstitute;
using System;

namespace UnitTestExample2023;

public class Tests
{
    // Add nuget Package FluentAssertions , NSubstitute , ExpectedObject
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task SetMember_insert_member_exception_occur()
    {
        IUtil util = Substitute.For<IUtil>();
        IMemberDao memberDao = Substitute.For<IMemberDao>();
        var example3 = new Example3(memberDao, util);

        Member inputMember = new Member();
        var exception = new Exception("Test Error");

        var actual = await example3.SetMember(inputMember);
        Assert.AreEqual($"Error 58825252 : {exception.Message}", actual);
    }
}