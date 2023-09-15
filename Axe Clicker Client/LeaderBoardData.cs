using System.Collections.Generic;

public class LeaderBoardData
{
    public int playerScore { get; set; }
    public int playerRank { get; set; }

    public List<ScoreDTO> scoreDTOs { get; set; }
}