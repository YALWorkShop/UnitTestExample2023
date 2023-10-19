using ExpectedObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace UnitTestExample2023;

public class Tests
{
    // Add nuget Package FluentAssertions , NSubstitute , ExpectedObject
    private readonly DateTime _dateTimeNow = new DateTime(2023, 12, 12);
    private Example3 _example3;
    private IMemberDao _memberDao;
    private IUtil _util;
    [Test]
    public async Task SetMember_insert_member_exception_occur()
    {
        var inputMember = GenerateMember("IT012", "EMIAL@email.com", "0978123456", new DateTime());
        var expectedMember = GenerateMember("IT012", "EMIAL@email.com", "0978123456", new DateTime());
        var exception = new Exception("Test Error");

        GivenQueryMember(inputMember.Id, null);
        GivenInsertMemberThrow(expectedMember, exception);

        await SetMemberShouldBe(inputMember, $"Error 58825252 : {exception.Message}");
        SetExceptionLogShouldReceived(exception, 1);
    }

    [Test]
    public async Task SetMember_insert_member_success()
    {
        var inputMember = GenerateMember("IT012", "EMIAL@email.com", "0978123456", new DateTime());
        var expectedMember = GenerateMember("IT012", "EMIAL@email.com", "0978123456", _dateTimeNow);
        var exception = new Exception("Test Error");

        GivenDateTimeNow(_dateTimeNow);
        GivenQueryMember(inputMember.Id, null);

        await SetMemberShouldBe(inputMember, "Success");
        SetExceptionLogShouldReceived(exception, 0);
        await InsertMemberShouldReceived(expectedMember, 1);
        await UpdateMemberShouldReceived(expectedMember, 0);
    }

    [Test]
    public async Task SetMember_memberId_is_null_exception_occur()
    {
        var inputMember = GenerateMember(null, "EMIAL@email.com", "0978123456", new DateTime());

        await SetMemberShouldThrow(inputMember, "MemberId is null");
    }

    [Test]
    public async Task SetMember_update_member_success()
    {
        var inputMember = GenerateMember("IT012", "EMIAL@email.com", "0978123456", new DateTime());
        var expectedMember = GenerateMember("IT012", "EMIAL@email.com", "0978123456", _dateTimeNow);
        var resultMember = GenerateMember("IT012", "123", "789", new DateTime(2023, 1, 1));
        var exception = new Exception("Test Error");

        GivenDateTimeNow(_dateTimeNow);
        GivenQueryMember(inputMember.Id, resultMember);

        await SetMemberShouldBe(inputMember, "Success");
        SetExceptionLogShouldReceived(exception, 0);
        await InsertMemberShouldReceived(expectedMember, 0);
        await UpdateMemberShouldReceived(expectedMember, 1);
    }

    [SetUp]
    public void SetUp()
    {
        _memberDao = Substitute.For<IMemberDao>();
        _util = Substitute.For<IUtil>();
        _example3 = new Example3(_memberDao, _util);
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

    private void GivenDateTimeNow(DateTime dateTimeNow)
    {
        _util.GetNow().Returns(dateTimeNow);
    }

    private void GivenInsertMemberThrow(Member expectedMember, Exception exception)
    {
        _memberDao.InsertMember(Arg.Is<Member>(p => expectedMember.ToExpectedObject().Matches(p))).Throws(exception);
    }

    private void GivenQueryMember(string memberId, Member? resultMember)
    {
        _memberDao.QueryMember(Arg.Is(memberId)).Returns(Task.FromResult(resultMember));
    }

    private async Task InsertMemberShouldReceived(Member expectedMember, int times)
    {
        await _memberDao.Received(times).InsertMember(Arg.Is<Member>(p => expectedMember.ToExpectedObject().Matches(p)));
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

    private async Task SetMemberShouldThrow(Member inputMember, string exceptionMessage)
    {
        var actual = async () => { await _example3.SetMember(inputMember); };
        await actual.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);
    }


    private async Task UpdateMemberShouldReceived(Member expectedMember, int times)
    {
        await _memberDao.Received(times).UpdateMember(Arg.Is<Member>(p => expectedMember.ToExpectedObject().Matches(p)));
    }
}