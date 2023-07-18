using System.Threading.Tasks;
using Thirdweb;
using UnityEngine;

namespace RPGM.Core
{
    public class Web3 : MonoBehaviour
    {
        private ThirdwebSDK
            sdk =
                new ThirdwebSDK("https://rpc.testnet.mantle.xyz");

        public async Task<bool> IsConnected()
        {
            return await sdk.wallet.IsConnected();
        }

        public async Task<string> Connect()
        {
            string addr =
                await sdk
                    .wallet
                    .Connect(new WalletConnection()
                    {
                        provider = WalletProvider.WalletConnect, // Use Coinbase Wallet
                        // chainId = 5001 // Switch the wallet Mantle testnet network on connection
                    });
            return addr;
        }

        public Contract GetTokenDropContract()
        {
            return sdk
                .GetContract("0xe33CFA21806A8678d047F7b240e506571380C22d");
                // MMQ COIN
        }

        public async Task<TransactionResult> Claim()
        {
            await Connect();
            var contract = GetTokenDropContract();
            return await contract.ERC20.Claim("10");
        }

        public Marketplace GetMarketplaceContract()
        {
            return sdk
                .GetContract("0x424832c400e69230E3553Aed59e0eF9A605Ea195")
                .marketplace;
                // MMQ Marketplace
        }

        public async Task<TransactionResult> BuyItem(string itemId)
        {
            await Connect();
            return await GetMarketplaceContract().BuyListing(itemId, 1);
        }

        internal async Task<string> GetAddress()
        {
            if (Application.isEditor)
            {
                return "0x0000000000000000000000000000000000000000";
            }
            return await sdk.wallet.GetAddress();
        }
    }
}
