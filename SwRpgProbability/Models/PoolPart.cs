using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwRpgProbability.Models
{
	class PoolPart
	{
		int poolMasterId { get; set; }

		int die { get; set; }
		int dieCount { get; set; }

	}
}
