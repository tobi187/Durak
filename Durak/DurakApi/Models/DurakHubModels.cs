using DurakApi.Models.Game;

namespace DurakApi.Models;

public class HubBaseModel
{
    public string roomId { get; set; }
}

public class HubUserModel : HubBaseModel
{
    public string? userName { get; set; }
}

public class HubCardModel : HubBaseModel
{
    public Card card { get; set; }
}

public class HubCardToBeatModel : HubCardModel
{
    public Card cardToBeat { get; set; }
}
