using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamGenerator : MonoBehaviour
{
    [SerializeField] private LoadTeamsData loadTeams;
    [SerializeField] private List<string> playerANames;

    [SerializeField] private GameObject teamDataPanel;
    [SerializeField] private TeamData teamDataPrefab;
    [SerializeField] private GameObject teamDataHolder;

    [SerializeField] private GameObject teamDetailsPanel;
    [SerializeField] private Transform playerListHolder;
    [SerializeField] private PlayerInfo playerDataPrefab;

    public void GetTeamsData(int totalTeams, List<Button> teamA, List<Button> teamB, string teamAName, string teamBName)
    {
        string[] choosenteamA = new string[teamA.Count];
        string[] choosenteamB = new string[teamB.Count];

        double[] teamACredits = new double[teamA.Count];
        double[] teamBCredits = new double[teamB.Count];

        int[] specialA = new int[teamA.Count];
        int[] specialB = new int[teamB.Count];

        for (int i = 0; i < teamA.Count; i++)
        {
            choosenteamA[i] = teamA[i].GetComponentInParent<PlayerInfo>().playerName;
            teamACredits[i] = teamA[i].GetComponentInParent<PlayerInfo>().playerCredit;
            specialA[i] = teamA[i].GetComponentInParent<PlayerInfo>().playerRoleCredit;
        }
        for (int i = 0; i < teamB.Count; i++)
        {
            choosenteamB[i] = teamB[i].GetComponentInParent<PlayerInfo>().playerName;
            teamBCredits[i] = teamB[i].GetComponentInParent<PlayerInfo>().playerCredit;
            specialB[i] = teamB[i].GetComponentInParent<PlayerInfo>().playerRoleCredit;
        }

        GenerateTeams(totalTeams, teamA: choosenteamA, specialA: specialA, creditsA: teamACredits, choosenteamB, specialB, teamBCredits, teamAName, teamBName);
    }

    public void GenerateTeams(int totalTeams, string[] teamA, int[] specialA, double[] creditsA, string[] teamB, int[] specialB, double[] creditsB, string teamAName, string teamBName)
    {
        int totalNoOfPlayers = teamA.Length + teamB.Length;
        int totalNoOfPlayersTeamA = teamA.Length;
        int totalNoOfPlayersTeamB = teamB.Length;

        int maximumTeamAPlayers = 6;
        int maximumTeamBPlayers = 6;
        int minimumTeamAPlayers = 5;
        int minimumTeamBPlayers = 5;

        int totalDreamPlayers = 11;

        int minimumWk = 1, minimumBat = 1, minimumAll = 1, minimumBowl = 1;
        int maximumWk = 8, maximumBat = 8, maximumAll = 8, maximumBowl = 8;
        int atleastWk = 1, atleastBat = 1, atleastAll = 1, atleastBowl = 1;

        string[] allPlayers = new string[totalNoOfPlayers];
        int[] allSpecials = new int[totalNoOfPlayers];
        double[] allCredits = new double[totalNoOfPlayers];

        for (int i = 0; i < totalNoOfPlayersTeamA; i++)
        {
            allPlayers[i] = teamA[i];
            allSpecials[i] = specialA[i];
            allCredits[i] = creditsA[i];
        }
        for (int i = totalNoOfPlayersTeamA; i < totalNoOfPlayers; i++)
        {
            allPlayers[i] = teamB[i - totalNoOfPlayersTeamA];
            allSpecials[i] = specialB[i - totalNoOfPlayersTeamA];
            allCredits[i] = creditsB[i - totalNoOfPlayersTeamA];
        }

        int totalNoOfTeams = totalTeams;
        for (int i = 1; i <= totalNoOfTeams; i++)
        {
            int wk1 = 0, bat1 = 0, all1 = 0, bowl1 = 0;
            List<string> list = new List<string>();

            int randomA = 6;
            int randomB = totalDreamPlayers - randomA;

            int x = atleastWk + atleastBat + atleastAll + atleastBowl;

            while (x != 0)
            {
                int r = UnityEngine.Random.Range(0, totalNoOfPlayers);

                if (allSpecials[r] == 1 && wk1 == 0 && atleastWk == 1)
                {
                    wk1++; x--;
                }
                else if (allSpecials[r] == 2 && bat1 == 0 && atleastBat == 1)
                {
                    bat1++; x--;
                }
                else if (allSpecials[r] == 3 && all1 == 0 && atleastAll == 1)
                {
                    all1++; x--;
                }
                else if (allSpecials[r] == 4 && bowl1 == 0 && atleastBowl == 1)
                {
                    bowl1++; x--;
                }
                else continue;

                list.Add(allPlayers[r]);
                if (r < totalNoOfPlayersTeamA) randomA--;
                else randomB--;
            }

            int wk = wk1, bat = bat1, all = all1, bowl = bowl1;

            while (randomA > 0)
            {
                int r = UnityEngine.Random.Range(0, totalNoOfPlayersTeamA);
                if (list.Contains(teamA[r])) continue;

                bool added = false;
                if (specialA[r] == 1 && wk < maximumWk) { wk++; added = true; }
                else if (specialA[r] == 2 && bat < maximumBat) { bat++; added = true; }
                else if (specialA[r] == 3 && all < maximumAll) { all++; added = true; }
                else if (specialA[r] == 4 && bowl < maximumBowl) { bowl++; added = true; }

                if (!added) continue;

                list.Add(teamA[r]);
                randomA--;
            }

            while (randomB > 0)
            {
                int r = UnityEngine.Random.Range(0, totalNoOfPlayersTeamB);
                if (list.Contains(teamB[r])) continue;

                bool added = false;
                if (specialB[r] == 1 && wk < maximumWk) { wk++; added = true; }
                else if (specialB[r] == 2 && bat < maximumBat) { bat++; added = true; }
                else if (specialB[r] == 3 && all < maximumAll) { all++; added = true; }
                else if (specialB[r] == 4 && bowl < maximumBowl) { bowl++; added = true; }

                if (!added) continue;

                list.Add(teamB[r]);
                randomB--;
            }

            if (wk < minimumWk || bat < minimumBat || all < minimumAll || bowl < minimumBowl)
            {
                i--; continue;
            }

            //Debug.Log($"Team : {i}");
            //for (int j = 0; j < list.Count; j++)
            //{
            //    Debug.Log($"{j + 1}. {list[j]}");
            //}

            int captain = 0, viceCaptain = 0;
            while (captain == viceCaptain)
            {
                captain = UnityEngine.Random.Range(1, 12);
                viceCaptain = UnityEngine.Random.Range(1, 12);
            }

            //Debug.Log($"Captain : {captain}");
            //Debug.Log($"Vice Captain : {viceCaptain}");
            //Debug.Log("\n");

            int countTeamA = 0;
            int countTeamB = 0;

            foreach (var name in list)
            {
                if (Array.IndexOf(teamA, name) >= 0) countTeamA++;
                else if (Array.IndexOf(teamB, name) >= 0) countTeamB++;
            }

            teamDataPanel.SetActive(true);

            TeamData teamUI = Instantiate(teamDataPrefab, teamDataHolder.transform);
            teamUI.SetTeamData($"Team {i}", teamAName, countTeamA.ToString(), teamBName, countTeamB.ToString());

            List<PlayerData> playerInfos = new List<PlayerData>();
            for (int j = 0; j < list.Count; j++)
            {
                string playerName = list[j];
                int index = Array.IndexOf(allPlayers, playerName);
                PlayerRole role = GetRoleName(allSpecials[index]);
                double credit = allCredits[index];
                bool isCaptain = (j == captain - 1);
                bool isVCaptain = (j == viceCaptain - 1);
                playerInfos.Add(new PlayerData(playerName, role, credit, isCaptain, isVCaptain));
            }

            teamUI.teamBtn.onClick.AddListener(() =>
            {
                ShowTeamDetails(playerInfos);
            });
        }
    }

    private PlayerRole GetRoleName(int roleId)
    {
        return roleId switch
        {
            1 => PlayerRole.Wicketkeeper_Batter,
            2 => PlayerRole.Batter,
            3 => PlayerRole.All_Rounder,
            4 => PlayerRole.Bowler,
            _ => PlayerRole.Default
        };
    }

    private void ShowTeamDetails(List<PlayerData> playerInfos)
    {
        teamDetailsPanel.SetActive(true);

        // Clear previous players
        foreach (Transform child in playerListHolder)
        {
            Destroy(child.gameObject);
        }

        // Populate new players
        foreach (var info in playerInfos)
        {
            PlayerInfo playerUI = Instantiate(playerDataPrefab, playerListHolder).GetComponent<PlayerInfo>();
            playerUI.SetUpData(info.role, info.name, info.credit.ToString());
            if (info.isCaptain)
            {
                playerUI.SetCaptain();
            }
            if (info.isVCaptain)
            {
                playerUI.SetVCaptain();
            }
        }
    }
}
