using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseTeam : MonoBehaviour
{
    static List<IPLTeams> playingTeams = new List<IPLTeams>();
    public List<Image> selectedImage = new List<Image>(); // Assign buttons in the Inspector
    public List<Button> buttons = new List<Button>(); // Assign buttons in the Inspector

    [SerializeField] private List<Button> selectedButtons = new List<Button>();
    [SerializeField] private Color colorZero;
    [SerializeField] private Color colorMax;

    [SerializeField] private Image teamOne;
    [SerializeField] private Image teamTwo;
    [SerializeField] private Sprite blank;

    public Dictionary<Button, IPLTeams> teamData = new Dictionary<Button, IPLTeams>();
    [SerializeField] List<IPLTeams> teamsOrder;
    [SerializeField] List<Sprite> teamIcons;

    [SerializeField] private GameObject continuePanel;
    [SerializeField] private Button continueBtn;

    [SerializeField] private GameObject playerSelectionPanel;
    [SerializeField] private LoadTeamsData loadTeamsData;

    void Start()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClicked(button));
            teamData.Add(button, teamsOrder[buttons.IndexOf(button)]);
        }

        continueBtn.onClick.AddListener(OpenTeamSelectionPanel);
    }

    public void OnButtonClicked(Button clickedButton)
    {
        int index = buttons.IndexOf(clickedButton);
        if (selectedButtons.Contains(clickedButton))
        {
            // Deselect the button
            selectedButtons.Remove(clickedButton);
            selectedImage[index].color = colorZero;
            RemoveVersus(teamIcons[index]);
            TeamSelected(hasSelected: false);
        }
        else
        {
            if (selectedButtons.Count < 2)
            {
                // Select the button
                selectedButtons.Add(clickedButton);
                selectedImage[index].color = colorMax;
                
                SetVersus(teamIcons[index]);

                if(selectedButtons.Count == 2)
                {
                    TeamSelected(hasSelected: true);
                }
            }
        }
    }

    public void SetVersus(Sprite icon)
    {
        if(teamOne.sprite == blank)
        {
            teamOne.sprite = icon;
        }
        else
        if(teamTwo.sprite == blank)
        {
            teamTwo.sprite = icon;
        }
    }

    public void RemoveVersus(Sprite icon)
    {
        if (teamOne.sprite == icon)
        {
            teamOne.sprite = blank;
        }
        else
        if (teamTwo.sprite == icon)
        {
            teamTwo.sprite = blank;
        }
    }

    public void TeamSelected(bool hasSelected)
    {
        bool enableContinue = hasSelected ? true : false;
        continuePanel.SetActive(enableContinue);
    }

    public void OpenTeamSelectionPanel()
    {
        foreach (Button button in selectedButtons)
        {
            teamData.TryGetValue(button, out IPLTeams team);
            playingTeams.Add(team);
        }

        playerSelectionPanel.SetActive(true);
        loadTeamsData.ShowPlayerData(playingTeams);
    }
}

public enum IPLTeams
{
    Chennai,
    Delhi,
    Mumbai,
    Gujrat,
    Bangalore,
    Kolkata,
    Lucknow,
    Punjab,
    Rajasthan,
    Hyderabad
}
