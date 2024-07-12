using ATMAPPAPI.Models.DTOs;

namespace ATMAPPAPI.Repositoris
{
    public interface ICardOperations
    {
        public Task<CardInfoDTO> FindCardInfoAsync(string searchType, string searchValue);
        public Task UpdateCardInfoAsync(CardInfoDTO updatedCardInfo);
    }
}
