using AutoMapper;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Threading.Tasks;
using WebApi.Core;
using WebApi.Core.DomainModel.Dto;
using WebApi.Core.DomainModel.Entities;
using WebApi.Core.Misc;

namespace WebApi.Controllers; 
[Route("banking/owners")]
[ApiController]
public class OwnersController(
   // Dependency injection
   IOwnersRepository ownersRepository,
   IDataContext dataContext,
   IMapper mapper,
   ILogger<OwnersController> logger
) : ControllerBase {
   
   // Get all owners as Dtos
   // http://localhost:5100/banking/owners
   [HttpGet("")]
   public async Task<ActionResult<IEnumerable<OwnerDto>>> GetOwners() {
      logger.LogDebug("GetOwners()");
      
      // get all owners
      var owners = await ownersRepository.SelectAsync();
      
      // return owners as Dtos
      var ownerDtos = mapper.Map<IEnumerable<OwnerDto>>(owners);
      return Ok(ownerDtos);  
   }
   
   // Get owner by Id as Dto
   // http://localhost:5100/banking/owners/{id}
   [HttpGet("{id}")]
   public async Task<ActionResult<OwnerDto>> GetOwnerById(
      [FromRoute] Guid id
   ) {
      logger.LogDebug("GetOwnerById() id={id}", id.As8());
      switch (await ownersRepository.FindByIdAsync(id)) {
         // return owner as Dto
         case Owner owner: 
            return Ok(mapper.Map<OwnerDto>(owner));
         // return not found
         case null:        
            return NotFound("Owner with given Id not found");
      }
   }

   // Get owners by name as Dto
   // http://localhost:5100/banking/owners/name
   [HttpGet("name")]
   public async Task<ActionResult<IEnumerable<OwnerDto>>> GetOwnersByName(
      [FromQuery] string name
   ) {
      logger.LogDebug("GetOwnersByName() name={name}", name);
      switch (await ownersRepository.SelectByNameAsync(name)) {
         // return owners as Dtos
         case IEnumerable<Owner> owners: 
            return Ok(mapper.Map<IEnumerable<Owner>, IEnumerable<OwnerDto>>(owners));
         // return not found
         case null:
            return NotFound("Owners with given name not found");
      }
   }

   // Get owner by email as Dto
   // http://localhost:5100/banking/owners/email
   [HttpGet("owners/email")]
   public async Task<ActionResult<OwnerDto?>> GetOwnerByEmail(
      [FromQuery] string email
   ) {
      switch (await ownersRepository.FindByEmailAsync(email)) {
         // return owner as Dto
         case Owner owner: 
            return Ok(mapper.Map<OwnerDto>(owner));
         // return not found
         case null:        
            return NotFound("Owner with given email not found");
      }
   }

   // Get owners by birthdate as Dtos
   // http://localhost:5100/banking/owners/birthdate/?from=yyyy-MM-dd&to=yyyy-MM-dd
   [HttpGet("birthdate")]
   public async Task<ActionResult<IEnumerable<OwnerDto>>> GetOwnerByBirthdate(
      [FromQuery] string from,   // Date must be in the format yyyy-MM-dd
                                 // MM = 01 for January through 12 for December
      [FromQuery] string to      
   ) {
      logger.LogDebug("GetOwnerByBirthdate() from={from} to={to}", from, to);

      // Convert string to DateTime
      var (errorFrom, dateFrom) = ConvertToDateTime(from);
      if(errorFrom) 
         return BadRequest($"GetOwnerByBirthdate: Invalid date 'from': {from}");
      var (errorTo, dateTo) = ConvertToDateTime(to);
      if(errorTo) 
         return BadRequest($"GetOwnerByBirthdate: Invalid date 'to': {to}");

      // Get owners by birthdate
      var owners = await ownersRepository.SelectByBirthDateAsync(dateFrom, dateTo);   
      
      // return owners as Dtos
      return Ok(mapper.Map<IEnumerable<OwnerDto>>(owners));
   }
   
   // Convert string in German format dd.MM.yyyy to DateTime
   private (bool, DateTime) ConvertToDateTime(string date) {
      try {
         var dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
         return (false, dateTime );
      } catch {
         return (true, DateTime.MinValue);
      }
   }
   
   // Create a new owner
   // http://localhost:5100/banking/owners
   [HttpPost("")]
   public async Task<ActionResult<Owner>> CreateOwner(
      [FromBody] OwnerDto ownerDto
   ) {
      logger.LogDebug("CreateOwner() ownerDto={ownerDto}", ownerDto.Name);
      
      // map ownerDto to owner
      var owner = mapper.Map<Owner>(ownerDto);
      
      // check if owner with given Id already exists   
      if(await ownersRepository.FindByIdAsync(owner.Id) != null) 
         return BadRequest("CreateOwner: Owner with the given id already exists");
      
      // add owner to repository
      ownersRepository.Add(owner); 
      // save to datastore
      await dataContext.SaveAllChangesAsync();
      
      // return created owner      
      var uri = new Uri($"{Request.Path}/{owner.Id}", UriKind.Relative);
      return Created(uri, mapper.Map<OwnerDto>(owner));     
   }
   
   // Update owner
   // http://localhost:5100/banking/owners/{id}
   [HttpPut("{id:Guid}")] 
   public async Task<ActionResult<OwnerDto>> UpdateOwner(
      [FromRoute] Guid id,
      [FromBody]  OwnerDto updOwnerDto
   ) {
      logger.LogDebug("UpdateOwner() id={id} updOwnerDto={updOwnerDto}", id.As8(), updOwnerDto.Name);
      
      var updOwner = mapper.Map<Owner>(updOwnerDto);

      // check if Id in the route and body match
      if(id != updOwner.Id) 
         return BadRequest("UpdateOwner: Id in the route and body do not match.");
      
      // check if owner with given Id exists
      Owner? owner = await ownersRepository.FindByIdAsync(id);
      if (owner == null)
         return NotFound("UpdateOwner: Owner with given id not found.");

      // Update person
      owner.Update(updOwner.Name, updOwner.Email);
      
      // save to repository 
      await ownersRepository.UpdateAsync(owner);
      // write to database
      await dataContext.SaveAllChangesAsync();

      // return updated owner
      return Ok(mapper.Map<OwnerDto>(updOwner)); 
   }
   
   // Delete owner
   // http://localhost:5100/banking/owners/{id}
   [HttpDelete("{id:Guid}")] 
   public async Task<ActionResult<Owner>> DeleteOwner(
      [FromRoute] Guid id
   ) {
      logger.LogDebug("DeleteOwner {id}", id.As8());
      
      // check if owner with given Id exists
      Owner? owner = await ownersRepository.FindByIdAsync(id);
      if (owner == null)
         return NotFound("DeleteOwner: Owner with given id not found.");

      // remove in repository 
      ownersRepository.Remove(owner);
      // write to database
      await dataContext.SaveAllChangesAsync();

      // return no content
      return NoContent(); 
   }
}