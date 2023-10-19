using NSubstitute;
using System;
using ExpectedObjects;
using NSubstitute.ExceptionExtensions;

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

        var inputMember = GenerateMember("IT012", "EMIAL@email.com", "0978123456", new DateTime());
        var expectedMember = GenerateMember("IT012", "EMIAL@email.com", "0978123456", new DateTime());
        var exception = new Exception("Test Error");

        memberDao.QueryMember(Arg.Is(inputMember.Id)).Returns(Task.FromResult<Member?>(null));
        memberDao.InsertMember(Arg.Is<Member>(p => expectedMember.ToExpectedObject().Matches(p))).Throws(exception);

        var actual = await example3.SetMember(inputMember);
        Assert.AreEqual($"Error 58825252 : {exception.Message}", actual);

        util.Received(1).SetExceptionLog(Arg.Is(exception.Message), Arg.Is(nameof(Example3.SetMember)));
    }

    private static Member GenerateMember(string id, string email, string phone, DateTime updateTime)
    {
        var inputMember = new Member
        {
            Id = id,
            Email = email,
            Phone = phone,
            UpdateTime = updateTime
        };
        return inputMember;
    }
}