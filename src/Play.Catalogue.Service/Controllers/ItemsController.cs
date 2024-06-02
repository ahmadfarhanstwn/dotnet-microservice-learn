using Microsoft.AspNetCore.Mvc;
using Play.Catalogue.Service.Dtos;
using Play.Catalogue.Service.Entities;
using Play.Catalogue.Service.Repositories;

namespace Play.Catalogue.Service.Controller;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private readonly ItemsRepository _itemsRepository = new();

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> Get()
    {
        var items = (await _itemsRepository.GetAllAsync()).Select(item => item.ItemToItemDto());
        return items;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetById(Guid id)
    {
        var item = (await _itemsRepository.GetByIdAsync(id)).ItemToItemDto();
        if (item is null)
        {
            return NotFound();
        }

        return item;
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> Post(CreateItemDto itemDto)
    {
        Item itemEntity = new Item
        {
            Name = itemDto.Name,
            Description = itemDto.Description,
            Price = itemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };
        await _itemsRepository.CreateAsync(itemEntity);
        return CreatedAtAction(nameof(GetById), new{id = itemEntity.Id}, itemEntity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, UpdateItemDto updatedItemDto)
    {
        var existingItem = await _itemsRepository.GetByIdAsync(id);

        if (existingItem is null)
        {
            return NotFound();
        }

        existingItem.Name = updatedItemDto.Name;
        existingItem.Description = updatedItemDto.Description;
        existingItem.Price = updatedItemDto.Price;

        await _itemsRepository.UpdateAsync(existingItem);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var existingItem = await _itemsRepository.GetByIdAsync(id);

        if (existingItem is null)
        {
            return NotFound();
        }

        await _itemsRepository.RemoveAsync(id);
        return NoContent();
    }
}