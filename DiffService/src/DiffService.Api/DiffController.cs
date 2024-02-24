using DiffService.src.models;
using Microsoft.AspNetCore.Mvc;

namespace DiffService.src.DiffService.Api
{
    [ApiController]
    [Route("v1/diff")]
    public class DiffController : ControllerBase
    {
        private static readonly Dictionary<string, (byte[], byte[])> dataStore;

        static DiffController()
        {
            dataStore = new Dictionary<string, (byte[], byte[])>();
        }

        [HttpPost]
        [Route("{ID}/left")]
        public IActionResult Left(string ID, [FromBody] DiffData diffData)
        {
            try
            {
                string dataString = diffData.Data;
                // Adding padding characters if necessary
                int padding = (4 - dataString.Length % 4) % 4;
                dataString += new string('=', padding);

                byte[] data = Convert.FromBase64String(dataString);
                if (!dataStore.TryGetValue(ID, out (byte[], byte[]) value))
                {
                    dataStore.Add(ID, (data, null));
                    dataStore[ID] = (null, data);
                }
                else
                {
                    dataStore[ID] = (value.Item1, data);
                }

                return StatusCode(201, "Created");
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPost]
        [Route("{ID}/right")]
        public IActionResult Right(string ID, [FromBody] DiffData diffData)
        {
            try
            {
                string dataString = diffData.Data;
                // Adding padding characters if necessary
                int padding = (4 - dataString.Length % 4) % 4;
                dataString += new string('=', padding);

                byte[] data = Convert.FromBase64String(dataString);
                if (!dataStore.TryGetValue(ID, out (byte[], byte[]) value))
                {
                    dataStore.Add(ID, (null, data));
                    dataStore[ID] = (null, data);
                }
                else
                {
                    dataStore[ID] = (value.Item2, data);
                }

                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{ID}")]
        public IActionResult Diff(string ID)
        {
            try
            {
                if (!dataStore.TryGetValue(ID, out (byte[], byte[]) value))
                    return NotFound();

                var leftData = value.Item1;
                var rightData = value.Item2;

                if (leftData == null || rightData == null)
                    return StatusCode(400);

                if (leftData.Length != rightData.Length)
                    return Ok(new { diffResultType = "SizeDoNotMatch" });

                var diffs = new List<Difference>();
                int length = 0;
                int offset = -1; // Initialize offset to -1 to handle diffs at the beginning of the data

                for (int i = 0; i < leftData.Length; i++)
                {
                    if (leftData[i] != rightData[i])
                    {
                        if (length == 0)
                        {
                            // Start of a new diffs
                            offset = i;
                        }
                        length++;
                    }
                    else
                    {
                        if (length > 0)
                        {
                            // End of a difference
                            diffs.Add(new Difference { Offset = offset, Length = length });
                            length = 0;
                        }
                        offset = -1; // Reset offset
                    }
                }

                if (length > 0)
                {
                    // If there's a difference at the end of the data
                    diffs.Add(new Difference { Offset = offset, Length = length });
                }

                if (diffs.Count == 0)
                {
                    // If there are no differences
                    return Ok(new { diffResultType = "Equals" });
                }

                return Ok(new { diffResultType = "ContentDoNotMatch", diffs });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
