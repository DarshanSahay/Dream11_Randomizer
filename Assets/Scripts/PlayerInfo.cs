using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public Image roleImage;
    public Sprite batterIcon;
    public Sprite bowlerIcon;
    public Sprite wicketKeeper_BatterIcon;
    public Sprite allRounderIcon;

    public Text playerNameText;
    public Text playerCreditText;

    public Outline selectedPlayer;
    public Button playerBtn;

    public string playerName;
    public double playerCredit;
    public int playerRoleCredit;
    public PlayerRole role;

    public bool isCaptain;
    public bool isVCaptain;
    public Text impRoleText;

    public PlayerInfo(PlayerRole role, string name, string credit)
    {
        //roleImage.sprite = GetPlayerIcon(role);
        playerName = name;
        playerCredit = double.Parse(credit);
        playerNameText.text = name;
        playerCreditText.text = credit;
    }

    public void SetUpData(PlayerRole role, string name, string credit)
    {
        roleImage.sprite = GetPlayerIcon(role);
        playerName = name;
        playerCredit = double.Parse(credit);
        playerNameText.text = name;
        playerCreditText.text = credit;
    }

    public Button GetPlayerBtn()
    {
        return playerBtn;
    }

    private Sprite GetPlayerIcon(PlayerRole role)
    {
        switch (role)
        {
            case PlayerRole.Batter:
                playerRoleCredit = 2;
                return batterIcon;
                break;

            case PlayerRole.Bowler:
                playerRoleCredit = 4;
                return bowlerIcon;
                break;

            case PlayerRole.Wicketkeeper_Batter:
                playerRoleCredit = 1;
                return wicketKeeper_BatterIcon;
                break;

            case PlayerRole.All_Rounder:
                playerRoleCredit = 3;
                return allRounderIcon;
                break;

            default:
                return null;
                break;
        }
    }

    public void SetCaptain()
    {
        isCaptain = true;
        impRoleText.text = "C";
    }

    public void SetVCaptain()
    {
        isVCaptain = true;
        impRoleText.text = "VC";
    }
}
