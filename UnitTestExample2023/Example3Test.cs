using ExpectedObjects;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace UnitTestExample2023;

public class Tests
{
    private Example3 _example3;
    private IMemberDao _memberDao;
    private IUtil _util;

    [SetUp]
    public void SetUp()
    {
        _memberDao = Substitute.For<IMemberDao>();
        _util = Substitute.For<IUtil>();
        _example3 = new Example3(_memberDao, _util);
    }

    // Add nuget Package FluentAssertions , NSubstitute , ExpectedObject

    [Test]
    public async Task SetMember_insert_member_exception_occur()
    {

        var inputMember = GenerateMember("IT012", "EMIAL@email.com", "0978123456", new DateTime());
        var expectedMember = GenerateMember("IT012", "EMIAL@email.com", "0978123456", new DateTime());
        var exception = new Exception("Test Error");

        _memberDao.QueryMember(Arg.Is(inputMember.Id)).Returns(Task.FromResult<Member?>(null));
        _memberDao.InsertMember(Arg.Is<Member>(p => expectedMember.ToExpectedObject().Matches(p))).Throws(exception);

        await SetMemberShouldBe(inputMember, $"Error 58825252 : {exception.Message}");
        SetExceptionLogShouldReceived(exception, 1);
    }

    private void SetExceptionLogShouldReceived(Exception exception, int times)
    {
        _util.Received(times).SetExceptionLog(Arg.Is(exception.Message), Arg.Is(nameof(Example3.SetMember)));
    }

    private async Task SetMemberShouldBe(Member inputMember, object? expectedResult)
    {
        var actual = await _example3.SetMember(inputMember);
        Assert.AreEqual(expectedResult, actual);
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