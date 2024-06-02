using Play.Catalogue.Service.Dtos;
using Play.Catalogue.Service.Entities;

namespace Play.Catalogue.Service;

public static class Extensions
{
    public static ItemDto ItemToItemDto(this Item item)
    {
        return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
    }
}