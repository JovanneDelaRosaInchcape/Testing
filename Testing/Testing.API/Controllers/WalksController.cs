using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Testing.API.Models.DTO;
using Testing.API.Repositories;

namespace Testing.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, 
            IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //fetch Data From Datbase - domain walks
            var walksDomain = await walkRepository.GetAllAsync();

            //Convert domain walks to DTo walks

            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            //return response

            return Ok(walksDTO);
        }


        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("{GetWalkAsync}")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get walk Domain Object from DataBase
            var walkDomain = await walkRepository.GetAsync(id);

            // Convert domain Object to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            // return Response 
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalksAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            // Validate the incoming request
            if (!(await ValidateAddWalksAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            //convert DTO to Domain Object
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };
            // Pass domain Object to Repository to persist this
            walkDomain = await walkRepository.AddAsync(walkDomain);
            // Convert The Domian ObjectBack to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
            };
            //Send DTO Response back to Client

            return CreatedAtAction(nameof(GetWalkAsync), new {id = walkDTO.Id}, walkDTO);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public  async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
            [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {

            //Validate the incoming request
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            // Convert DTO to Domain Object
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
            };

            //Pass the details to repository - Get Domain object in response(or null)
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            //Handle Null (not found)
            if (walkDomain == null)
            {
                return NotFound();
            }
           
                //Convert backDomain to DTO

                var walkDTO = new Models.DTO.Walk
                {
                    Id = walkDomain.Id,
                    Length = walkDomain.Length,
                    Name = walkDomain.Name,
                    RegionId = walkDomain.RegionId,
                    WalkDifficultyId = walkDomain.WalkDifficultyId,

                };

        
            //Return Response

            return Ok(walkDTO);
        }


        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DaleteWalkAsync(Guid id)
        {
            //call repository to delete walk
           var walkDomain = await  walkRepository.DeleteAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);

        }

        #region Private methods

        private async Task<bool>  ValidateAddWalksAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest),
                   $"{nameof(addWalkRequest)} cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name),
                   $"{nameof(addWalkRequest.Name)} is required.");
               
            }

            if (addWalkRequest.Length > 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length),
                   $"{nameof(addWalkRequest.Length)}should be greater than 0.");
               
            }

            var region = regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                   $"{nameof(addWalkRequest.RegionId)} is invalid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                   $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest),
                   $"{nameof(updateWalkRequest)} cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name),
                   $"{nameof(updateWalkRequest.Name)} is required.");

            }

            if (updateWalkRequest.Length > 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length),
                   $"{nameof(updateWalkRequest.Length)}should be greater than 0.");

            }

            var region = regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                   $"{nameof(updateWalkRequest.RegionId)} is invalid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                   $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
