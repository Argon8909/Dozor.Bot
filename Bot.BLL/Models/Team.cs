namespace Bot.BLL.Models;

public class Team
{
    private static int _lastId = 0;

    public Team(string name)
    {
        Name = name;
        Id = ++_lastId;
        Members = new List<Member>();
    }

    public int Id { get; }
    public string Name { get; }
    public List<Member> Members { get; private set; }

    public void AddMember(Member member)
    {
        var cap = Members.Any(x => x.Status == Status.Captain);
        if (cap)
        {
            member.Status = Status.Player;
        }
        else
        {
            member.Status = Status.Captain;
        }

        member.Team = Id;
        Members.Add(member);
    }

    public void DeleteMember(Member member)
    {
        var check = Members.Any(x =>
            x.Status == Status.Player && x.TgUsername == member.TgUsername && x.Name == member.Name && x.Team == Id);
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
    public string Name { get; }
    public string? TgUsername { get; }
    public Status Status { get; set; }
}

public enum Status
{
    Captain,
    Player,
}

/* пример использования
Team team = new Team();
Member member1 = new Member { Team = team, Name = "Player1", Status = Status.Active };
Member member2 = new Member { Team = team, Name = "Player2", Status = Status.Active };

team.Members.Add(member1);
team.Members.Add(member2);

*/