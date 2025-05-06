using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadTeamsData : MonoBehaviour
{
    private Dictionary<string, List<PlayerData>> teamsData = new();
    [SerializeField] private GameObject playerHolderOne;
    [SerializeField] private GameObject playerHolderTwo;
    [SerializeField] private GameObject playerInfoPrefab;

    [SerializeField] private TeamIcons teamIconSO;

    [SerializeField] private List<Button> teamOnePlayersList;
    [SerializeField] private List<Button> teamTwoPlayersList;

    [SerializeField] private List<Button> selectedTeamOnePlayers;
    [SerializeField] private List<Button> selectedTeamTwoPlayers;

    [SerializeField] private Image teamOne;
    [SerializeField] private Image teamTwo;

    [SerializeField] private string teamOneName;
    [SerializeField] private string teamTwoName;

    [SerializeField] private Text teamOneCount;
    [SerializeField] private Text teamTwoCount;

    [SerializeField] private Color colorZero;
    [SerializeField] private Color colorMax;

    private Dictionary<PlayerRole, List<Button>> teamOneButtonsByRole = new();
    private Dictionary<PlayerRole, List<Button>> teamTwoButtonsByRole = new();
    private Dictionary<Button, PlayerInfo> buttonToPlayerInfo = new();

    [SerializeField] private Button filterAll;
    [SerializeField] private Button filterBatters;
    [SerializeField] private Button filterBowlers;
    [SerializeField] private Button filterWicketkeepers;
    [SerializeField] private Button filterAllRounders;

    [SerializeField] private InputField teamOneSearchField;
    [SerializeField] private InputField teamTwoSearchField;

    [SerializeField] private GameObject teamGenerationPanel;
    [SerializeField] private Button generateTeamBtn;
    [SerializeField] private InputField teamsToGenerateField;
    [SerializeField] private TeamGenerator teamGenerator;

    void Start()
    {
        LoadAllTeamsData();

        // Assign button click events for filtering
        filterAll.onClick.AddListener(() => ShowFilteredPlayers(PlayerRole.Default));
        filterBatters.onClick.AddListener(() => ShowFilteredPlayers(PlayerRole.Batter));
        filterBowlers.onClick.AddListener(() => ShowFilteredPlayers(PlayerRole.Bowler));
        filterWicketkeepers.onClick.AddListener(() => ShowFilteredPlayers(PlayerRole.Wicketkeeper_Batter));
        filterAllRounders.onClick.AddListener(() => ShowFilteredPlayers(PlayerRole.All_Rounder));
        generateTeamBtn.onClick.AddListener(SendPlayerList);

        teamOneSearchField.onValueChanged.AddListener((text) => FilterTeamPlayersByName(teamOnePlayersList, text));
        teamTwoSearchField.onValueChanged.AddListener((text) => FilterTeamPlayersByName(teamTwoPlayersList, text));
    }

    private void SetTeamNames(string TeamOne, string TeamTwo)
    {
        teamOneName = TeamOne;
        teamTwoName = TeamTwo;
    }

    private void SetTeamIcons(IPLTeams team, Image teamIcon)
    {
        switch (team)
        {
            case IPLTeams.Chennai:
                teamIcon.sprite = teamIconSO.chennai;
                break;

            case IPLTeams.Delhi:
                teamIcon.sprite = teamIconSO.delhi;
                break;

            case IPLTeams.Mumbai:
                teamIcon.sprite = teamIconSO.mumbai;
                break;

            case IPLTeams.Gujrat:
                teamIcon.sprite = teamIconSO.gujrat;
                break;

            case IPLTeams.Bangalore:
                teamIcon.sprite = teamIconSO.bangalore;
                break;

            case IPLTeams.Kolkata:
                teamIcon.sprite = teamIconSO.kolkata;
                break;

            case IPLTeams.Lucknow:
                teamIcon.sprite = teamIconSO.lucknow;
                break;

            case IPLTeams.Punjab:
                teamIcon.sprite = teamIconSO.punjab; 
                break;

            case IPLTeams.Rajasthan:
                teamIcon.sprite = teamIconSO.rajashthan;
                break;

            case IPLTeams.Hyderabad:
                teamIcon.sprite = teamIconSO.hyderabad;
                break;

            default:
                Debug.Log("Team sprite set to default.");
                break;
        }
    }

    void LoadAllTeamsData()
    {
        string[] teamNames = { "Chennai", "Delhi", "Gujrat", "Kolkata", "Lucknow", "Mumbai", "Punjab", "Rajasthan", "Bangalore", "Hyderabad" };

        foreach (string team in teamNames)
        {
            string filePath = $"Teams/{team}"; // Assuming files are inside Resources/Teams/
            TextAsset csvFile = Resources.Load<TextAsset>(filePath);

            if (csvFile != null)
            {
                List<PlayerData> players = ParseCSV(csvFile.text);
                teamsData[team] = players;
            }
            else
            {
                Debug.LogError($"CSV file not found: {filePath}");
            }
        }
    }

    List<PlayerData> ParseCSV(string csvContent)
    {
        List<PlayerData> players = new List<PlayerData>();
        string[] lines = csvContent.Split('\n');

        for (int i = 1; i < lines.Length; i++) // Skipping header
        {
            string[] columns = lines[i].Split(',');
            if (columns.Length >= 2)
            {
                string name = columns[0].Trim();
                PlayerRole role = GetPlayerRole(columns[1].Trim());
                float credit = -1; // Default to -1 for missing or invalid credit
                if (columns.Length == 3)
                {
                    string creditStr = columns[2].Trim();
                    if (float.TryParse(creditStr, out float temp))
                    {
                        credit = temp; // Use parsed value if valid
                    }
                }
                players.Add(new PlayerData(name, role, credit));
            }
        }

        return players;
    }

    PlayerRole GetPlayerRole(string role)
    {
        if (string.Equals(role, PlayerRole.All_Rounder.ToString()))
        {
            return PlayerRole.All_Rounder;
        }
        else
        if (string.Equals(role, PlayerRole.Batter.ToString()))
        {
            return PlayerRole.Batter;
        }
        else
        if (string.Equals(role, PlayerRole.Bowler.ToString()))
        {
            return PlayerRole.Bowler;
        }
        else
        if (string.Equals(role, PlayerRole.Wicketkeeper_Batter.ToString()))
        {
            return PlayerRole.Wicketkeeper_Batter;
        }

        return PlayerRole.Default;
    }

    public void ShowPlayerData(List<IPLTeams> teams)
    {
        SetTeamNames(teams[0].ToString(), teams[1].ToString());
        SetTeamIcons(teams[0], teamOne);
        SetTeamIcons(teams[1], teamTwo);

        teamsData.TryGetValue(teams[0].ToString(), out List<PlayerData> teamOneData);
        if (teamOneData != null)
        {
            foreach (PlayerData player in teamOneData)
            {
                PlayerInfo playerInfo = Instantiate(playerInfoPrefab, playerHolderOne.transform).GetComponent<PlayerInfo>();
                playerInfo.SetUpData(player.role, player.name, player.credit.ToString());

                Button playerButton = playerInfo.GetPlayerBtn();
                playerButton.onClick.AddListener(() => OnButtonClicked(playerButton, teamOnePlayersList, selectedTeamOnePlayers));
                teamOnePlayersList.Add(playerButton);
                buttonToPlayerInfo[playerButton] = playerInfo;

                // Categorizing by role
                if (!teamOneButtonsByRole.ContainsKey(player.role))
                {
                    teamOneButtonsByRole[player.role] = new List<Button>();
                }
                teamOneButtonsByRole[player.role].Add(playerButton);
            }
        }

        teamsData.TryGetValue(teams[1].ToString(), out List<PlayerData> teamTwoData);
        if (teamTwoData != null)
        {
            foreach (PlayerData player in teamTwoData)
            {
                PlayerInfo playerInfo = Instantiate(playerInfoPrefab, playerHolderTwo.transform).GetComponent<PlayerInfo>();
                playerInfo.SetUpData(player.role, player.name, player.credit.ToString());

                Button playerButton = playerInfo.GetPlayerBtn();
                playerButton.onClick.AddListener(() => OnButtonClicked(playerButton, teamTwoPlayersList, selectedTeamTwoPlayers));
                teamTwoPlayersList.Add(playerButton);
                buttonToPlayerInfo[playerButton] = playerInfo;

                // Categorizing by role
                if (!teamTwoButtonsByRole.ContainsKey(player.role))
                {
                    teamTwoButtonsByRole[player.role] = new List<Button>();
                }
                teamTwoButtonsByRole[player.role].Add(playerButton);
            }
        }
    }

    public void OnButtonClicked(Button clickedButton, List<Button> teamPlayersList, List<Button> selectedTeamPlayers)
    {
        int index = teamPlayersList.IndexOf(clickedButton);
        PlayerInfo playerinfo = clickedButton.GetComponentInParent<PlayerInfo>();
        if (selectedTeamPlayers.Contains(clickedButton))
        {
            // Deselect the button
            selectedTeamPlayers.Remove(clickedButton);
            playerinfo.selectedPlayer.effectColor = colorZero;

            if (selectedTeamPlayers == selectedTeamOnePlayers) 
            {
                teamOneCount.text = "Playing XI : " + selectedTeamPlayers.Count;
            }
            else
            {
                teamTwoCount.text = "Playing XI : " + selectedTeamPlayers.Count;
            }
        }
        else
        {
            if (selectedTeamPlayers.Count < 11)
            {
                // Select the button
                selectedTeamPlayers.Add(clickedButton);
                playerinfo.selectedPlayer.effectColor = colorMax;

                if (selectedTeamPlayers == selectedTeamOnePlayers)
                {
                    teamOneCount.text = "Playing XI : " + selectedTeamPlayers.Count;
                }
                else
                {
                    teamTwoCount.text = "Playing XI : " + selectedTeamPlayers.Count;
                }
            }
        }

        bool enable = selectedTeamOnePlayers.Count == 11 && selectedTeamTwoPlayers.Count == 11;
        teamGenerationPanel.SetActive(enable);
    }

    void ShowFilteredPlayers(PlayerRole role)
    {
        // Show All Players
        if (role == PlayerRole.Default)
        {
            foreach (var button in teamOnePlayersList) buttonToPlayerInfo[button].gameObject.SetActive(true);
            foreach (var button in teamTwoPlayersList) buttonToPlayerInfo[button].gameObject.SetActive(true);
            return;
        }

        // Hide All Players
        foreach (var button in teamOnePlayersList) buttonToPlayerInfo[button].gameObject.SetActive(false);
        foreach (var button in teamTwoPlayersList) buttonToPlayerInfo[button].gameObject.SetActive(false);

        // Show Only Selected Role Players
        if (teamOneButtonsByRole.ContainsKey(role))
        {
            foreach (var button in teamOneButtonsByRole[role]) buttonToPlayerInfo[button].gameObject.SetActive(true);
        }
        if (teamTwoButtonsByRole.ContainsKey(role))
        {
            foreach (var button in teamTwoButtonsByRole[role]) buttonToPlayerInfo[button].gameObject.SetActive(true);
        }
    }

    private void FilterTeamPlayersByName(List<Button> teamList, string searchText)
    {
        string lowerSearch = searchText.Trim().ToLower();

        foreach (var button in teamList)
        {
            PlayerInfo playerInfo = buttonToPlayerInfo[button];
            bool shouldShow = string.IsNullOrEmpty(lowerSearch) || playerInfo.playerName.ToLower().Contains(lowerSearch);
            playerInfo.gameObject.SetActive(shouldShow);
        }
    }

    public void SendPlayerList()
    {
        if (string.IsNullOrEmpty(teamsToGenerateField.text) || string.IsNullOrWhiteSpace(teamsToGenerateField.text))
            return;

        int teams = int.Parse(teamsToGenerateField.text);
        teamGenerator.GetTeamsData(teams, selectedTeamOnePlayers, selectedTeamTwoPlayers,teamOneName, teamTwoName);
    }
}

[Serializable]
public class PlayerData
{
    public string name;
    public PlayerRole role;
    public double credit;
    public bool isCaptain;
    public bool isVCaptain;

    public PlayerData(string name, PlayerRole role, double credit, bool isCaptain = false, bool isVCaptain = false)
    {
        this.name = name;
        this.role = role;
        this.credit = credit;
        this.isCaptain = isCaptain;
        this.isVCaptain = isVCaptain;
    }
}

public enum PlayerRole
{
    Default,
    Wicketkeeper_Batter,
    Batter,
    All_Rounder,
    Bowler
}
