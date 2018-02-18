using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiceCalculator.Models
{
	class RollMaster
	{
		int rollMasterId { get; set; }
		int poolMasterId { get; set; }
		long rollCount { get; set; }
	}
}
