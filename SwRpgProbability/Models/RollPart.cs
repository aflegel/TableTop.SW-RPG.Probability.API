using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwRpgProbability.Models
{
	[Table("RollPart")]
	class RollPart
	{
		byte faceId { get; set; }

		int rollMasterId { get; set; }
	}
}
