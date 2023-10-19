using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic.CompilerServices;

namespace UnitTestExample2023;

public class Example3
{
    private IMemberDao _memberDao;
    private IUtil _util;

    public Example3(IMemberDao memberDao, IUtil util)
    {
        _memberDao = memberDao;
        _util = util;
    }

    public async Task<string> SetMember(Member inputMember)
    {
        if (inputMember.Id == null)
        {
            throw new Exception("MemberId is null");
        }

        try
        {
            var member = await _memberDao.QueryMember(inputMember.Id);

            if (member == null)
            {
                inputMember.UpdateTime = _util.GetNow();
                await _memberDao.InsertMember(inputMember);
            }
            else
            {
                member.Email = inputMember.Email;
                member.Phone = inputMember.Phone;
                member.UpdateTime = _util.GetNow();
                await _memberDao.UpdateMember(member);
            }

            return "Success";
        }
        catch (Exception exception)
        {
            _util.SetExceptionLog(exception.Message, nameof(SetMember));
            return $"Error 58825252 : {exception.Message}";
        }
    }
}

public class Member
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime UpdateTime { get; set; }
}

public interface IMemberDao
{
    Task<Member?> QueryMember(string id);
    Task InsertMember(Member inputMember);
    Task UpdateMember(Member member);
}

public class MemberDao : IMemberDao
{
    public async Task<Member?> QueryMember(string id)
    {
        throw new NotImplementedException();
    }

    public async Task InsertMember(Member inputMember)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateMember(Member member)
    {
        throw new NotImplementedException();
    }
}

public interface IUtil
{
    void SetExceptionLog(string exception, string functionName);
    DateTime GetNow();
}

public class Util : IUtil
{
    public void SetExceptionLog(string exception, string functionName)
    {
        throw new NotImplementedException();
    }

    public DateTime GetNow()
    {
        return DateTime.Now;
    }
}
