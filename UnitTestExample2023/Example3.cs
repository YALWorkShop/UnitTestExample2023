using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic.CompilerServices;

namespace UnitTestExample2023;

public class Example3
{
    public async Task<string> SetMember(Member inputMember)
    {
        try
        {
            var memberDao = new MemberDao();

            var member =await memberDao.QueryMember(inputMember.Id);

            if (member == null)
            {
                inputMember.UpdateTime = DateTime.Now;
                await memberDao.InsertMember(inputMember);
            }
            else
            {
                member.Email = inputMember.Email;
                member.Phone = inputMember.Phone;
                member.UpdateTime = DateTime.Now;
                await memberDao.UpdateMember(member);
            }

            return "Success";
        }
        catch (Exception exception)
        {
            new Util().SetExceptionLog(exception.Message,nameof(SetMember));
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

public class MemberDao
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

public class Util
{
    public void SetExceptionLog(string exception, string functionName)
    {
        throw new NotImplementedException();
    }
}
