namespace Bot.BLL.Models;

public class Team
{
    
    private static int _lastId = 0;

    public Team(string name, TeamStatus teamStatus )
    {
        TeamStatus = teamStatus;
        Name = name;
        Id = ++_lastId;
        Members = new List<Member>();
    }

    public int Id { get; }
    public string Name { get; }
    public List<Member> Members { get; private set; }
    public TeamStatus TeamStatus { get; }

    public void AddMember(Member member)
    {
        var cap = Members.Any(x => x.MemberStatus == MemberStatus.Captain);
        if (cap)
        {
            member.MemberStatus = MemberStatus.Player;
        }
        else
        {
            member.MemberStatus = MemberStatus.Captain;
        }

        member.Team = Id;
        Members.Add(member);
    }

    public void DeleteMember(Member member)
    {
        var check = Members.Any(x =>
            x.MemberStatus == MemberStatus.Player && x.TgUsername == member.TgUsername && x.Name == member.Name &&
            x.Team == Id);
        if (check)
        {
            Members.Remove(member);
        }
    }
}

public class Member
{
    public Member(string name, string? tgUsername)
    {
        Name = name;
        TgUsername = tgUsername;
    }

    public int Team { get; set; }
    public string? Name { get; }
    public string? TgUsername { get; }
    public MemberStatus MemberStatus { get; set; }
}

public enum MemberStatus
{
    Captain,
    Player,
    Admin,
}

public enum TeamStatus
{
    Players,
    Admins,
}


/*
*/