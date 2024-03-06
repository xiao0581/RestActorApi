using ActorRepositoryLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestActorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private IActorsRepository _actorsRepository;
        //  motager det actor fra
        public ActorsController(IActorsRepository actorsRepository)
        {
            _actorsRepository = actorsRepository;
        }
        // GET: api/<ActorsController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public ActionResult<IEnumerable<Actor>> Get(
            [FromHeader] string? birthdayAfter)
        {
            IEnumerable<Actor> actorlist= _actorsRepository.Get(null,null,null);
            if(birthdayAfter != null)
            {    //
                if (int.TryParse(birthdayAfter, out int bir))
                {

                    actorlist = _actorsRepository.Get(birthdayAfter: bir);
                }
                else
                {
                    return BadRequest("birthdayAfter is not a number");
                }
               
            }
            else
            {
               return BadRequest("birthdayAfter is null");
            }
            if (actorlist.Any())
            {
                Response.Headers.Add("TotalCount", "" + actorlist.Count());
                return Ok(actorlist);
            }
            else {
                return BadRequest("actor not found");
            }
            
        }

        // GET api/<ActorsController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Actor> Get(int id)
        {
            Actor? actor = _actorsRepository.GetById(id);
            if (actor == null)
            {
                return NotFound("No such class, id: \"" + id);
            }
            else
            {
                return Ok(actor);
            }
        }
        // POST api/<ActorsController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        [HttpPost]
        public ActionResult<Actor> Post([FromBody] Actor actor)
        {
         
            try
            {

                Actor addedActor = _actorsRepository.Add(actor);
                return Created("/" + addedActor.Id, addedActor);

            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("null");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest("out of range");
            }
            catch (Exception ex)
            {
                return BadRequest("all fault");
            }

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        
        public ActionResult<Actor> Put(int id, [FromBody] Actor actor)
        {
            try
            {
                Actor? updatedActor = _actorsRepository.Update(id, actor);
                if (updatedActor == null) return NotFound();
                else return Ok(updatedActor);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE api/<ActorsController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public ActionResult<Actor> Delete(int id)
        {
            Actor? deletActor = _actorsRepository.Delete(id);
            if (deletActor == null) return NotFound();
            else return Ok(deletActor);
        }
    }
}
