using ATMAPPAPI.Models.DTOs;
using System.Text.Json;

namespace ATMAPPAPI.Repositoris
{
    public class CardOperations:ICardOperations
    {
        public async Task<CardInfoDTO> FindCardInfoAsync(string searchType, string searchValue)
        {
            // Path to your cardinfo.json file
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "cardinfo.json");

            // Read the JSON file
            string jsonData = await File.ReadAllTextAsync(filePath);

            // Deserialize the JSON data
            var cardInfoList = JsonSerializer.Deserialize<List<CardInfoDTO>>(jsonData);

            // Search for the card info based on the search type and value
            CardInfoDTO cardInfo = null;

            if (searchType.Equals("cardNumber", StringComparison.OrdinalIgnoreCase))
            {
                cardInfo = cardInfoList?.FirstOrDefault(c => c.CardNumber == searchValue);
            }
            else if (searchType.Equals("accountNumber", StringComparison.OrdinalIgnoreCase))
            {
                cardInfo = cardInfoList?.FirstOrDefault(c => c.AccountNumber == searchValue);
            }

            return cardInfo;
        }
        public async Task UpdateCardInfoAsync(CardInfoDTO updatedCardInfo)
        {
            // Path to your cardinfo.json file
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "cardinfo.json");

            // Read the JSON file
            string jsonData = await File.ReadAllTextAsync(filePath);

            // Deserialize the JSON data
            var cardInfoList = JsonSerializer.Deserialize<List<CardInfoDTO>>(jsonData);

            // Find the index of the card info to update
            var index = cardInfoList.FindIndex(c => c.AccountNumber == updatedCardInfo.AccountNumber);

            if (index != -1)
            {
                // Update the card info in the list
                cardInfoList[index] = updatedCardInfo;

                // Serialize the updated list back to JSON
                string updatedJsonData = JsonSerializer.Serialize(cardInfoList);

                // Write the updated JSON data back to the file
                await File.WriteAllTextAsync(filePath, updatedJsonData);
            }
            else
            {
                throw new InvalidOperationException("Card information not found for update.");
            }
        }
    }
}
