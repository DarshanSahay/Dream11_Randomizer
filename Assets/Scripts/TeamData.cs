using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamData : MonoBehaviour
{
    public string teamNumber;
    public string teamOneInitials;
    public string teamTwoInitials;
    public int teamOnePlayers;
    public int teamTwoPlayers;

    public Button teamBtn;
    public Text teamNumberText;
    public Text teamDetailsText;

    public void SetTeamData(string teamNumber, string teamOneName, string teamOneCount, string teamTwoName, string teamTwoCount)
    {
        this.teamNumber = teamNumber;
        teamNumberText.text = teamNumber;
        teamDetailsText.text = teamOneName + " " + teamOneCount + " : " + teamTwoCount + " " + teamTwoName;
    }
}
