using System.Threading.Tasks;
using Thirdweb;
using UnityEngine;

public class TokenClaimer : MonoBehaviour
{
    private ThirdwebSDK sdk;

    public GameObject balanceText;

    public GameObject claimButton;

    async void OnEnable()
    {
        sdk =
            new ThirdwebSDK("goerli",new ThirdwebSDK.Options()
                {
                    gasless =
                        new ThirdwebSDK.GaslessOptions()
                        {
                            openzeppelin =
                                new ThirdwebSDK.OZDefenderOptions()
                                {
                                    relayerUrl =
                                        "https://api.defender.openzeppelin.com/autotasks/41148a44-dd74-47e1-bff1-e06133a51a17/runs/webhook/0dfe2c75-0aeb-4f38-8647-581b8b035750/G4M3pwkfzodxgz6arzeDzp"
                                }
                        }
                });

        string wallet = GetWalletKey();
        if(string.Equals(wallet, "Coinbase")) {
            await Coinbase();
        } else {
            await Metamask();
        }
        CheckBalance();
    }

     public string GetWalletKey()
    {
        return PlayerPrefs.GetString("SelectedProvider");
    }

    public async void Claim()
    {
        // Update claim button text
        claimButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text =
            "Claiming...";

        await getTokenDrop().ERC20.Claim("25");

        // hide claim button
        claimButton.SetActive(false);

        CheckBalance();
    }

    private Contract getTokenDrop()
    {
        return sdk.GetContract("0x559d9a4544c6B3Bc8d21595DA02fc6EEcf0af112");
    }

    private async void CheckBalance()
    {
        // Set text to user's balance
        var bal = await getTokenDrop().ERC20.Balance();

        balanceText.GetComponent<TMPro.TextMeshProUGUI>().text =
            bal.displayValue + " " + bal.symbol;
    }

    public async Task<string> WalletConnect()
    {
        string address =
            await sdk
                .wallet
                .Connect(new WalletConnection()
                {
                    provider = WalletProvider.WalletConnect,
                    chainId = 420 // Switch the wallet Goerli network on connection
                });
        return address;
    }

    public async Task<string> Metamask()
    {
        string address =
            await sdk
                .wallet
                .Connect(new WalletConnection()
                {
                    provider = WalletProvider.MetaMask,
                    chainId = 5 // Switch the wallet Goerli network on connection
                });
        return address;
    }

    public async Task<string> Coinbase()
    {
        string address =
            await sdk
                .wallet
                .Connect(new WalletConnection()
                {
                    provider = WalletProvider.CoinbaseWallet,
                    chainId = 5 // Switch the wallet Goerli network on connection
                });
        return address;
    }
}