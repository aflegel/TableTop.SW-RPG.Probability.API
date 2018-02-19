using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiceCalculator.Models
{
	class PoolMaster
	{
		int poolMasterId { get; set; }

		int uniqueRolls { get; set; }
		int totalRolls { get; set; }

		decimal successRate { get; set; }
		decimal failureRate { get; set; }
		decimal advantageRate { get; set; }
		decimal staleRate { get; set; }
		decimal threatRate { get; set; }
	}
}
